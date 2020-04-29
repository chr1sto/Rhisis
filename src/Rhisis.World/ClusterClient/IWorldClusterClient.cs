using Rhisis.Core.Structures.Configuration;
using Rhisis.Core.Structures.Configuration.World;
using Sylver.Network.Client;

namespace Rhisis.World.ClusterClient
{
    public interface IWorldClusterClient : INetClient
    {
        /// <summary>
        /// Gets the world server configuration.
        /// </summary>
        WorldConfiguration WorldServerConfiguration { get; }

        /// <summary>
        /// Gets the core client configuration.
        /// </summary>
        CoreConfiguration CoreClientConfiguration { get; }

        /// <summary>
        /// Gets the remote end point (IP and port) for this client.
        /// </summary>
        string RemoteEndPoint { get; }
    }
}
