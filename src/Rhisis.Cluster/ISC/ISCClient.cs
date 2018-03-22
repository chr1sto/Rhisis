using Ether.Network.Client;
using Ether.Network.Packets;
using Rhisis.Core.Exceptions;
using Rhisis.Core.IO;
using Rhisis.Core.ISC.Packets;
using Rhisis.Core.ISC.Structures;
using Rhisis.Core.Network;
using Rhisis.Core.Structures.Configuration;
using System.Collections.Generic;
using System.Net.Sockets;

namespace Rhisis.Cluster.ISC
{
    public sealed class ISCClient : NetClient
    {
        /// <summary>
        /// Gets the cluster configuration.
        /// </summary>
        public ClusterConfiguration ClusterConfiguration { get; }

        /// <summary>
        /// Gets the world server's informations connected to this cluster.
        /// </summary>
        public IList<WorldServerInfo> Worlds { get; }

        /// <summary>
        /// Creates a new <see cref="ISCClient"/> instance.
        /// </summary>
        /// <param name="configuration">Cluster Server configuration</param>
        public ISCClient(ClusterConfiguration configuration)
        {
            this.ClusterConfiguration = configuration;
            this.Worlds = new List<WorldServerInfo>();
            this.Configuration.Host = this.ClusterConfiguration.ISC.Host;
            this.Configuration.Port = this.ClusterConfiguration.ISC.Port;
            this.Configuration.BufferSize = 512;
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

        /// <inheritdoc />
        protected override void OnConnected()
        {
            // Nothing to do.
        }

        /// <inheritdoc />
        protected override void OnDisconnected()
        {
            Logger.Info("Disconnected from InterServer.");
        }

        /// <inheritdoc />
        protected override void OnSocketError(SocketError socketError)
        {
            Logger.Error("ISC client Socket Error: {0}", socketError);
        }
    }
}
