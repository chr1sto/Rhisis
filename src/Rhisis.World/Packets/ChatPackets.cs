using Ether.Network;
using Rhisis.Core.Network;
using Rhisis.Core.Network.Packets;
using Rhisis.World.Core.Components;
using Rhisis.World.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rhisis.World.Packets
{
    public static partial class WorldPacketFactory
    {
        public static void SendChat(NetConnection client, IEntity player, string message)
        {
            var objectComponent = player.GetComponent<ObjectComponent>();

            using (var packet = new FFPacket())
            {
                packet.StartNewMergedPacket(player.Id, SnapshotType.CHAT);
                packet.Write(message);

                client.Send(packet);
                SendToVisible(packet, player);
            }
        }
    }
}
