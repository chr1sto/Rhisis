using Sylver.Network.Data;

namespace Rhisis.Network.Packets.Cluster
{
    public class CreatePlayerPacket : IPacketDeserializer
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

        public void Deserialize(INetPacketStream packet)
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
    }
}
