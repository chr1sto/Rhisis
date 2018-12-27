using Ether.Network.Common;
using Ether.Network.Packets;
using Rhisis.Core.Exceptions;
using Rhisis.Core.Helpers;
using Rhisis.Network;
using Rhisis.Network.Packets;
using System;
using System.Collections.Generic;
using Rhisis.Core.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Rhisis.Login
{
    public sealed class LoginClient : NetUser
    {
        private readonly ILogger<LoginClient> _logger;
        private readonly ILoginServer _loginServer;

        /// <summary>
        /// Gets the ID assigned to this session.
        /// </summary>
        public uint SessionId { get; }
        
        /// <summary>
        /// Gets the client's logged username.
        /// </summary>
        public string Username { get; private set; }

        /// <summary>
        /// Gets the remote end point (IP and port) for this client.
        /// </summary>
        public string RemoteEndPoint => this.Socket.RemoteEndPoint.ToString();

        /// <summary>
        /// Creates a new <see cref="LoginClient"/> instance.
        /// </summary>
        public LoginClient()
        {
            this.SessionId = RandomHelper.GenerateSessionKey();
            this._logger = DependencyContainer.Instance.Resolve<ILogger<LoginClient>>();
            this._loginServer = DependencyContainer.Instance.Resolve<ILoginServer>();
        }

        /// <summary>
        /// Disconnects the current client.
        /// </summary>
        public void Disconnect()
        {
            this._loginServer.DisconnectClient(this.Id);
            this.Dispose();
        }

        /// <summary>
        /// Sets the client's username.
        /// </summary>
        /// <param name="username"></param>
        public void SetClientUsername(string username)
        {
            if (!string.IsNullOrEmpty(this.Username))
                throw new InvalidOperationException("Client username already set.");

            this.Username = username;
        }

        /// <inheritdoc />
        public override void Send(INetPacketStream packet)
        {
            this._logger.LogTrace("Send {0} packet to {1}.", (PacketType)BitConverter.ToUInt32(packet.Buffer, 5), this.RemoteEndPoint);
            base.Send(packet);
        }

        /// <inheritdoc />
        public override void HandleMessage(INetPacketStream packet)
        {
            uint packetHeaderNumber = 0;

            if (Socket == null)
            {
                this._logger.LogTrace("Skip to handle packet from {0}. Reason: client is no more connected.", this.RemoteEndPoint);
                return;
            }

            try
            {
                packetHeaderNumber = packet.Read<uint>();

                this._logger.LogTrace("Received {0} packet from {1}.", (PacketType)packetHeaderNumber, this.RemoteEndPoint);

                bool packetInvokSuccess = PacketHandler<LoginClient>.Invoke(this, packet, (PacketType)packetHeaderNumber);

                if (!packetInvokSuccess)
                {
                    if (Enum.IsDefined(typeof(PacketType), packetHeaderNumber))
                        this._logger.LogWarning("Received an unimplemented Login packet {0} (0x{1}) from {2}.", Enum.GetName(typeof(PacketType), packetHeaderNumber), packetHeaderNumber.ToString("X2"), this.RemoteEndPoint);
                    else
                        this._logger.LogWarning("Received an unknown Login packet 0x{0} from {1}.", packetHeaderNumber.ToString("X2"), this.RemoteEndPoint);
                }
            }
            catch (RhisisPacketException packetException)
            {
                this._logger.LogError("Packet handle error from {0}. {1}", this.RemoteEndPoint, packetException);
                this._logger.LogDebug(packetException.InnerException?.StackTrace);
            }
        }
    }
}