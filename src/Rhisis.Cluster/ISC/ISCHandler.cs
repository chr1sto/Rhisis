using Ether.Network.Packets;
using Rhisis.Core.IO;
using Rhisis.Core.ISC.Packets;
using Rhisis.Core.Network;

namespace Rhisis.Cluster.ISC
{
    public static class ISCHandler
    {
        [PacketHandler(InterPacketType.Welcome)]
        public static void OnWelcome(ISCClient client, NetPacketBase packet)
        {
            ISCPackets.SendAuthentication(client, client.Configuration.Id, client.Configuration.Host, client.Configuration.Name);
        }

        [PacketHandler(InterPacketType.AuthenticationResult)]
        public static void OnAuthenticationResult(ISCClient client, NetPacketBase packet)
        {
            var authenticationResult = packet.Read<uint>();

            Logger.Debug("Authentication result: {0}", (InterServerError)authenticationResult);
        }
    }
}
