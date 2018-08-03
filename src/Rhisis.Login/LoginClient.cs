using Ether.Network.Common;
using Ether.Network.Packets;
using NLog;
using Rhisis.Core.Exceptions;
using Rhisis.Core.Helpers;
using Rhisis.Core.ISC.Structures;
using Rhisis.Core.Network;
using Rhisis.Core.Network.Packets;
using Rhisis.Core.Structures.Configuration;
using System.Collections.Generic;

namespace Rhisis.Login
{
    public sealed class LoginClient : NetUser
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        private readonly uint _sessionId;
        private LoginServer _loginServer;
        
        /// <summary>
        /// Gets the list of connected clusters.
        /// </summary>
        public IEnumerable<ClusterServerInfo> ClustersConnected => this._loginServer.ClustersConnected;

        /// <summary>
        /// Gets the login server's configuration.
        /// </summary>
        public LoginConfiguration Configuration => this._loginServer.LoginConfiguration;

        /// <summary>
        /// Creates a new <see cref="LoginClient"/> instance.
        /// </summary>
        public LoginClient()
        {
            this._sessionId = RandomHelper.GenerateSessionKey();
        }

        /// <summary>
        /// Initialize the client.
        /// </summary>
        /// <param name="loginServer"></param>
        public void InitializeClient(LoginServer loginServer)
        {
            this._loginServer = loginServer;
            CommonPacketFactory.SendWelcome(this, this._sessionId);
        }

        /// <summary>
        /// Disconnects the current client.
        /// </summary>
        public void Disconnect()
        {
            this.Dispose();
            this._loginServer.DisconnectClient(this.Id);
        }

        /// <inheritdoc />
        public override void HandleMessage(INetPacketStream packet)
        {
            var pak = packet as FFPacket;
            var packetHeaderNumber = packet.Read<uint>();

            try
            {
                PacketHandler<LoginClient>.Invoke(this, pak, (PacketType)packetHeaderNumber);
            }
            catch (KeyNotFoundException)
            {
                FFPacket.UnknowPacket<PacketType>(packetHeaderNumber, 2);
            }
            catch (RhisisPacketException packetException)
            {
                Logger.Error(packetException);
            }
        }
    }
}