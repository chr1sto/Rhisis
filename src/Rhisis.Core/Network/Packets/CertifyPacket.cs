using Rhisis.Core.Cryptography;
using Rhisis.Core.Helpers;
using System.Linq;
using System.Text;

namespace Rhisis.Core.Network.Packets
{
    public struct CertifyPacket
    {
        public string BuildData;
        public string Username;
        public string Password;
        
        public CertifyPacket(FFPacket packet, bool encryptPassword, string encryptionKey)
        {
            this.BuildData = packet.Read<string>();
            this.Username = packet.Read<string>();

            if (encryptPassword)
            {
                byte[] passwordData = packet.ReadBytes(16 * 42);
                var key = Encoding.ASCII.GetBytes(encryptionKey).Concat(Enumerable.Repeat((byte)0, 5).ToArray()).ToArray();

                this.Password = Rijndael.DecryptData(passwordData, key).Trim('\0');
            }
            else
                this.Password = packet.Read<string>();
        }
    }
}
