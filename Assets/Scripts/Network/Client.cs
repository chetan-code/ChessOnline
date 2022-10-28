using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Networking.Transport;
using UnityEngine;

public class Client : MonoBehaviour
{
    #region singleton
    public static Client Instance { set; get; }
    private void Awake()
    {
        Instance = this;
    }
    #endregion

    public NetworkDriver Driver;
    private NetworkConnection _connection;

    private bool _isActive = false;

    public Action ConnectionDropped;

    /// <summary>
    /// Initalising client - by creating driver
    /// and endpoint (ip,port of server)
    /// - try to connect to the the server
    /// </summary>
    /// <param name="port">port through which we will listen</param>

    public void Init(string ip, ushort port)
    {

        Driver = NetworkDriver.Create();
        //endpoint to connect to the server
        NetworkEndPoint endpoint = NetworkEndPoint.Parse(ip, port);

        //connect client driver with the server 
        _connection = Driver.Connect(endpoint);
        _isActive = true;

        Debug.Log("Client init() attempting to connect to - " + endpoint.Address + " Port :" + port);

        RegisterToEvent();
    }

    /// <summary>
    /// Shuting down client - by disposing driver 
    /// and connection(connection with server)
    /// </summary>
    public void ShutDown()
    {
        if (_isActive)
        {
            UnregisterToEvent();
            Driver.Dispose();
            _isActive = false;
            _connection = default(NetworkConnection);
            Debug.Log("Client shutdown() completed");
        }
    }

    private void OnDestroy()
    {
        ShutDown();
    }



    private void Update()
    {
        if (!_isActive) { return; }

        #region explanation
        //We call the driver's ScheduleUpdate method on every frame.
        //This is so we can update the state of each connection we have active
        //to make sure we read all data that we have received and
        //finally produce events that the user can react to
        //more details - https://docs.unity3d.com/Packages/com.unity.transport@0.2/manual/update-flow.html
        #endregion
        Driver.ScheduleUpdate().Complete();

        CheckAlive();
        UpdateMessagePump();
    }

    /// <summary>
    /// Check if our connection with server is active
    /// </summary>
    private void CheckAlive()
    {
        if (!_connection.IsCreated && _isActive)
        {
            Debug.Log("Something went wrong, lost connection to server");
            ConnectionDropped?.Invoke();
            ShutDown();
        }
    }



    /// <summary>
    /// Now we establised connection with server
    /// We can now start querying the server connection for events 
    /// that might have happened since the last update.
    /// </summary>
    private void UpdateMessagePump()
    {
        DataStreamReader stream;
        NetworkEvent.Type cmd;

        //not an empty event we recieved something
        //pop event that has happened for server connection - so we can read it
        while ((cmd = _connection.PopEvent(Driver, out stream)) != NetworkEvent.Type.Empty)
        {
            //we connected to server
            if (cmd == NetworkEvent.Type.Connect) {
                //We send welcome first from client - so assignedTeam value doesnt matter
                //then wait for server to assign team by sending us welcome message.
                SendToServer(new NetWelcome());            
                Debug.Log("Client is connected to SERVER!!!");
            }
            //we recieved some data
            else if (cmd == NetworkEvent.Type.Data)
            {
                Debug.Log("Client recieved some data from server");
                NetUtility.OnData(stream, default(NetworkConnection));
            }                       
            //we disconnected from server
            else if (cmd == NetworkEvent.Type.Disconnect)
            {
                Debug.Log("Client got disconnected from server");
                _connection = default(NetworkConnection);
                ConnectionDropped?.Invoke();
                ShutDown();
            }

        }

        //Debug.Log("Client UpdateMessagePumo() event type : " + cmd.ToString());
    }

    public void SendToServer(NetMessage msg)
    {
        DataStreamWriter writer;
        Driver.BeginSend(_connection, out writer);
        msg.Serialize(ref writer);
        //The writer needs to be Disposed
        //because it uses native memory which needs to be freed.
        Driver.EndSend(writer);
    }

    /// <summary>
    /// Event Parsing - listening to event messages 
    /// sent by server and reacting to it
    /// </summary>
    private void RegisterToEvent() {
        //KEEP_ALIVE message is generated from server evry 20 sec to check status of client
        NetUtility.C_KEEP_ALIVE += OnKeepAlive;
    }

    /// <summary>
    /// Unregistering from events from server - if disconnected
    /// </summary>
    private void UnregisterToEvent() {
        NetUtility.C_KEEP_ALIVE -= OnKeepAlive;
    }

    /// <summary>
    /// This is ping back and forth from server to client
    /// and from client to server - to make sure
    /// both server and client are alive/present/connected on network
    /// </summary>
    private void OnKeepAlive(NetMessage msg) {
        //Send it back to server to keep both side alive
        SendToServer(msg);
    }
}
