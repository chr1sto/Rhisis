using Ether.Network;
using Ether.Network.Packets;
using Rhisis.Core.IPC.Packets;
using Rhisis.Core.IPC.Structures;

namespace Rhisis.Login.IPC.Packets
{
    public static partial class PacketFactory
    {
        public static void SendWelcome(NetConnection client)
        {
            using (var packet = new NetPacket())
            {
                packet.Write((uint)InterPacketType.Welcome);

                client.Send(packet);
            }
        }

        public static void SendAuthenticationResult(NetConnection client, InterServerError error)
        {
            using (var packet = new NetPacket())
            {
                packet.Write((uint)InterPacketType.AuthenticationResult);
                packet.Write((uint)error);

                client.Send(packet);
            }
        }

        public static void SendWorldsToCluster(NetConnection clusterClient, ClusterServerInfo clusterServerInfo)
        {
            using (var packet = new NetPacket())
            {
                packet.Write((uint)InterPacketType.UpdateClusterWorldsList);
                packet.Write(clusterServerInfo.Worlds.Count);

                foreach (WorldServerInfo world in clusterServerInfo.Worlds)
                {
                    packet.Write(world.Id);
                    packet.Write(world.Host);
                    packet.Write(world.Name);
                    // TODO: add more world informations if needed
                }

                clusterClient.Send(packet);
            }
        }
    }
}
