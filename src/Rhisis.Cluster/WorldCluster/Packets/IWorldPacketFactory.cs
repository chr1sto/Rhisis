using Rhisis.Cluster.WorldCluster.Server;
using Rhisis.Network.Core;

namespace Rhisis.Cluster.WorldCluster.Packets
{
    public interface IWorldPacketFactory
    {
        /// <summary>
        /// Sends a welcome packet to a client.
        /// </summary>
        /// <param name="client">Client.</param>
        void SendHandshake(IWorldClusterServerClient client);

        /// <summary>
        /// Sends the authentication result to a client.
        /// </summary>
        /// <param name="client">Client.</param>
        /// <param name="authenticationResultType">Authentication result type.</param>
        void SendAuthenticationResult(IWorldClusterServerClient client, CoreAuthenticationResultType authenticationResultType);
    }
}
