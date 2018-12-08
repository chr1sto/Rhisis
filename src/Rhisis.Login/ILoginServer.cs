using Ether.Network.Server;
using Rhisis.Network.ISC.Structures;
using System.Collections.Generic;

namespace Rhisis.Login
{
    public interface ILoginServer : INetServer
    {
        /// <summary>
        /// Gets a list of all connected clusters.
        /// </summary>
        /// <returns>Collection of <see cref="ClusterServerInfo"/></returns>
        IEnumerable<ClusterServerInfo> GetConnectedClusters();
    }
}