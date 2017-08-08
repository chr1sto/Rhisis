using Ether.Network;
using Ether.Network.Packets;
using Rhisis.Core.IO;
using Rhisis.Core.Network;
using Rhisis.Core.Network.Packets;
using System;

namespace Rhisis.Login
{
    public sealed class LoginClient : NetConnection
    {

        // DEBUG
        public void SendWelcomePacket()
        {
            using (var packet = new FFPacket())
            {
                packet.WriteHeader(PacketType.WELCOME);
                packet.Write(0);

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

            if (!FFPacketHandler<LoginClient>.Invoke(this, pak))
            {
                // Unknow or unimplemented packet
            }

        }

        [FFIncomingPacket(PacketType.CERTIFY)]
        public void OnLogin(FFPacket packet)
        {
            var certify = new CertifyPacket(packet);

            Logger.Debug("BuildDate: {0}", certify.BuildData);
            Logger.Debug("Username: {0}", certify.Username);
            Logger.Debug("Password: {0}", certify.Password);
        }
    }
}