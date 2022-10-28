using System.Collections;
using System.Collections.Generic;
using Unity.Networking.Transport;
using UnityEngine;




/// <summary>
/// Base class for : Data Messages that are trasmitted
/// between the client and the server
/// </summary>
public class NetMessage 
{
   public OperationCode Code { set; get; }

    /// <summary>
    /// serialising data (that need to be sent) with DataStreamWriter
    /// </summary>
    /// <param name="writer"></param>
    public virtual void Serialize(ref DataStreamWriter writer) {
        
        writer.WriteByte((byte)Code);
    }

    /// <summary>
    /// Deserialising the serilized data received from network with DataStreamReader
    /// NOTE : we read operationcode in OnData() of NetUtility class.
    /// We read(deserialize) the next set of data here
    /// </summary>
    /// <param name="reader"></param>
    public virtual void Deserialize(DataStreamReader reader) { 
    
    }

    /// <summary>
    /// When Data is received on client from server
    /// </summary>
    public virtual void ReceiveOnClient() { 
    
    }

    /// <summary>
    ///  When data is recieved on server from client
    ///  We need to identify the client hence the [param]
    /// </summary>
    /// <param name="clientConnection">client that sent the data</param>
    public virtual void ReceiveOnServer(NetworkConnection clientConnection) { 
    
    }


}
