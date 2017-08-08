namespace Rhisis.Core.Network.Packets
{
    public struct CertifyPacket
    {
        public string BuildData;
        public string Username;
        public string Password;
        
        public CertifyPacket(FFPacket packet, bool encryptPassword)
        {
            this.BuildData = packet.Read<string>();
            this.Username = packet.Read<string>();

            if (encryptPassword)
            {
                this.Password = "";
            }
            else
                this.Password = packet.Read<string>();
        }
    }
}
