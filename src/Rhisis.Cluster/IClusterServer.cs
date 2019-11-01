using Rhisis.Core.Structures.Configuration;
using Rhisis.Network.Core;
using Sylver.Network.Server;
using System.Collections.Generic;

namespace Rhisis.Cluster
{
    /// <summary>
    /// Provides a mechanism to manage the cluster server instance.
    /// </summary>
    public interface IClusterServer : INetServer
    {
        /// <summary>
        /// Gets the cluster server's configuration.
        /// </summary>
        ClusterConfiguration ClusterConfiguration { get; }

        /// <summary>
        /// Gets the cluster server's connected world servers list.
        /// </summary>
        IList<WorldServerInfo> WorldServers { get; }

        /// <summary>
        /// Gets world server by his id.
        /// </summary>
        /// <param name="id">World Server id</param>
        /// <returns></returns>
        WorldServerInfo GetWorldServerById(int id);
    }
}