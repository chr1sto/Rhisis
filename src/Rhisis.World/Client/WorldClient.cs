using Microsoft.Extensions.Logging;
using Rhisis.Core.Helpers;
using Rhisis.Network.Packets;
using Rhisis.World.Game.Entities;
using Sylver.HandlerInvoker;
using Sylver.Network.Data;
using Sylver.Network.Server;
using System;
using System.Net.Sockets;

namespace Rhisis.World.Client
{
    public sealed class WorldClient : NetServerClient, IWorldClient
    {
        private ILogger<WorldClient> _logger;
        private IHandlerInvoker _handlerInvoker;

        /// <inheritdoc />
        public uint SessionId { get; }

        /// <inheritdoc />
        public IPlayerEntity Player { get; set; }

        /// <summary>
        /// Creates a new <see cref="WorldClient"/> instance.
        /// </summary>
        /// <param name="socketConnection">Socket connectin.</param>
        public WorldClient(Socket socketConnection)
            : base(socketConnection)
        {
            this.SessionId = RandomHelper.GenerateSessionKey();
        }

        /// <summary>
        /// Initialize the client and send welcome packet.
        /// </summary>
        public void Initialize(ILogger<WorldClient> logger, IHandlerInvoker handlerInvoker)
        {
            this._logger = logger;
            this._handlerInvoker = handlerInvoker;
        }

        /// <inheritdoc />
        public override void HandleMessage(INetPacketStream packet)
        {
            uint packetHeaderNumber = 0;

            if (this.Socket == null)
            {
                this._logger.LogTrace($"Skip to handle world packet from {this.Socket.RemoteEndPoint}. Reason: {nameof(WorldClient)} is not connected.");
                return;
            }

            try
            {
                packet.Read<uint>(); // DPID: Always 0xFFFFFFFF (uint.MaxValue)
                packetHeaderNumber = packet.Read<uint>();
#if DEBUG
                this._logger.LogTrace("Received {0} packet from {1}.", (PacketType)packetHeaderNumber, this.Socket.RemoteEndPoint);
#endif
                this._handlerInvoker.Invoke((PacketType)packetHeaderNumber, this, packet);
            }
            catch (ArgumentNullException)
            {
                if (Enum.IsDefined(typeof(PacketType), packetHeaderNumber))
                    this._logger.LogWarning("Received an unimplemented World packet {0} (0x{1}) from {2}.", Enum.GetName(typeof(PacketType), packetHeaderNumber), packetHeaderNumber.ToString("X4"), this.Socket.RemoteEndPoint);
                else
                    this._logger.LogWarning("[SECURITY] Received an unknown World packet 0x{0} from {1}.", packetHeaderNumber.ToString("X4"), this.Socket.RemoteEndPoint);
            }
            catch (Exception exception)
            {
                this._logger.LogError(exception, $"An error occured while handling a world packet.");
                this._logger.LogDebug(exception.InnerException?.StackTrace);
            }
        }

        /// <inheritdoc />
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.Player != null)
                {
                    this.Player.Delete();
                    this.Player.Dispose();
                }
            }

            base.Dispose(disposing);
        }
    }
}
