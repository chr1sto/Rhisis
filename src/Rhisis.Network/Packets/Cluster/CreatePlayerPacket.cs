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
            Username = packet.Read<string>();
            Password = packet.Read<string>();
            Slot = packet.Read<byte>();
            Name = packet.Read<string>();
            FaceId = packet.Read<byte>();
            CostumeId = packet.Read<byte>();
            SkinSet = packet.Read<byte>();
            HairMeshId = packet.Read<byte>();
            HairColor = packet.Read<uint>();
            Gender = packet.Read<byte>();
            Job = packet.Read<byte>();
            HeadMesh = packet.Read<byte>();
            BankPassword = packet.Read<int>();
            AuthenticationKey = packet.Read<int>();
        }
    }
}
