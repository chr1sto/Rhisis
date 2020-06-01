using System.Collections.Generic;
using Rhisis.Core.Structures.Configuration;
using Rhisis.Network.Core;

namespace Rhisis.Cluster.CoreClient.Packets
{
    /// <summary>
    /// Provdes factory methods to send packets to the core server.
    /// </summary>
    public interface ICorePacketFactory
    {
        /// <summary>
        /// Sends the authentication request to the core server.
        /// </summary>
        /// <param name="client">Client.</param>
        /// <param name="clusterConfiguration">Cluster server configuration.</param>
        void SendAuthentication(IClusterCoreClient client, ClusterConfiguration clusterConfiguration);
        
        /// <summary>
        /// Sends the world server list to a core server through it's client.
        /// </summary>
        /// <param name="client">Cluster client.</param>
        /// <param name="worldServers">World server list.</param>
        void SendUpdateWorldList(IClusterCoreClient client, IEnumerable<WorldServerInfo> worldServers);
    }
}
