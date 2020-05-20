using Microsoft.Extensions.DependencyInjection;
using Rhisis.Core.DependencyInjection;
using Rhisis.Network;
using Rhisis.Network.Packets;
using Rhisis.World.Game.Entities;

namespace Rhisis.World.Packets.Internal
{
    [Injectable(ServiceLifetime.Singleton)]
    internal class SystemMessagePacketFactory : ISystemMessagePacketFactory
    {
        private readonly IWorldServer _worldServer;

        /// <summary>
        /// Creates a new <see cref="SystemMessagePacketFactory"/> instance.
        /// </summary>
        /// <param name="worldServer">world server system.</param>
        public SystemMessagePacketFactory(IWorldServer worldServer)
        {
            _worldServer = worldServer;
        }

        /// <inheritdoc />
        public void SendSystemMessage(IPlayerEntity player, string message)
        {
            using var packet = new FFPacket();

            packet.WriteHeader(PacketType.SYSTEM);
            packet.Write(message);

            _worldServer.SendToAll(packet);
        }
    }
}
