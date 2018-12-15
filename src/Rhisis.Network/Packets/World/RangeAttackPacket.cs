using Ether.Network.Packets;
using Rhisis.Core.Data;
using System;

namespace Rhisis.Network.Packets.World
{
    /// <summary>
    /// Defines the <see cref="RangeAttackPacket"/> structure.
    /// </summary>
    public struct RangeAttackPacket : IEquatable<RangeAttackPacket>
    {
        /// <summary>
        /// Gets the attack message.
        /// </summary>
        public ObjectMessageType AttackMessage { get; }

        /// <summary>
        /// Gets the object id.
        /// </summary>
        public uint ObjectId { get; }

        /// <summary>
        /// Gets the item id.
        /// </summary>
        public uint ItemId { get; }

        /// <summary>
        /// Gets the id of the hit SFX.
        /// </summary>
        public int IdSfxHit { get; }

        /// <summary>
        /// Creates a new <see cref="RangeAttackPacket"/> instance.
        /// </summary>
        /// <param name="packet">Incoming packet</param>
        public RangeAttackPacket(INetPacketStream packet)
        {
            this.AttackMessage = (ObjectMessageType)packet.Read<uint>();
            this.ObjectId = packet.Read<uint>();
            this.ItemId = packet.Read<uint>();
            this.IdSfxHit = packet.Read<int>();
        }

        /// <summary>
        /// Compares two <see cref="RangeAttackPacket"/> instances.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(RangeAttackPacket other)
        {
            return this.AttackMessage == other.AttackMessage &&
                   this.ObjectId == other.ObjectId &&
                   this.ItemId == other.ItemId &&
                   this.IdSfxHit == other.IdSfxHit;
        }
    }
}
