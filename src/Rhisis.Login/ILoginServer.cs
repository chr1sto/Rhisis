using Ether.Network.Server;
using Rhisis.Network.ISC.Structures;
using System.Collections.Generic;

namespace Rhisis.Login
{
    /// <summary>
    /// Provides an interface for the Login Server instance.
    /// </summary>
    public interface ILoginServer : INetServer
    {
        /// <summary>
        /// Gets a list of all connected clusters.
        /// </summary>
        /// <returns>Collection of <see cref="ClusterServerInfo"/></returns>
        IEnumerable<ClusterServerInfo> GetConnectedClusters();

        /// <summary>
        /// Gets a connected client by his username.
        /// </summary>
        /// <param name="username">Client username</param>
        /// <returns></returns>
        LoginClient GetClientByUsername(string username);

        /// <summary>
        /// Verify if a client is connected to the login server.
        /// </summary>
        /// <param name="username">Client username</param>
        /// <returns></returns>
        bool IsClientConnected(string username);
    }
}