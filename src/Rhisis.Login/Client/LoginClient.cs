using Microsoft.Extensions.Logging;
using Rhisis.Core.Helpers;
using Rhisis.Login.Packets;
using Rhisis.Network.Packets;
using Sylver.HandlerInvoker;
using Sylver.Network.Data;
using Sylver.Network.Server;
using System;
using System.Net.Sockets;

namespace Rhisis.Login.Client
{
    public sealed class LoginClient : NetServerClient, ILoginClient
    {
        private ILogger<LoginClient> _logger;
        private ILoginServer _loginServer;
        private IHandlerInvoker _handlerInvoker;

        /// <inheritdoc />
        public uint SessionId { get; }

        /// <inheritdoc />
        public string Username { get; private set; }

        /// <inheritdoc />
        public bool IsConnected => !string.IsNullOrEmpty(Username);

        /// <summary>
        /// Creates a new <see cref="LoginClient"/> instance.
        /// </summary>
        public LoginClient(Socket socketConnection)
            : base(socketConnection)
        {
            SessionId = RandomHelper.GenerateSessionKey();
        }

        /// <summary>
        /// Initializes the current <see cref="LoginClient"/> instance.
        /// </summary>
        /// <param name="loginServer">Parent login server.</param>
        /// <param name="logger">Logger.</param>
        /// <param name="handlerInvoker">Handler invoker.</param>
        /// <param name="loginPacketFactory">Login packet factory.</param>
        public void Initialize(ILoginServer loginServer, ILogger<LoginClient> logger, IHandlerInvoker handlerInvoker, ILoginPacketFactory loginPacketFactory)
        {
            _loginServer = loginServer;
            _logger = logger;
            _handlerInvoker = handlerInvoker;

            loginPacketFactory.SendWelcome(this, SessionId);
        }

        /// <inheritdoc />
        public void Disconnect()
        {
            _loginServer.DisconnectClient(Id);
            Dispose();
        }

        /// <inheritdoc />
        public void SetClientUsername(string username)
        {
            if (!string.IsNullOrEmpty(Username))
                throw new InvalidOperationException("Client username already set.");

            Username = username;
        }

        /// <inheritdoc />
        public override void Send(INetPacketStream packet)
        {
            _logger.LogTrace("Send {0} packet to {1}.", (PacketType)BitConverter.ToUInt32(packet.Buffer, 5), Socket.RemoteEndPoint);
            base.Send(packet);
        }

        /// <inheritdoc />
        public override void HandleMessage(INetPacketStream packet)
        {
            uint packetHeaderNumber = 0;

            if (Socket == null)
            {
                _logger.LogTrace("Skip to handle login packet. Reason: client is not connected.");
                return;
            }

            try
            {
                packetHeaderNumber = packet.Read<uint>();

                _logger.LogTrace("Received {0} packet from {1}.", (PacketType)packetHeaderNumber, Socket.RemoteEndPoint);
                _handlerInvoker.Invoke((PacketType)packetHeaderNumber, this, packet);
            }
            catch (ArgumentException)
            {
                if (Enum.IsDefined(typeof(PacketType), packetHeaderNumber))
                {
                    _logger.LogWarning("Received an unimplemented Login packet {0} (0x{1}) from {2}.",
                        Enum.GetName(typeof(PacketType), packetHeaderNumber),
                        packetHeaderNumber.ToString("X2"),
                        Socket.RemoteEndPoint);
                }
                else
                {
                    _logger.LogWarning("Received an unknown Login packet 0x{0} from {1}.", 
                        packetHeaderNumber.ToString("X2"), 
                        Socket.RemoteEndPoint);
                }
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"An error occured while handling a login packet.");
                _logger.LogDebug(exception.InnerException?.StackTrace);
            }
        }
    }
}