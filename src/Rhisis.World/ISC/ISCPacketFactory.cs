using Ether.Network.Client;
using Ether.Network.Packets;
using Rhisis.Network.ISC;
using Rhisis.Network.ISC.Packets;
using Rhisis.Core.Structures.Configuration;

namespace Rhisis.World.ISC
{
    public static class ISCPacketFactory
    {
        public static void SendAuthentication(INetClient client, WorldConfiguration worldConfiguration)
        {
            using (var packet = new NetPacket())
            {
                packet.Write((uint)ISCPacketType.AUTHENT);
                packet.Write(worldConfiguration.Id);
                packet.Write(worldConfiguration.Name);
                packet.Write(worldConfiguration.Host);
                packet.Write(worldConfiguration.Port);
                packet.Write((byte)ISCServerType.World);
                packet.Write(worldConfiguration.ClusterId);

                // TODO: add more information to packet if needed.
                client.Send(packet);
            }
        }
    }
}
