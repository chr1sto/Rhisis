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
    }
}