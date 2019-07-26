using Rhisis.Login.Client;
using Rhisis.Network.Core;
using Rhisis.Network.Packets;
using System.Collections.Generic;

namespace Rhisis.Login.Packets
{
    public interface ILoginPacketFactory
    {
        /// <summary>
        /// Send a welcome packet to the client.
        /// </summary>
        /// <param name="client">Client</param>
        /// <param name="sessionId">Session id</param>
        void SendWelcome(ILoginClient client, uint sessionId);

        /// <summary>
        /// Send a pong response to the client.
        /// </summary>
        /// <param name="client">Client</param>
        /// <param name="time">Current time</param>
        void SendPong(ILoginClient client, int time);

        /// <summary>
        /// Sends a login error.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="error"></param>
        void SendLoginError(ILoginClient client, ErrorType error);

        /// <summary>
        /// Sends the available server list.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="username"></param>
        /// <param name="clusters"></param>
        void SendServerList(ILoginClient client, string username, IEnumerable<ClusterServerInfo> clusters);
    }
}
