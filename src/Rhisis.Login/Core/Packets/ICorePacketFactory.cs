using Rhisis.Network.Core;
using System.Collections.Generic;

namespace Rhisis.Login.Core.Packets
{
    public interface ICorePacketFactory
    {
        /// <summary>
        /// Sends a welcome packet to a client.
        /// </summary>
        /// <param name="client">Client.</param>
        void SendWelcome(ICoreServerClient client);

        /// <summary>
        /// Sends the authentication result to a client.
        /// </summary>
        /// <param name="client">Client.</param>
        /// <param name="authenticationResultType">Authentication result type.</param>
        void SendAuthenticationResult(ICoreServerClient client, CoreAuthenticationResultType authenticationResultType);

        /// <summary>
        /// Sends the world server list to a cluster client.
        /// </summary>
        /// <param name="client">Cluster client.</param>
        /// <param name="worldServers">World server list.</param>
        void SendUpdateWorldList(ICoreServerClient client, IEnumerable<WorldServerInfo> worldServers);
    }
}
