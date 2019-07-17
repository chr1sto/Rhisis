using Ether.Network.Packets;
using Microsoft.Extensions.Options;
using Rhisis.Core.Cryptography;
using Rhisis.Core.Structures.Configuration;
using System;

namespace Rhisis.Network.Packets.Login
{
    public class CertifyPacket : IEquatable<CertifyPacket>, IPacketDeserializer
    {
        private readonly LoginConfiguration _configuration;

        public string BuildVersion { get; private set; }

        public string Username { get; private set; }

        public string Password { get; private set; }

        public byte[] EncryptedPassword { get; private set; }

        public CertifyPacket(IOptions<LoginConfiguration> loginConfiguration)
        {
            this._configuration = loginConfiguration.Value;
        }

        public bool Equals(CertifyPacket other)
        {
            return this.BuildVersion == other.BuildVersion &&
                this.Username == other.Username &&
                this.Password == other.Password;
        }

        public void Deserialize(INetPacketStream packet)
        {
            this.BuildVersion = packet.Read<string>();
            this.Username = packet.Read<string>();
            this.Password = null;
            this.EncryptedPassword = null;

            if (this._configuration.PasswordEncryption)
            {
                this.EncryptedPassword = packet.ReadArray<byte>(16 * 42);
                byte[] encryptionKey = Aes.BuildEncryptionKeyFromString(this._configuration.EncryptionKey, 16);

                this.Password = Aes.DecryptByteArray(this.EncryptedPassword, encryptionKey);
            }
            else
            {
                this.Password = packet.Read<string>();
            }
        }
    }
}