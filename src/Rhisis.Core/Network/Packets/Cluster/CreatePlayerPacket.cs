using Ether.Network.Packets;
using System;

namespace Rhisis.Core.Network.Packets.Cluster
{
    public struct CreatePlayerPacket : IEquatable<CreatePlayerPacket>
    {
        public string Username { get; private set; }

        public string Password { get; private set; }

        public int Slot { get; private set; }

        public string Name { get; private set; }

        public int FaceId { get; private set; }

        public int CostumeId { get; private set; }

        public int SkinSet { get; private set; }

        public int HairMeshId { get; private set; }

        public uint HairColor { get; private set; }

        public byte Gender { get; private set; }

        public int Job { get; private set; }

        public int HeadMesh { get; private set; }

        public int BankPassword { get; private set; }

        public int AuthenticationKey { get; private set; }

        public CreatePlayerPacket(NetPacketBase packet)
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
