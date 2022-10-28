using System.Collections;
using System.Collections.Generic;
using Unity.Networking.Transport;
using UnityEngine;

/// <summary>
/// Message after we connect to server
/// to get intial info between server and client like
/// Team id etc - before tha game begins
/// </summary>
public class NetWelcome : NetMessage
{
    public int AssignedTeam { set; get; }


    //Constructors
    //Constructor to use - When we sent a message
    public NetWelcome() {
        Code = OperationCode.WELCOME;
    }

    //When we read the message
    public NetWelcome(DataStreamReader reader) {
        Code = OperationCode.WELCOME;
        Deserialize(reader);
    }

    public override void Serialize(ref DataStreamWriter writer)
    {
        writer.WriteByte((byte)Code);
        writer.WriteInt(AssignedTeam);
    }

    public override void Deserialize(DataStreamReader reader)
    {
        //remember the Code is read as byte in NetUitility already
        AssignedTeam = reader.ReadInt(); 
    }

    public override void ReceiveOnClient()
    {
        NetUtility.C_WELCOME?.Invoke(this);
    }

    public override void ReceiveOnServer(NetworkConnection clientConnection)
    {
        NetUtility.S_WELCOME?.Invoke(this, clientConnection);
    }

}
