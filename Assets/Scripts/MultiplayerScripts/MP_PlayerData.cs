using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public struct MP_PlayerData : IEquatable<MP_PlayerData>, INetworkSerializable
{


    public ulong clientId;
    public int characterId;
    public FixedString64Bytes playerUsername;
    public FixedString64Bytes playerId;

    public bool Equals(MP_PlayerData other)
    {
        return clientId == other.clientId
            && characterId == other.characterId
            && playerUsername == other.playerUsername
            && playerId == other.playerId;
    }

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref clientId);
        serializer.SerializeValue(ref characterId);
        serializer.SerializeValue(ref playerUsername);
        serializer.SerializeValue(ref playerId);
    }

}