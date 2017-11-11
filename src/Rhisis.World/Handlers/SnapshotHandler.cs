using Ether.Network.Packets;
using Rhisis.Core.Exceptions;
using Rhisis.Core.IO;
using Rhisis.Core.Network;
using Rhisis.Core.Network.Packets;
using System;

namespace Rhisis.World.Handlers
{
    public static class SnapshotHandler
    {
        [PacketHandler(PacketType.SNAPSHOT)]
        public static void OnSnapshot(WorldClient client, NetPacketBase packet)
        {
            var snapshotCount = packet.Read<byte>();

            while (snapshotCount > 0)
            {
                var snapshotHeaderNumber = packet.Read<short>();

                try
                {
                    var snapshotHeader = (SnapshotType)snapshotHeaderNumber;

                    switch (snapshotHeader)
                    {
                        case SnapshotType.DESTPOS:
                            MovementHandler.OnSnapshotSetDestPosition(client, packet);
                            break;

                        default: throw new RhisisPacketException("Unknow snapshot handler");
                    }
                }
                catch (RhisisPacketException)
                {
                    FFPacket.UnknowPacket<SnapshotType>((uint)snapshotHeaderNumber, sizeof(short));
                }
                catch (Exception)
                {
                    Logger.Error("An error occured during the execution of snapshot: 0x{0}", snapshotHeaderNumber.ToString("X4"));
                }

                snapshotCount--;
            }
        }
    }
}
