using Rhisis.Core.ISC.Structures;
using Rhisis.Core.Network;
using Rhisis.Core.Network.Packets;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.Login.Packets
{
    public static class PacketFactory
    {
        public static void SendWelcome(LoginClient client, uint sessionId)
        {
            using (var packet = new FFPacket())
            {
                packet.WriteHeader(PacketType.WELCOME);
                packet.Write(sessionId);

                client.Send(packet);
            }
        }

        public static void SendLoginError(LoginClient client, ErrorType error)
        {
            using (var packet = new FFPacket())
            {
                packet.WriteHeader(PacketType.ERROR);
                packet.Write((int)error);

                client.Send(packet);
            }
        }

        public static void SendServerList(LoginClient client, string username, IEnumerable<ClusterServerInfo> clusters)
        {
            using (var packet = new FFPacket())
            {
                packet.WriteHeader(PacketType.SRVR_LIST);
                packet.Write(0);
                packet.Write<byte>(1);
                packet.Write(username);
                packet.Write(clusters.Sum(x => x.Worlds.Count) + clusters.Count());

                foreach (ClusterServerInfo cluster in clusters)
                {
                    packet.Write(-1);
                    packet.Write(cluster.Id);
                    packet.Write(cluster.Name);
                    packet.Write(cluster.Host);
                    packet.Write(0);
                    packet.Write(0);
                    packet.Write(1);
                    packet.Write(0);

                    foreach (WorldServerInfo world in cluster.Worlds)
                    {
                        packet.Write(cluster.Id);
                        packet.Write(world.Id);
                        packet.Write(world.Name);
                        packet.Write(world.Host);
                        packet.Write(0);
                        packet.Write(0);
                        packet.Write(1);
                        packet.Write(100); // Capacity
                    }
                }

                client.Send(packet);
            }
        }
    }
}
