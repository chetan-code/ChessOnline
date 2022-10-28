using System.Collections;
using System.Collections.Generic;
using Unity.Networking.Transport;
using UnityEngine;

/// <summary>
/// Message after we connect to server
/// to get intial info between server and client like
/// Team id etc - before tha game begins
/// </summary>
public class NetStartGame : NetMessage
{

    //Constructors
    //Constructor to use - When we sent a message
    public NetStartGame() {
        Code = OperationCode.START_GAME;
    }

    //When we read the message
    public NetStartGame(DataStreamReader reader) {
        Code = OperationCode.START_GAME;
        Deserialize(reader);
    }

    public override void Serialize(ref DataStreamWriter writer)
    {
        writer.WriteByte((byte)Code);
    }

    public override void Deserialize(DataStreamReader reader)
    {

    }

    public override void ReceiveOnClient()
    {
        NetUtility.C_START_GAME?.Invoke(this);
    }

    public override void ReceiveOnServer(NetworkConnection clientConnection)
    {
        NetUtility.S_START_GAME?.Invoke(this, clientConnection);
    }

}
