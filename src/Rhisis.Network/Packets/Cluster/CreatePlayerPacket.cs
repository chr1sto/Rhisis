using Ether.Network.Packets;
using System;

namespace Rhisis.Network.Packets.Cluster
{
    public struct CreatePlayerPacket : IEquatable<CreatePlayerPacket>
    {
        public string Username { get; }

        public string Password { get; }

        public int Slot { get; }

        public string Name { get; }

        public int FaceId { get; }

        public int CostumeId { get; }

        public int SkinSet { get; }

        public int HairMeshId { get; }

        public uint HairColor { get; }

        public byte Gender { get; }

        public int Job { get; }

        public int HeadMesh { get; }

        public int BankPassword { get; }

        public int AuthenticationKey { get; }

        public CreatePlayerPacket(INetPacketStream packet)
        {
            this.Username = packet.Read<string>();
            this.Password = packet.Read<string>();
            this.Slot = packet.Read<byte>();
            this.Name = packet.Read<string>();
            this.FaceId = packet.Read<byte>();
            this.CostumeId = packet.Read<byte>();
            this.SkinSet = packet.Read<byte>();
            this.HairMeshId = packet.Read<byte>();
            this.HairColor = packet.Read<uint>();
            this.Gender = packet.Read<byte>();
            this.Job = packet.Read<byte>();
            this.HeadMesh = packet.Read<byte>();
            this.BankPassword = packet.Read<int>();
            this.AuthenticationKey = packet.Read<int>();
        }

        public bool Equals(CreatePlayerPacket other)
        {
            return this.Username == other.Username &&
                this.Password == other.Password &&
                this.Slot == other.Slot &&
                this.Name == other.Name &&
                this.FaceId == other.FaceId &&
                this.CostumeId == other.CostumeId &&
                this.SkinSet == other.SkinSet &&
                this.HairMeshId == other.HairMeshId &&
                this.HairColor == other.HairColor &&
                this.Gender == other.Gender &&
                this.HeadMesh == other.HeadMesh &&
                this.BankPassword == other.BankPassword &&
                this.AuthenticationKey == other.AuthenticationKey;
        }
    }
}
