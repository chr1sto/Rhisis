using Sylver.Network.Data;
using Microsoft.Extensions.Options;
using Rhisis.Core.Cryptography;
using Rhisis.Core.Structures.Configuration;

namespace Rhisis.Network.Packets.Login
{
    public class CertifyPacket : IPacketDeserializer
    {
        private readonly LoginConfiguration _configuration;

        public virtual string BuildVersion { get; private set; }

        public virtual string Username { get; private set; }

        public virtual string Password { get; private set; }

        public CertifyPacket(IOptions<LoginConfiguration> loginConfiguration)
        {
            _configuration = loginConfiguration.Value;
        }

        public void Deserialize(INetPacketStream packet)
        {
            BuildVersion = packet.Read<string>();
            Username = packet.Read<string>();
            Password = null;

            if (_configuration.PasswordEncryption)
            {
                byte[] encryptedPassword = packet.Read<byte>(16 * 42);
                byte[] encryptionKey = Aes.BuildEncryptionKeyFromString(_configuration.EncryptionKey, 16);

                Password = Aes.DecryptByteArray(encryptedPassword, encryptionKey);
            }
            else
            {
                Password = packet.Read<string>();
            }
        }
    }
}