using System;
using Unity.Networking.Transport;
using UnityEngine;

/// <summary>
/// Enum codes for the type of message we are sending
/// Only used to define different kind of messages
/// that we can send/recieve
/// </summary>
public enum OperationCode
{
    //we will write this codes are BYTES 
    //so we can have only 0-255 types
    KEEP_ALIVE = 1,
    WELCOME = 2,
    START_GAME = 3,
    MAKE_MOVE = 4,
    REMATCH = 5
}

public static class NetUtility
{

    //Net Messages

    //When we recieve message on client form server
    public static Action<NetMessage> C_KEEP_ALIVE;
    public static Action<NetMessage> C_WELCOME;
    public static Action<NetMessage> C_START_GAME;
    public static Action<NetMessage> C_MAKE_MOVE;
    public static Action<NetMessage> C_REMATCH;

    //When we recieve message on server from client
    public static Action<NetMessage, NetworkConnection> S_KEEP_ALIVE;
    public static Action<NetMessage, NetworkConnection> S_WELCOME;
    public static Action<NetMessage, NetworkConnection> S_START_GAME;
    public static Action<NetMessage, NetworkConnection> S_MAKE_MOVE;
    public static Action<NetMessage, NetworkConnection> S_REMATCH;


    /// <summary>
    /// To read data when recieved on server/client based on the Operation Code
    /// </summary>
    /// <param name="stream">stream of data recieved</param>
    /// <param name="connection">client that sent the data</param>
    /// <param name="server">[null by default] not null if we are reading data in server class</param>
    public static void OnData(DataStreamReader stream, NetworkConnection connection, Server server = null)
    {

        NetMessage msg = null;
        // a byte from the stream and advances the position within the stream by one byte (you can imagine a cursor moving ahead in a text file)
        var OpCode = (OperationCode)stream.ReadByte();

        switch (OpCode)
        {
            //Handling messages based on OperationCode on data recieved
            case OperationCode.KEEP_ALIVE:
                msg = new NetKeepAlive(stream);
                break;
            case OperationCode.WELCOME:
                msg = new NetWelcome(stream);
                break;
            case OperationCode.START_GAME:
                msg = new NetStartGame(stream);
                break;
            case OperationCode.MAKE_MOVE:
                msg = new NetMakeMove(stream);
                break;
            case OperationCode.REMATCH:
                //msg = new NetRematch(stream);
                break;
            default:
                Debug.Log("Message recieved had no OperationCode");
                break;
        }

        if (server != null)
        {
            msg.ReceiveOnServer(connection);
        }
        else 
        { 
            msg.ReceiveOnClient(); 
        }

    }
}
