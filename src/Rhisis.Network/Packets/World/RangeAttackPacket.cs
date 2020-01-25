using Sylver.Network.Data;
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
            AttackMessage = (ObjectMessageType)packet.Read<uint>();
            ObjectId = packet.Read<uint>();
            ItemId = packet.Read<uint>();
            IdSfxHit = packet.Read<int>();
        }

        /// <summary>
        /// Compares two <see cref="RangeAttackPacket"/> instances.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(RangeAttackPacket other)
        {
            return AttackMessage == other.AttackMessage &&
                   ObjectId == other.ObjectId &&
                   ItemId == other.ItemId &&
                   IdSfxHit == other.IdSfxHit;
        }
    }
}
