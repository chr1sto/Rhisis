using Rhisis.Cluster.Client;
using Rhisis.Database.Entities;
using Rhisis.Network.Packets;
using System.Collections.Generic;

namespace Rhisis.Cluster.Packets
{
    /// <summary>
    /// Provides a factory to communicate with a cluster client.
    /// </summary>
    public interface IClusterPacketFactory
    {
        /// <summary>
        /// Sends a welcome packet to the client.
        /// </summary>
        /// <param name="client">Client.</param>
        void SendWelcome(IClusterClient client);
        
        /// <summary>
        /// Sends a pong packet in response of a ping packet.
        /// </summary>
        /// <param name="client">Client.</param>
        /// <param name="time">Ping time.</param>
        void SendPong(IClusterClient client, int time);

        /// <summary>
        /// Sends a cluster server error to the client.
        /// </summary>
        /// <param name="client">Client.</param>
        /// <param name="errorType">Cluster server error type.</param>
        void SendClusterError(IClusterClient client, ErrorType errorType);

        /// <summary>
        /// Sends the client's characters list.
        /// </summary>
        /// <param name="client">Client.</param>
        /// <param name="authenticationKey">Client authentication key.</param>
        /// <param name="characters">A list of the client's available characters.</param>
        void SendPlayerList(IClusterClient client, int authenticationKey, IEnumerable<DbCharacter> characters);

        /// <summary>
        /// Sends the selected world server host address.
        /// </summary>
        /// <param name="client">Client.</param>
        /// <param name="address">Selected world server host address.</param>
        void SendWorldAddress(IClusterClient client, string address);

        /// <summary>
        /// Sends the login num pad to the client.
        /// </summary>
        /// <param name="client">Client.</param>
        /// <param name="loginProtectValue">Login protect value.</param>
        void SendLoginNumPad(IClusterClient client, int loginProtectValue);

        /// <summary>
        /// Sends the login protect value.
        /// </summary>
        /// <param name="client">Client.</param>
        /// <param name="loginProtectValue">Login protect value.</param>
        void SendLoginProtect(IClusterClient client, int loginProtectValue);

        /// <summary>
        /// Sends the join world to the client.
        /// </summary>
        /// <param name="client">Client.</param>
        void SendJoinWorld(IClusterClient client);

        /// <summary>
        /// Sends a query to retrieve the client's tick count.
        /// </summary>
        /// <param name="client">Client.</param>
        /// <param name="time">Time.</param>
        void SendQueryTickCount(IClusterClient client, uint time);
    }
}
