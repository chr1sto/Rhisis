using Ether.Network;
using Ether.Network.Packets;
using Rhisis.Core.IPC;
using Rhisis.Core.IPC.Packets;
using Rhisis.Core.Structures.Configuration;

namespace Rhisis.World.IPC
{
    public static class IPCPackets
    {
        public static void SendAuthentication(INetClient connection, WorldConfiguration worldConfiguration)
        {
            using (var packet = new NetPacket())
            {
                packet.Write((uint)InterPacketType.AUTHENTICATE);
                packet.Write(worldConfiguration.Id);
                packet.Write(worldConfiguration.Host);
                packet.Write(worldConfiguration.Name);
                packet.Write((byte)InterServerType.World);
                packet.Write(worldConfiguration.ClusterId);

                // TODO: add more information to packet if needed

                connection.Send(packet);
            }
        }
    }
}
