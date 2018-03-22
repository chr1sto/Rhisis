using Ether.Network.Packets;
using Rhisis.Core.Common;
using Rhisis.Core.IO;
using Rhisis.Core.Network;
using Rhisis.Core.Network.Packets;
using Rhisis.Core.Network.Packets.World;
using Rhisis.Core.Structures;
using Rhisis.Database;
using Rhisis.Database.Structures;
using Rhisis.World.Game.Components;
using Rhisis.World.Game.Entities;
using Rhisis.World.Packets;
using Rhisis.World.Systems.Inventory;

namespace Rhisis.World.Handlers
{
    public static class JoinGameHandler
    {
        [PacketHandler(PacketType.JOIN)]
        public static void OnJoin(WorldClient client, INetPacketStream packet)
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
            client.Player = map.Context.CreateEntity<PlayerEntity>();

            // 2nd: create and initialize the components
            client.Player.ObjectComponent = new ObjectComponent
            {
                ModelId = character.Gender == 0 ? 11 : 12,
                Type = WorldObjectType.Mover,
                MapId = character.MapId,
                Position = new Vector3(character.PosX, character.PosY, character.PosZ),
                Angle = character.Angle,
                Size = 100,
                Name = character.Name,
                Spawned = false,
                Level = character.Level
            };

            client.Player.HumanComponent = new HumanComponent
            {
                Gender = character.Gender,
                SkinSetId = character.SkinSetId,
                HairId = character.HairId,
                HairColor = character.HairColor,
                FaceId = character.FaceId,
            };

            client.Player.PlayerComponent = new PlayerComponent
            {
                Id = character.Id,
                Slot = character.Slot
            };

            client.Player.MovableComponent = new MovableComponent
            {
                Speed = WorldServer.Movers[client.Player.ObjectComponent.ModelId].Speed,
                DestinationPosition = client.Player.ObjectComponent.Position.Clone(),
                LastMoveTime = Time.GetElapsedTime(),
                NextMoveTime = Time.GetElapsedTime() + 10
            };

            client.Player.StatisticsComponent = new StatisticsComponent(character);

            client.Player.Connection = client;

            // Initialize the inventory
            var inventoryEventArgs = new InventoryEventArgs(InventoryActionType.Initialize, character.Items);
            client.Player.Context.NotifySystem<InventorySystem>(client.Player, inventoryEventArgs);
            
            // 3rd: spawn the player
            WorldPacketFactory.SendPlayerSpawn(client.Player);

            // 4th: player is now spawned
            client.Player.ObjectComponent.Spawned = true;
        }
    }
}
