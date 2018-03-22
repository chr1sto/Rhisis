using Ether.Network.Client;
using Ether.Network.Packets;
using Rhisis.Core.ISC;
using Rhisis.Core.ISC.Packets;
using Rhisis.Core.Structures.Configuration;

namespace Rhisis.World.ISC
{
    public static class ISCPackets
    {
        public static void SendAuthentication(INetClient connection, WorldConfiguration worldConfiguration)
        {
            using (var packet = new NetPacket())
            {
                packet.Write((uint)InterPacketType.Authentication);
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
