using Ether.Network.Packets;
using Rhisis.Core.IO;
using Rhisis.Core.ISC.Packets;
using Rhisis.Core.Network;

namespace Rhisis.World.ISC
{
    public static class ISCHandler
    {
        [PacketHandler(InterPacketType.Welcome)]
        public static void OnWelcome(ISCClient client, INetPacketStream packet)
        {
            ISCPackets.SendAuthentication(client, client.WorldConfiguration);
        }

        [PacketHandler(InterPacketType.AuthenticationResult)]
        public static void OnAuthenticationResult(ISCClient client, INetPacketStream packet)
        {
            var authenticationResult = packet.Read<uint>();

            Logger.Debug("Authentication result: {0}", (InterServerError)authenticationResult);
        }
    }
}
