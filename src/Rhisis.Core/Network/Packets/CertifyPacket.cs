using Rhisis.Core.Cryptography;
using Rhisis.Core.Helpers;
using System;
using System.Linq;
using System.Text;

namespace Rhisis.Core.Network.Packets
{
    public struct CertifyPacket : IEquatable<CertifyPacket>
    {
        public string BuildData { get; private set; }

        public string Username { get; private set; }

        public string Password { get; private set; }

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

        public bool Equals(CertifyPacket other)
        {
            return this.BuildData == other.BuildData &&
                this.Username == other.Username &&
                this.Password == other.Password;
        }
    }
}
