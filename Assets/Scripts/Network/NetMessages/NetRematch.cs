using System.Collections;
using System.Collections.Generic;
using Unity.Networking.Transport;
using UnityEngine;

public class NetRematch : NetMessage
{
    public int teamId;
    public byte wantRematch;

    public NetRematch()
    {
        Code = OperationCode.WELCOME;
    }

    //When we read the message
    public NetRematch(DataStreamReader reader)
    {
        Code = OperationCode.WELCOME;
        Deserialize(reader);
    }

    public override void Serialize(ref DataStreamWriter writer)
    {
        writer.WriteByte((byte)Code);
        writer.WriteInt(teamId);
        writer.WriteByte(wantRematch);
    }

    public override void Deserialize(DataStreamReader reader)
    {
        teamId = reader.ReadInt();
        wantRematch = reader.ReadByte();
    }

    public override void ReceiveOnClient()
    {
        NetUtility.C_REMATCH?.Invoke(this);
    }

    public override void ReceiveOnServer(NetworkConnection clientConnection)
    {
        NetUtility.S_REMATCH?.Invoke(this, clientConnection);
    }
}
