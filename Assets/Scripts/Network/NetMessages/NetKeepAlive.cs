using System.Collections;
using System.Collections.Generic;
using Unity.Networking.Transport;
using UnityEngine;

/// <summary>
/// This class is used as a message template
/// to send between client and server to check - 
/// if both are alive on network.(ping each other after every 20 sec)
/// Only operation code is sent and recieved - hence no more data to deserialize
/// </summary>

public class NetKeepAlive : NetMessage
{

    //Constructors

    /// <summary>
    /// Constructor : assigning operation code, 
    /// used when this message is sent by server/client
    /// </summary>
    public NetKeepAlive() {
        Code = OperationCode.KEEP_ALIVE;
    }

    /// <summary>
    /// Constructor : used when data is recieved in this mesage form from server/client 
    /// </summary>
    /// <param name="reader"></param>
    public NetKeepAlive(DataStreamReader reader) {
        Code = OperationCode.KEEP_ALIVE;
        Deserialize(reader);
    }

    /// <summary>
    /// Serialize - Write OperationCode as Byte in the message to be sent on network
    /// </summary>
    /// <param name="writer"></param>
    public override void Serialize(ref DataStreamWriter writer)
    {
        writer.WriteByte((byte)Code); 
    }

    /// <summary>
    /// In NetUtility class we started reading the messages
    /// so we already have OperationCode
    /// So we dont need to read any more data here - hence is it empty
    /// </summary>
    /// <param name="reader"></param>
    public override void Deserialize(DataStreamReader reader)
    {
        
    }

    public override void ReceiveOnClient()
    {
        NetUtility.C_KEEP_ALIVE?.Invoke(this);
    }

    public override void ReceiveOnServer(NetworkConnection clientConnection)
    {
        NetUtility.S_KEEP_ALIVE?.Invoke(this, clientConnection);
    }
    
}
