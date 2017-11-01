using Ether.Network.Packets;
using Rhisis.Core.Common;
using Rhisis.Core.Network;
using Rhisis.Core.Network.Packets;
using Rhisis.Core.Network.Packets.World;
using Rhisis.Core.Structures;
using Rhisis.Database;
using Rhisis.Database.Structures;
using Rhisis.World.Core;
using Rhisis.World.Core.Components;
using Rhisis.World.Core.Entities;
using Rhisis.World.Packets;
using System;

namespace Rhisis.World.Handlers
{
    public static class JoinGameHandler
    {
        [PacketHandler(PacketType.JOIN)]
        public static void OnJoin(WorldClient client, NetPacketBase packet)
        {
            var joinPacket = new JoinPacket(packet);
            Character character = null;

            using (var db = DatabaseService.GetContext())
            {
                character = db.Characters.Get(joinPacket.PlayerId);
            }

            if (character == null)
            {
                // This is an hack attempt
                return;
            }

            if (character.User.Authority <= 0)
            {
                // Account banned so he can't connect to the game.
                return;
            }

            var map = WorldServer.Maps[character.MapId];

            // 1st: Create the player entity with the map context
            IEntity player = map.Context.CreateEntity();

            // 2nd: create the components
            var objectComponent = new ObjectComponent
            {
                ModelId = character.Gender == 0 ? 11 : 12,
                Type = WorldObjectType.Mover,
                MapId = character.MapId,
                Position = new Vector3(character.PosX, character.PosY, character.PosZ),
                Angle = character.Angle,
                Size = 100,
                Name = character.Name,
                Spawned = false
            };

            var humanComponent = new HumanComponent()
            {
                Gender = character.Gender,
                SkinSetId = character.SkinSetId,
                HairId = character.HairId,
                HairColor = character.HairColor,
                FaceId = character.FaceId,
            };

            var playerComponent = new PlayerComponent()
            {
                Id = character.Id,
                Slot = character.Slot,
                Connection = client
            };

            // 3rd: attach the component to the entity
            player.AddComponent(objectComponent);
            player.AddComponent(humanComponent);
            player.AddComponent(playerComponent);

            // 4rd: spawn the player
            WorldPacketFactory.SendPlayerSpawn(client, player);

            // 5th: player is now spawned
            objectComponent.Spawned = true;
        }
    }
}
