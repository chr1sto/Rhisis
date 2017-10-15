using Ether.Network;
using Ether.Network.Packets;
using Rhisis.Core.IO;
using Rhisis.Core.ISC.Packets;
using Rhisis.Core.ISC.Structures;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.Login.ISC.Packets
{
    public static class PacketFactory
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

        public static void SendUpdateWorldList(NetConnection client, IEnumerable<WorldServerInfo> worlds)
        {
            Logger.Warning("Cluster connected: {0}", client.Socket.Connected);

            using (var packet = new NetPacket())
            {
                packet.Write((uint)InterPacketType.UpdateClusterWorldsList);
                packet.Write(worlds.Count());

                foreach (var world in worlds)
                {
                    packet.Write(world.Id);
                    packet.Write(world.Host);
                    packet.Write(world.Name);
                    packet.Write(world.ParentClusterId);
                }

                client.Send(packet);
                Logger.Warning("Update World List OK");
            }
        }
    }
}
