using Rhisis.Core.Common;
using Rhisis.World.Game.Entities;
using Sylver.Network.Data;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.World.Packets.Internal
{
    internal class PacketFactoryBase
    {
        protected PacketFactoryBase() { }

        /// <summary>
        /// Sends the packet to all visible entities of the specified entity.
        /// </summary>
        /// <param name="packet">Packet to send.</param>
        /// <param name="entity">Entity.</param>
        /// <param name="sendToPlayer">Send packet to player if entity is a player.</param>
        public void SendToVisible(INetPacketStream packet, IWorldEntity entity, bool sendToPlayer = false)
        {
            IEnumerable<IPlayerEntity> visiblePlayers = from x in entity.Object.Entities
                                                        where x.Type == WorldEntityType.Player
                                                        select x as IPlayerEntity;

            foreach (IPlayerEntity visiblePlayer in visiblePlayers)
                visiblePlayer.Connection.Send(packet);

            if (sendToPlayer && entity is IPlayerEntity player)
                player.Connection.Send(packet);
        }

        /// <summary>
        /// Sends the packet to the player.
        /// </summary>
        /// <param name="player">Current player.</param>
        /// <param name="packet">Packet to send.</param>
        public void SendToPlayer(IPlayerEntity player, INetPacketStream packet) => player.Connection.Send(packet);
    }
}
