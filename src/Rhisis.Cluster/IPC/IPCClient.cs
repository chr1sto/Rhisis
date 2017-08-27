using Ether.Network;
using System;
using System.Collections.Generic;
using System.Text;
using Ether.Network.Packets;
using Rhisis.Core.Structures.Configuration;
using Rhisis.Core.Network;
using Rhisis.Core.IPC.Packets;
using Rhisis.Core.IO;
using Rhisis.Core.Exceptions;

namespace Rhisis.Cluster.IPC
{
    public sealed class IPCClient : NetClient
    {
        private ClusterConfiguration _configuration;

        public IPCClient(ClusterConfiguration configuration) 
            : base(configuration.IPC.Host, configuration.IPC.Port, 1024)
        {
            this._configuration = configuration;
        }

        protected override void HandleMessage(NetPacketBase packet)
        {
            var packetHeaderNumber = packet.Read<uint>();

            try
            {
                PacketHandler<IPCClient>.Invoke(this, packet, (InterPacketType)packetHeaderNumber);
            }
            catch (KeyNotFoundException)
            {
                Logger.Warning("Unknown inter-server packet with header: 0x{0}", packetHeaderNumber.ToString("X2"));
            }
            catch (RhisisPacketException packetException)
            {
                Logger.Error(packetException.Message);
#if DEBUG
                Logger.Debug("STACK TRACE");
                Logger.Debug(packetException.InnerException?.StackTrace);
#endif
            }
        }

        protected override void OnConnected()
        {
            Logger.Info("Connected to InterServer.");
        }

        protected override void OnDisconnected()
        {
            Logger.Info("Disconnected from InterServer.");
        }

        [PacketHandler(InterPacketType.WELCOME)]
        public void OnWelcome(NetPacketBase packet)
        {
            Logger.Debug("Welcome recieved. Send informations");
            IPCPackets.SendAuthentication(this, this._configuration.Id, this._configuration.Host, this._configuration.Name);
        }

        [PacketHandler(InterPacketType.AUTHENTICATION_RESULT)]
        public void OnAuthenticationResult(NetPacketBase packet)
        {
            var result = packet.Read<uint>();

            Logger.Debug("Authentication result: {0}", (InterServerError)result);
        }
    }
}
