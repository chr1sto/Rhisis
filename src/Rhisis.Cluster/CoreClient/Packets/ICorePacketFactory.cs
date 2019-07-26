using Rhisis.Core.Structures.Configuration;

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
    }
}
