using Ether.Network;
using System;
using Ether.Network.Packets;

namespace Rhisis.World
{
    public sealed class WorldClient : NetConnection
    {
        public WorldClient()
        {
        }

        public override void HandleMessage(NetPacketBase packet)
        {
            throw new NotImplementedException();
        }
    }
}
