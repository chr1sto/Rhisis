using Ether.Network.Packets;
using Rhisis.Core.Data;
using System;

namespace Rhisis.Network.Packets.World
{
    /// <summary>
    /// Defines the <see cref="MeleeAttackPacket"/> structure.
    /// </summary>
    public struct MeleeAttackPacket : IEquatable<MeleeAttackPacket>
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
        /// Gets the unknown parameter.
        /// </summary>
        public int UnknownParameter { get; }
        
        /// <summary>
        /// Gets the attack flags.
        /// </summary>
        public int AttackFlags { get; }

        /// <summary>
        /// Gets the attack speed.
        /// </summary>
        public float WeaponAttackSpeed { get; }

        /// <summary>
        /// Creates a new <see cref="MeleeAttackPacket"/> instance.
        /// </summary>
        /// <param name="packet">Incoming packet</param>
        public MeleeAttackPacket(INetPacketStream packet)
        {
            this.AttackMessage = (ObjectMessageType)packet.Read<int>();
            this.ObjectId = packet.Read<uint>();
            this.UnknownParameter = packet.Read<int>(); // ??
            this.AttackFlags = packet.Read<int>() & 0xFFFF; // Attack flags ?!
            this.WeaponAttackSpeed = packet.Read<float>();
        }

        /// <summary>
        /// Compares two <see cref="MeleeAttackPacket"/> instances.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(MeleeAttackPacket other)
        {
            return this.AttackMessage == other.AttackMessage &&
                   this.ObjectId == other.ObjectId &&
                   this.UnknownParameter == other.UnknownParameter &&
                   this.AttackFlags == other.AttackFlags &&
                   this.WeaponAttackSpeed == other.WeaponAttackSpeed;
        }
    }
}
