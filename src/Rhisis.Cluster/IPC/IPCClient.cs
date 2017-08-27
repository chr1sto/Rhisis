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

        /// <summary>
        /// Creates a new <see cref="IPCClient"/> instance.
        /// </summary>
        /// <param name="configuration">Cluster Server configuration</param>
        public IPCClient(ClusterConfiguration configuration) 
            : base(configuration.IPC.Host, configuration.IPC.Port, 1024)
        {
            this._configuration = configuration;
        }

        /// <summary>
        /// Handles the incoming messages.
        /// </summary>
        /// <param name="packet"></param>
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
        }

        protected override void OnDisconnected()
        {
            Logger.Info("Disconnected from InterServer.");
        }

        [PacketHandler(InterPacketType.WELCOME)]
        public void OnWelcome(NetPacketBase packet)
        {
            IPCPackets.SendAuthentication(this, this._configuration.Id, this._configuration.Host, this._configuration.Name);
        }

        [PacketHandler(InterPacketType.AUTHENTICATION_RESULT)]
        public void OnAuthenticationResult(NetPacketBase packet)
        {
            var authenticationResult = packet.Read<uint>();

            Logger.Debug("Authentication result: {0}", (InterServerError)authenticationResult);
        }
    }
}
