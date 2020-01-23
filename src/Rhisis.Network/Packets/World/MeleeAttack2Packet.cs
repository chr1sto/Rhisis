using System;
using Sylver.Network.Data;
using Rhisis.Core.Data;

namespace Rhisis.Network.Packets.World
{
    /// <summary>
    /// Defines the <see cref="MeleeAttack2Packet"/> structure.
    /// </summary>
    public struct MeleeAttack2Packet : IEquatable<MeleeAttack2Packet>
    {
        /// <summary>
        /// Gets the attack message.
        /// </summary>
        public ObjectMessageType AttackMessage { get; set; }

        /// <summary>
        /// Gets the target object id.
        /// </summary>
        public uint ObjectId { get; set; }

        /// <summary>
        /// Gets the second parameter.
        /// </summary>
        public int Parameter2 { get; set; }

        /// <summary>
        /// Gets the third parameter.
        /// </summary>
        public int Parameter3 { get; set; }

        /// <summary>
        /// Creates a new <see cref="MeleeAttack2Packet"/> object.
        /// </summary>
        /// <param name="packet">Incoming packet</param>
        public MeleeAttack2Packet(INetPacketStream packet)
        {
            AttackMessage = (ObjectMessageType) packet.Read<uint>();
            ObjectId = packet.Read<uint>();
            Parameter2 = packet.Read<int>();
            Parameter3 = packet.Read<int>();
        }

        /// <summary>
        /// Compares two <see cref="MeleeAttack2Packet"/>.
        /// </summary>
        /// <param name="other">Other <see cref="MeleeAttack2Packet"/></param>
        public bool Equals(MeleeAttack2Packet other)
        {
            return AttackMessage == other.AttackMessage 
                && ObjectId == other.ObjectId 
                && Parameter2 == other.Parameter2 
                && Parameter3 == other.Parameter3;
        }
    }
}