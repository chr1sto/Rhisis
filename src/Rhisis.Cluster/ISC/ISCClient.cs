using Ether.Network;
using System.Collections.Generic;
using Ether.Network.Packets;
using Rhisis.Core.Structures.Configuration;
using Rhisis.Core.Network;
using Rhisis.Core.ISC.Packets;
using Rhisis.Core.IO;
using Rhisis.Core.Exceptions;
using Rhisis.Core.ISC.Structures;

namespace Rhisis.Cluster.ISC
{
    public sealed class ISCClient : NetClient
    {
        private readonly ClusterConfiguration _configuration;
        private readonly IList<WorldServerInfo> _worlds;

        /// <summary>
        /// Gets the cluster configuration.
        /// </summary>
        public ClusterConfiguration Configuration => this._configuration;

        /// <summary>
        /// Gets the world server's informations connected to this cluster.
        /// </summary>
        public IList<WorldServerInfo> Worlds => this._worlds;

        /// <summary>
        /// Creates a new <see cref="ISCClient"/> instance.
        /// </summary>
        /// <param name="configuration">Cluster Server configuration</param>
        public ISCClient(ClusterConfiguration configuration) 
            : base(configuration.ISC.Host, configuration.ISC.Port, 1024)
        {
            this._configuration = configuration;
            this._worlds = new List<WorldServerInfo>();
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

        protected override void OnConnected()
        {
            // Nothing to do.
        }

        protected override void OnDisconnected()
        {
            Logger.Info("Disconnected from InterServer.");
        }
    }
}
