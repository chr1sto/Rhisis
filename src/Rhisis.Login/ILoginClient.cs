using Ether.Network.Common;

namespace Rhisis.Login
{
    public interface ILoginClient : INetUser
    {
        /// <summary>
        /// Disconnects the current client.
        /// </summary>
        void Disconnect();

        /// <summary>
        /// Sets the client's username.
        /// </summary>
        /// <param name="username"></param>
        void SetClientUsername(string username);
    }
}