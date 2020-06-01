using Microsoft.Extensions.Logging;
using Rhisis.Network.Packets;
using Rhisis.World.Client;
using Sylver.HandlerInvoker;
using Sylver.HandlerInvoker.Attributes;
using Sylver.Network.Data;
using System;

namespace Rhisis.World.Handlers
{
    [Handler]
    public class SnapshotHandler
    {
        private readonly ILogger<SnapshotHandler> _logger;
        private readonly IHandlerInvoker _handlerInvoker;

        /// <summary>
        /// Creates a new <see cref="SnapshotHandler"/> instance.
        /// </summary>
        /// <param name="logger">Logger.</param>
        /// <param name="handlerInvoker">Handler invoker.</param>
        public SnapshotHandler(ILogger<SnapshotHandler> logger, IHandlerInvoker handlerInvoker)
        {
            _logger = logger;
            _handlerInvoker = handlerInvoker;
        }

        /// <summary>
        /// Receives and handles a snapshot.
        /// </summary>
        /// <param name="serverClient">Client.</param>
        /// <param name="packet">Incoming packet with snapshots.</param>
        [HandlerAction(PacketType.SNAPSHOT)]
        public void OnSnapshot(IWorldServerClient serverClient, INetPacketStream packet)
        {
            var snapshotCount = packet.Read<byte>();

            while (snapshotCount > 0)
            {
                var snapshotHeaderNumber = packet.Read<short>();

                try
                {
                    var snapshotHeader = (SnapshotType)snapshotHeaderNumber;

                    _handlerInvoker.Invoke(snapshotHeader, serverClient, packet);

                }
                catch (ArgumentNullException)
                {
                    if (Enum.IsDefined(typeof(SnapshotType), snapshotHeaderNumber))
                        _logger.LogWarning("Received an unimplemented World snapshot {0} (0x{1}) from {2}.", Enum.GetName(typeof(SnapshotType), snapshotHeaderNumber), snapshotHeaderNumber.ToString("X4"), serverClient.Socket.RemoteEndPoint);
                    else
                        _logger.LogWarning("[SECURITY] Received an unknown World snapshot 0x{0} from {1}.", snapshotHeaderNumber.ToString("X4"), serverClient.Socket.RemoteEndPoint);
                }
                catch (Exception exception)
                {
                    _logger.LogError(exception, $"An error occured while handling a world snapshot.");
                    _logger.LogDebug(exception.InnerException?.StackTrace);
                }

                snapshotCount--;
            }
        }
    }
}
