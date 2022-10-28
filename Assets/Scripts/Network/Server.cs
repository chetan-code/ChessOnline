using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Networking.Transport;
using UnityEngine;

public class Server : MonoBehaviour
{
    #region Singleton
    public static Server Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
    }
    #endregion

    public NetworkDriver Driver;
    private NativeList<NetworkConnection> _connections;

    //This is used to ping clients after every 20sec. to make sure it is connected
    private bool _isActive = false;
    private const float KeepAliveTickRate = 20.0f;
    private float _lastKeepAlive;

    public Action ConnectionDropped;

    /// <summary>
    /// Initalising server - by creating driver
    /// and binding it to an endpoint which has info of port,protocol etc
    /// allow driver to listen to any connections/messages through port
    /// We also initialise native list of connections - just 2 participants in chess
    /// </summary>
    /// <param name="port">port through which we will listen</param>

    public void Init(ushort port)
    {

        Driver = NetworkDriver.Create();
        //endpoint class has info like ip , protocol, port etc
        NetworkEndPoint endpoint = NetworkEndPoint.AnyIpv4;
        endpoint.Port = port;

        //binding driver to a endpoint - 0 if we failed to bind to the port - else bind successful
        if (Driver.Bind(endpoint) != 0)
        {
            //unable to bind to specified endpoint(port/ip etc)
            Debug.Log("Unable to bind on port " + endpoint.Port);
            return;
        }
        else
        {
            //make driiver now listen to any upcoming connections (from clients) at specified ports
            Driver.Listen(); //driver is in a listening state
            Debug.Log("Currently listening on port " + endpoint.Address + " " + endpoint.Port);

        }

        _connections = new NativeList<NetworkConnection>(2, Allocator.Persistent);
        _isActive = true;

        Debug.Log("Server init() completed");
    }



    /// <summary>
    /// Shuting down server - by disposing driver 
    /// and connections(other connected devices)
    /// </summary>
    public void ShutDown()
    {
        if (_isActive)
        {
            Driver.Dispose();
            _connections.Dispose();
            _isActive = false;
            Debug.Log("Server shutdown() completed");
        }
    }

    private void OnDestroy()
    {
        ShutDown();
    }


    private void Update()
    {
        if (!_isActive) { return; }

        KeepAlive();

        #region explanation
        //We call the driver's ScheduleUpdate method on every frame.
        //This is so we can update the state of each connection we have active
        //to make sure we read all data that we have received and
        //finally produce events that the user can react to
        //more details - https://docs.unity3d.com/Packages/com.unity.transport@0.2/manual/update-flow.html
        #endregion
        Driver.ScheduleUpdate().Complete();



        CleanupConnections();
        AcceptNewConnections();
        UpdateMessagePump();
    }
    /// <summary>
    ///we iterate through our connection list and
    ///just simply remove any stale connections.
    /// </summary>
    private void CleanupConnections()
    {
        for (int i = 0; i < _connections.Length; i++)
        {
            //if connection dropped 
            if (!_connections[i].IsCreated)
            {
                _connections.RemoveAtSwapBack(i);
                --i;//we removed connection from list so we need to reduce the i
            }
        }
    }

    /// <summary>
    /// we add a connection 
    /// while there are new connections to accept.
    /// </summary>
    private void AcceptNewConnections()
    {
        NetworkConnection newConnection;
        //the new connection is not a default value 
        while ((newConnection = Driver.Accept()) != default(NetworkConnection))
        {
            _connections.Add(newConnection);
        }

    }

    /// <summary>
    /// Now we have an up-to-date connection list.
    /// We can now start querying the driver for events 
    /// that might have happened since the last update.
    /// </summary>
    private void UpdateMessagePump()
    {
        DataStreamReader stream;
        for (int i = 0; i < _connections.Length; i++)
        {
            NetworkEvent.Type cmd;
            //pop event that has happened for specified connection - so we can read it
            //not an empty event we recieved something
            while ((cmd = Driver.PopEventForConnection(_connections[i], out stream)) != NetworkEvent.Type.Empty)
            {

                //we recieved some data
                if (cmd == NetworkEvent.Type.Data)
                {
                    //we read data
                    NetUtility.OnData(stream, _connections[i], this);
                }
                else if (cmd == NetworkEvent.Type.Disconnect)
                {
                    //we recieved an event that client has disconnect for some reason
                    Debug.Log("Client disconnected from server");
                    //reset value of connection to default value of class NetworkConnection
                    _connections[i] = default(NetworkConnection);
                    ConnectionDropped?.Invoke();
                    //since it is two player game - if client disconnect (only one player remains)- we need to shutdown sever
                    //this doesnt happen generally
                    ShutDown();
                }
            }
            //Debug.Log("Server UpdateMessagePump() : event type : " + eventType.ToString());
        }
    }


    //Server specific
    /// <summary>
    /// To broadcast a message to all the cliennt connected to server
    /// </summary>
    /// <param name="msg"></param>
    public void Broadcast(NetMessage msg)
    {
        for (int i = 0; i < _connections.Length; i++)
        {
            //if coonnection hasn't dropped 
            if (_connections[i].IsCreated)
            {
                Debug.Log($"Sending {msg.Code} to : {_connections[i].InternalId}");
                SendToClient(_connections[i], msg);
            }
        }
    }

    /// <summary>
    /// Serialise data to send it over network to a client
    /// </summary>
    /// <param name="connection">connection establised with client</param>
    /// <param name="msg">message that need to be transmitted</param>
    public void SendToClient(NetworkConnection connection, NetMessage msg)
    {
        //The DataStreamWriter and DataStreamReader classes work together
        //to serialize data for sending and then to deserialize when receiving.
        DataStreamWriter writer;
        Driver.BeginSend(connection, out writer);
        msg.Serialize(ref writer);
        ////The writer needs to be Disposed
        //because it uses native memory which needs to be freed.
        Driver.EndSend(writer);
        Debug.Log("SendToClient() completed");
    }

    private void KeepAlive()
    {
        if (Time.time - _lastKeepAlive > KeepAliveTickRate)
        {
            _lastKeepAlive = Time.time;
            Broadcast(new NetKeepAlive());
        }
    }
}
