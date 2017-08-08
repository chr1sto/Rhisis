using Ether.Network;
using Ether.Network.Packets;
using Rhisis.Core.IO;
using Rhisis.Core.Network;
using Rhisis.Core.Network.Packets;
using Rhisis.Core.Structures.Configuration;
using System;

namespace Rhisis.Login
{
    public sealed class LoginClient : NetConnection
    {
        private readonly int _sessionId;
        private LoginConfiguration _loginConfig;

        public LoginClient()
        {
            this._sessionId = 0;
        }

        public void InitializeClient(LoginConfiguration configuration)
        {
            this._loginConfig = configuration;
            this.SendWelcomePacket();
        }

        // DEBUG
        public void SendWelcomePacket()
        {
            using (var packet = new FFPacket())
            {
                packet.WriteHeader(PacketType.WELCOME);
                packet.Write(this._sessionId);

                this.Send(packet);
            }
        }

        public override void HandleMessage(NetPacketBase packet)
        {
            var pak = packet as FFPacket;
            var packetHeader = new PacketHeader(pak);

            if (!FFPacket.VerifyPacketHeader(packetHeader))
            {
                Logger.Warning("Invalid packet header: {0}", packetHeader.Header);
                return;
            }

            var packetHeaderNumber = packet.Read<uint>();

            if (!FFPacketHandler<LoginClient>.Invoke(this, pak, packetHeaderNumber))
                FFPacket.UnknowPacket<PacketType>(packetHeaderNumber, 2);
        }

        [FFIncomingPacket(PacketType.CERTIFY)]
        public void OnLogin(FFPacket packet)
        {
            var certify = new CertifyPacket(packet, this._loginConfig.PasswordEncryption, this._loginConfig.EncryptionKey);

            Logger.Debug("BuildDate: {0}", certify.BuildData);
            Logger.Debug("Username: {0}", certify.Username);
            Logger.Debug("Password: {0}", certify.Password);
        }
    }
}