using Ether.Network.Packets;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rhisis.Core.Network.Packets.World
{
    /// <summary>
    /// Defines the <see cref="MoveItemPacket"/> structure.
    /// </summary>
    public struct MoveItemPacket : IEquatable<MoveItemPacket>
    {
        public byte ItemType { get; }

        public byte SourceSlot { get; }

        public byte DestinationSlot { get; }

        public MoveItemPacket(NetPacketBase packet)
        {
            this.ItemType = packet.Read<byte>();
            this.SourceSlot = packet.Read<byte>();
            this.DestinationSlot = packet.Read<byte>();
        }

        public bool Equals(MoveItemPacket other)
        {
            throw new NotImplementedException();
        }
    }
}
