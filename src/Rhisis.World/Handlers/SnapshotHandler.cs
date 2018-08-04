using Ether.Network.Packets;
using NLog;
using Rhisis.Core.Exceptions;
using Rhisis.Core.Network;
using Rhisis.Core.Network.Packets;
using System;

namespace Rhisis.World.Handlers
{
    public static class SnapshotHandler
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        [PacketHandler(PacketType.SNAPSHOT)]
        public static void OnSnapshot(WorldClient client, INetPacketStream packet)
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
                    if (Enum.IsDefined(typeof(SnapshotType), snapshotHeaderNumber))
                        Logger.Warn("Unimplemented World packet {0} (0x{1})", Enum.GetName(typeof(PacketType), snapshotHeaderNumber), snapshotHeaderNumber.ToString("X4"));
                    else
                        Logger.Warn("Unknow World packet 0x{0}", snapshotHeaderNumber.ToString("X4"));
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
