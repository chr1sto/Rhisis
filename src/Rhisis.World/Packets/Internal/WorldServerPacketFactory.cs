using Microsoft.Extensions.DependencyInjection;
using Rhisis.Core.DependencyInjection;
using Rhisis.Network;
using Rhisis.Network.Packets;
using Rhisis.World.Client;

namespace Rhisis.World.Packets.Internal
{
    [Injectable(ServiceLifetime.Singleton)]
    internal class WorldServerPacketFactory : IWorldServerPacketFactory
    {
        /// <inheritdoc />
        public void SendWelcome(IWorldServerClient serverClient, uint sessionId)
        {
            using var packet = new FFPacket(PacketType.WELCOME);

            packet.Write(sessionId);

            serverClient.Send(packet);
        }
    }
}