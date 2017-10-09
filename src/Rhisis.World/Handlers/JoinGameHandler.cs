using Ether.Network.Packets;
using Rhisis.Core.Network;
using Rhisis.Core.Network.Packets;
using System;

namespace Rhisis.World.Handlers
{
    public static class JoinGameHandler
    {
        [PacketHandler(PacketType.JOIN)]
        public static void OnJoin(WorldClient client, NetPacketBase packet)
        {
            throw new NotImplementedException();
        }
    }
}
