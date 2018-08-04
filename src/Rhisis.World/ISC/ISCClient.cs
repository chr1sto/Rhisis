using Ether.Network.Client;
using Ether.Network.Packets;
using NLog;
using Rhisis.Core.Exceptions;
using Rhisis.Core.ISC.Packets;
using Rhisis.Core.Network;
using Rhisis.Core.Structures.Configuration;
using System.Collections.Generic;
using System.Net.Sockets;

namespace Rhisis.World.ISC
{
    public sealed class ISCClient : NetClient
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Gets the world server's configuration.
        /// </summary>
        public WorldConfiguration WorldConfiguration { get; }

        /// <summary>
        /// Creates a new <see cref="ISCClient"/> instance.
        /// </summary>
        /// <param name="worldConfiguration"></param>
        public ISCClient(WorldConfiguration worldConfiguration) 
        {
            this.WorldConfiguration = worldConfiguration;
            this.Configuration.Host = this.WorldConfiguration.ISC.Host;
            this.Configuration.Port = this.WorldConfiguration.ISC.Port;
            this.Configuration.BufferSize = 1024;
        }

        /// <inheritdoc />
        public override void HandleMessage(INetPacketStream packet)
        {
            var packetHeaderNumber = packet.Read<uint>();

            try
            {
                PacketHandler<ISCClient>.Invoke(this, packet, (InterPacketType)packetHeaderNumber);
            }
            catch (KeyNotFoundException)
            {
                Logger.Warn("Unknown inter-server packet with header: 0x{0}", packetHeaderNumber.ToString("X2"));
            }
            catch (RhisisPacketException packetException)
            {
                Logger.Error(packetException.Message);
            }
        }

        /// <inheritdoc />
        protected override void OnConnected()
        {
            // Nothing to do.
        }

        /// <inheritdoc />
        protected override void OnDisconnected()
        {
            Logger.Info("Disconnected from Inter-Server.");
        }

        /// <inheritdoc />
        protected override void OnSocketError(SocketError socketError)
        {
            Logger.Error("ISC client error: {0}", socketError);
        }
    }
}
