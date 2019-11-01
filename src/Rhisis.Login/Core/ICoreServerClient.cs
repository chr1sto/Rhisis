using Rhisis.Network.Core;
using Sylver.Network.Common;

namespace Rhisis.Login.Core
{
    /// <summary>
    /// Provides an interface for the core server clients.
    /// </summary>
    public interface ICoreServerClient : INetUser
    {
        /// <summary>
        /// Gets or sets the server informations.
        /// </summary>
        ServerInfo ServerInfo { get; }
    }
}
