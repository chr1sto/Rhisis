using Rhisis.Login.Client;
using Sylver.Network.Server;

namespace Rhisis.Login
{
    /// <summary>
    /// Provides an interface for the Login Server instance.
    /// </summary>
    public interface ILoginServer : INetServer
    {
        /// <summary>
        /// Gets a connected client by his username.
        /// </summary>
        /// <param name="username">Client username</param>
        /// <returns></returns>
        ILoginClient GetClientByUsername(string username);

        /// <summary>
        /// Verify if a client is connected to the login server.
        /// </summary>
        /// <param name="username">Client username</param>
        /// <returns></returns>
        bool IsClientConnected(string username);
    }
}