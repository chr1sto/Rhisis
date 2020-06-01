using Rhisis.Core.Data;
using Rhisis.Core.Structures;
using Rhisis.Network.Packets;
using Rhisis.Network.Packets.World;
using Rhisis.World.Client;
using Rhisis.World.Packets;
using Sylver.HandlerInvoker.Attributes;

namespace Rhisis.World.Handlers
{
    [Handler]
    public class MovementHandler
    {
        private readonly IMoverPacketFactory _moverPacketFactory;

        /// <summary>
        /// Creates a new <see cref="MovementHandler"/> instance.
        /// </summary>
        /// <param name="moverPacketFactory">Mover packet factory.</param>
        public MovementHandler(IMoverPacketFactory moverPacketFactory)
        {
            _moverPacketFactory = moverPacketFactory;
        }

        /// <summary>
        /// Handles the destination position snapshot.
        /// </summary>
        /// <param name="serverClient">Client.</param>
        /// <param name="packet">Incoming packet.</param>
        [HandlerAction(SnapshotType.DESTPOS)]
        public void OnSnapshotSetDestPosition(IWorldServerClient serverClient, SetDestPositionPacket packet)
        {
            serverClient.Player.Object.MovingFlags = ObjectState.OBJSTA_FMOVE;
            serverClient.Player.Moves.DestinationPosition = new Vector3(packet.X, packet.Y, packet.Z);
            serverClient.Player.Follow.Reset();

            _moverPacketFactory.SendDestinationPosition(serverClient.Player);
        }
    }
}
