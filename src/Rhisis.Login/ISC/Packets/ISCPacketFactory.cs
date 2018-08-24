using Ether.Network.Common;
using Ether.Network.Packets;
using Rhisis.Core.ISC.Packets;
using Rhisis.Core.ISC.Structures;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.Login.ISC.Packets
{
    public static class ISCPacketFactory
    {
        public static void SendWelcome(INetUser client)
        {
            using (var packet = new NetPacket())
            {
                packet.Write((uint)ISCPacketType.WELCOME);

                client.Send(packet);
            }
        }

        public static void SendAuthenticationResult(INetUser client, ISCPacketCode result)
        {
            using (var packet = new NetPacket())
            {
                packet.Write((uint)ISCPacketType.AUTHENT_RESULT);
                packet.Write((uint)result);

                client.Send(packet);
            }
        }

        public static void SendUpdateWorldList(INetUser client, IEnumerable<WorldServerInfo> worldServers)
        {
            using (var packet = new NetPacket())
            {
                packet.Write((uint)ISCPacketType.UPDATE_CLUSTER_WORLDS_LIST);
                packet.Write(worldServers.Count());

                foreach (var world in worldServers)
                {
                    packet.Write(world.Id);
                    packet.Write(world.Host);
                    packet.Write(world.Name);
                    packet.Write(world.ParentClusterId);
                }

                client.Send(packet);
            }
        }
    }
}
