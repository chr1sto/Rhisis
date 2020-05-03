using Microsoft.Extensions.Logging;
using Rhisis.Core.Helpers;
using Rhisis.Network.Packets;
using Rhisis.World.Game.Entities;
using Rhisis.World.Packets;
using Sylver.HandlerInvoker;
using Sylver.Network.Data;
using Sylver.Network.Server;
using System;
using System.Net.Sockets;

namespace Rhisis.World.Client
{
    public sealed class WorldServerClient : NetServerClient, IWorldServerClient
    {
        private ILogger<WorldServerClient> _logger;
        private IHandlerInvoker _handlerInvoker;

        /// <inheritdoc />
        public uint SessionId { get; }

        /// <inheritdoc />
        public IPlayerEntity Player { get; set; }

        /// <summary>
        /// Creates a new <see cref="WorldServerClient"/> instance.
        /// </summary>
        /// <param name="socketConnection">Socket connectin.</param>
        public WorldServerClient(Socket socketConnection)
            : base(socketConnection)
        {
            SessionId = RandomHelper.GenerateSessionKey();
        }

        /// <summary>
        /// Initialize the client and send welcome packet.
        /// </summary>
        public void Initialize(ILogger<WorldServerClient> logger, IHandlerInvoker handlerInvoker, IWorldServerPacketFactory worldServerPacketFactory)
        {
            _logger = logger;
            _handlerInvoker = handlerInvoker;

            worldServerPacketFactory.SendWelcome(this, SessionId);
        }

        /// <inheritdoc />
        public override void HandleMessage(INetPacketStream packet)
        {
            uint packetHeaderNumber = 0;

            if (Socket == null)
            {
                _logger.LogTrace($"Skip to handle world packet from null socket. Reason: {nameof(WorldServerClient)} is not connected.");
                return;
            }

            try
            {
                packet.Read<uint>(); // DPID: Always 0xFFFFFFFF (uint.MaxValue)
                packetHeaderNumber = packet.Read<uint>();
#if DEBUG
                _logger.LogTrace("Received {0} packet from {1}.", (PacketType)packetHeaderNumber, Socket.RemoteEndPoint);
#endif
                _handlerInvoker.Invoke((PacketType)packetHeaderNumber, this, packet);
            }
            catch (ArgumentNullException)
            {
                if (Enum.IsDefined(typeof(PacketType), packetHeaderNumber))
                    _logger.LogWarning("Received an unimplemented World packet {0} (0x{1}) from {2}.", Enum.GetName(typeof(PacketType), packetHeaderNumber), packetHeaderNumber.ToString("X4"), Socket.RemoteEndPoint);
                else
                    _logger.LogWarning("[SECURITY] Received an unknown World packet 0x{0} from {1}.", packetHeaderNumber.ToString("X4"), Socket.RemoteEndPoint);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"An error occured while handling a world packet.");
                _logger.LogDebug(exception.InnerException?.StackTrace);
            }
        }

        /// <inheritdoc />
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (Player != null)
                {
                    Player.Delete();
                    Player.Dispose();
                }
            }

            base.Dispose(disposing);
        }
    }
}
