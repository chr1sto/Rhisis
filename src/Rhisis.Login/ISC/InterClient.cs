using Ether.Network;
using Ether.Network.Packets;
using Rhisis.Core.IO;
using Rhisis.Core.ISC;
using Rhisis.Core.ISC.Packets;
using Rhisis.Core.ISC.Structures;
using Rhisis.Login.ISC.Packets;
using System;

namespace Rhisis.Login.ISC
{
    public sealed class InterClient : NetConnection
    {
        private InterServer _server;

        public InterServerType Type { get; private set; }

        public BaseServerInfo ServerInfo { get; private set; }

        public void Initialize(InterServer server)
        {
            this._server = server;
            PacketFactory.SendWelcome(this);
        }

        public override void HandleMessage(NetPacketBase packet)
        {
            var packetHeaderNumber = packet.Read<uint>();

            try
            {
                var packetHeader = (InterPacketType)packetHeaderNumber;

                switch (packetHeader)
                {
                    case InterPacketType.AUTHENTICATE:
                        this.OnAuthenticate(packet);
                        break;
                    default: throw new NotImplementedException();
                }
            }
            catch
            {
                Logger.Warning("Unknown inter-server packet with header: 0x{0}", packetHeaderNumber.ToString("X2"));
            }
        }

        private void OnAuthenticate(NetPacketBase packet)
        {
            var id = packet.Read<int>();
            var host = packet.Read<string>();
            var name = packet.Read<string>();
            var type = packet.Read<byte>();
            var interServerType = (InterServerType)type;

            this.ServerInfo = new BaseServerInfo(id, host, name);

            if (interServerType == InterServerType.Cluster)
            {
                if (this._server.HasClusterWithId(id))
                {
                    // Has cluster: send error
                }
                

            }
            else if (interServerType == InterServerType.World)
            {
                var clusterId = packet.Read<int>();

                // TODO: check if world is registered in cluster id.
            }
            else
            {
                PacketFactory.SendAuthenticationResult(this, InterServerError.AUTH_FAILED);
                this._server.DisconnectClient(this.Id);
            }
        }
    }
}
