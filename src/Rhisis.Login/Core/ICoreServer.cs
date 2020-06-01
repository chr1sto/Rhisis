using Rhisis.Network.Core;
using Sylver.Network.Server;
using System.Collections.Generic;
using Rhisis.Core.Structures.Configuration;

namespace Rhisis.Login.Core
{
    public interface ICoreServer : INetServer
    {
        /// <summary>
        /// Contains the server core configuration
        /// </summary>
        CoreConfiguration CoreConfiguration { get; }
        
        /// <summary>
        /// Gets the cluster client by its cluster server id.
        /// </summary>
        /// <param name="clusterId">Cluster server id.</param>
        /// <returns></returns>
        CoreServerClient GetClusterServer(int clusterId);

        /// <summary>
        /// Checks if the current core server has a cluster server with the given id.
        /// </summary>
        /// <param name="clusterId">Cluster server id.</param>
        /// <returns>True if the cluster exists; false otherwise.</returns>
        bool HasCluster(int clusterId);

        /// <summary>
        /// Gets the world client by its parent cluster id and world id.
        /// </summary>
        /// <param name="parentClusterId">Parent cluster server id.</param>
        /// <param name="worldId">World server id.</param>
        /// <returns></returns>
        CoreServerClient GetWorldServer(int parentClusterId, int worldId);

        /// <summary>
        /// Checks if he current core server has a world serer with the given ids.
        /// </summary>
        /// <param name="parentClusterId">Parent cluster server id.</param>
        /// <param name="worldId">World server id.</param>
        /// <returns>True if the world server exists; false otherwise.</returns>
        bool HasWorld(int parentClusterId, int worldId);

        /// <summary>
        /// Gets a list of all connected clusters.
        /// </summary>
        /// <returns>Collection of <see cref="ClusterServerInfo"/></returns>
        IEnumerable<ClusterServerInfo> GetConnectedClusters();
    }
}
