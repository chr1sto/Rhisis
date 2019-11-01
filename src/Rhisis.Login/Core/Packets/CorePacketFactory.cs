using Rhisis.Network.Core;
using Sylver.Network.Data;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.Login.Core.Packets
{
    public class CorePacketFactory : ICorePacketFactory
    {
        /// <inheritdoc />
        public void SendWelcome(ICoreServerClient client)
        {
            using (var packet = new NetPacket())
            {
                packet.Write((uint)CorePacketType.Welcome);
                client.Send(packet);
            }
        }

        /// <inheritdoc />
        public void SendAuthenticationResult(ICoreServerClient client, CoreAuthenticationResultType authenticationResultType)
        {
            using (var packet = new NetPacket())
            {
                packet.Write((uint)CorePacketType.AuthenticationResult);
                packet.Write((uint)authenticationResultType);
                client.Send(packet);
            }
        }

        /// <inheritdoc />
        public void SendUpdateWorldList(ICoreServerClient client, IEnumerable<WorldServerInfo> worldServers)
        {
            if (!(client.ServerInfo is ClusterServerInfo))
                return;

            using (var packet = new NetPacket())
            {
                packet.Write((uint)CorePacketType.UpdateClusterWorldsList);
                packet.Write(worldServers.Count());

                foreach (WorldServerInfo world in worldServers)
                {
                    packet.Write(world.Id);
                    packet.Write(world.Host);
                    packet.Write(world.Name);
                    packet.Write(world.Port);
                    packet.Write(world.ParentClusterId);
                }

                client.Send(packet);
            }
        }
    }
}
