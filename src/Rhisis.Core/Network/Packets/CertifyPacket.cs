namespace Rhisis.Core.Network.Packets
{
    public struct CertifyPacket
    {
        public string BuildData;
        public string Username;
        public string Password;
        
        public CertifyPacket(FFPacket packet)
        {
            this.BuildData = packet.Read<string>();
            this.Username = packet.Read<string>();
            this.Password = packet.Read<string>();
        }
    }
}
