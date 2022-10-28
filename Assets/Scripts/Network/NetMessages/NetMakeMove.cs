using System.Collections;
using UnityEngine;
using Unity.Networking.Transport;


public class NetMakeMove : NetMessage
{
    public int originalX;
    public int originalY;
    public int destinationX;
    public int destinationY;
    public int teamId;

    public NetMakeMove()
    {
        Code = OperationCode.MAKE_MOVE;
    }

    public NetMakeMove(DataStreamReader reader)
    {
        Code = OperationCode.MAKE_MOVE;
        Deserialize(reader);
    }


    public override void Serialize(ref DataStreamWriter writer)
    {
        writer.WriteByte((byte)Code);
        writer.WriteInt(originalX);
        writer.WriteInt(originalY);
        writer.WriteInt(destinationX);
        writer.WriteInt(destinationY);
        writer.WriteInt(teamId);
    }

    public override void Deserialize(DataStreamReader reader)
    {
        originalX = reader.ReadInt();
        originalY = reader.ReadInt();
        destinationX = reader.ReadInt();
        destinationY = reader.ReadInt();
        teamId = reader.ReadInt();
    }

    public override void ReceiveOnClient()
    {
        NetUtility.C_MAKE_MOVE?.Invoke(this);
    }

    public override void ReceiveOnServer(NetworkConnection clientConnection)
    {
        NetUtility.S_MAKE_MOVE?.Invoke(this, clientConnection);
    }

}
