using Ether.Network;
using Rhisis.Core.Network;
using Rhisis.World.Core.Components;
using Rhisis.World.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rhisis.World.Packets
{
    public static partial class WorldPacketFactory
    {
        public static void SendPlayerSpawn(NetConnection client, IEntity player)
        {
            // Retrieve the component from the entity
            var objectComponent = player.GetComponent<ObjectComponent>();

            using (var packet = new FFPacket())
            {
                // TODO: build packet

                client.Send(packet);
            }
        }
    }
}
