using Ether.Network.Packets;
using System;

namespace Rhisis.Network.Packets.World
{
    public struct MeleeAttackPacket : IEquatable<MeleeAttackPacket>
    {
        public int AttackMessage { get; }

        public int ObjectId { get; }

        public float WeaponAttackSpeed { get; }

        public MeleeAttackPacket(INetPacketStream packet)
        {
            this.AttackMessage = packet.Read<int>();
            this.ObjectId = packet.Read<int>();
            packet.Read<int>(); // Always 0; don't need to store it
            packet.Read<int>(); // Possibly error number returned from client
            this.WeaponAttackSpeed = packet.Read<float>();
        }

        public bool Equals(MeleeAttackPacket other)
        {
            return this.AttackMessage == other.AttackMessage &&
                this.ObjectId == other.ObjectId &&
                this.WeaponAttackSpeed == other.WeaponAttackSpeed;
        }
    }
}
