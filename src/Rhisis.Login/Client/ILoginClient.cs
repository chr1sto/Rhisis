using Ether.Network.Common;

namespace Rhisis.Login.Client
{
    public interface ILoginClient : INetUser
    {
        /// <summary>
        /// Gets the ID assigned to this session.
        /// </summary>
        uint SessionId { get; }

        /// <summary>
        /// Gets the client's logged username.
        /// </summary>
        string Username { get; }

        /// <summary>
        /// Gets the remote end point (IP and port) for this client.
        /// </summary>
        string RemoteEndPoint { get; }

        /// <summary>
        /// Check if the client is connected.
        /// </summary>
        bool IsConnected { get; }

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