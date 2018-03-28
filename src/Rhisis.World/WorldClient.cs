using Ether.Network.Common;
using Ether.Network.Packets;
using Rhisis.Core.Exceptions;
using Rhisis.Core.Helpers;
using Rhisis.Core.IO;
using Rhisis.Core.Network;
using Rhisis.Core.Network.Packets;
using Rhisis.Database;
using Rhisis.Database.Structures;
using Rhisis.World.Game;
using Rhisis.World.Game.Core;
using Rhisis.World.Game.Core.Interfaces;
using Rhisis.World.Game.Entities;
using Rhisis.World.Packets;
using Rhisis.World.Systems;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.World
{
    public sealed class WorldClient : NetUser
    {
        private readonly uint _sessionId;

        /// <summary>
        /// Gets or sets the player entity.
        /// </summary>
        public IPlayerEntity Player { get; set; }

        /// <summary>
        /// Gets the world server's instance.
        /// </summary>
        public IWorldServer WorldServer { get; private set; }

        /// <summary>
        /// Creates a new <see cref="WorldClient"/> instance.
        /// </summary>
        public WorldClient()
        {
            this._sessionId = RandomHelper.GenerateSessionKey();
        }

        /// <summary>
        /// Initialize the client and send welcome packet.
        /// </summary>
        public void InitializeClient(IWorldServer server)
        {
            this.WorldServer = server;
            CommonPacketFactory.SendWelcome(this, this._sessionId);
        }
        
        /// <inheritdoc />
        public override void HandleMessage(INetPacketStream packet)
        {
            var pak = packet as FFPacket;

            packet.Read<uint>(); // DPID: Always 0xFFFFFFFF
            var packetHeaderNumber = packet.Read<uint>();

            try
            {
                PacketHandler<WorldClient>.Invoke(this, pak, (PacketType)packetHeaderNumber);
            }
            catch (KeyNotFoundException)
            {
                FFPacket.UnknowPacket<PacketType>(packetHeaderNumber, 2);
            }
            catch (RhisisPacketException packetException)
            {
                Logger.Error(packetException.Message);

                if (packetException.InnerException != null)
                    Logger.Error("Inner Exception: {0}", packetException.InnerException.Message);

#if DEBUG
                Logger.Debug("STACK TRACE");
                Logger.Debug(packetException.InnerException?.StackTrace);
#endif
            }
        }

        /// <summary>
        /// Save the entity to the database.
        /// </summary>
        private void Save()
        {
            if (this.Player == null)
                return;

            this.Player.ObjectComponent.Spawned = false;

            using (DatabaseContext db = DatabaseService.GetContext())
            {
                Character character = db.Characters.Get(this.Player.PlayerComponent.Id);

                if (character != null)
                {
                    character.PosX = this.Player.ObjectComponent.Position.X;
                    character.PosY = this.Player.ObjectComponent.Position.Y;
                    character.PosZ = this.Player.ObjectComponent.Position.Z;
                    character.Angle = this.Player.ObjectComponent.Angle;
                    character.MapId = this.Player.ObjectComponent.MapId;
                    character.Gender = this.Player.HumanComponent.Gender;
                    character.HairColor = this.Player.HumanComponent.HairColor;
                    character.HairId = this.Player.HumanComponent.HairId;
                    character.FaceId = this.Player.HumanComponent.FaceId;
                    character.SkinSetId = this.Player.HumanComponent.SkinSetId;
                    character.Level = this.Player.ObjectComponent.Level;

                    character.Gold = this.Player.PlayerComponent.Gold;

                    character.Strength = this.Player.StatisticsComponent.Strenght;
                    character.Stamina = this.Player.StatisticsComponent.Stamina;
                    character.Dexterity = this.Player.StatisticsComponent.Dexterity;
                    character.Intelligence = this.Player.StatisticsComponent.Intelligence;
                    character.StatPoints = this.Player.StatisticsComponent.StatPoints;

                    // Save inventory

                    // Delete items
                    for (int i = character.Items.Count - 1; i > 0; i--)
                    {
                        Item dbItem = character.Items.ElementAt(i);
                        Game.Structures.Item inventoryItem = this.Player.Inventory.GetItemBySlot(dbItem.ItemSlot);

                        if (inventoryItem != null && inventoryItem.Id == -1)
                            character.Items.Remove(dbItem);
                    }

                    // Add or update items
                    foreach (Game.Structures.Item item in this.Player.Inventory.Items)
                    {
                        if (item.Id != -1)
                        {
                            Item dbItem = character.Items.FirstOrDefault(x => x.Id == item.DbId);

                            if (dbItem != null)
                            {
                                dbItem.CharacterId = this.Player.PlayerComponent.Id;
                                dbItem.ItemId = item.Id;
                                dbItem.ItemCount = item.Quantity;
                                dbItem.ItemSlot = item.Slot;
                                dbItem.Refine = item.Refine;
                                dbItem.Element = item.Element;
                                dbItem.ElementRefine = item.ElementRefine;
                                db.Items.Update(dbItem);
                            }
                            else
                            {
                                dbItem = new Item
                                {
                                    CharacterId = this.Player.PlayerComponent.Id,
                                    CreatorId = item.CreatorId,
                                    ItemId = item.Id,
                                    ItemCount = item.Quantity,
                                    ItemSlot = item.Slot,
                                    Refine = item.Refine,
                                    Element = item.Element,
                                    ElementRefine = item.ElementRefine
                                };

                                db.Items.Create(dbItem);
                            }
                        }
                    }
                }

                db.SaveChanges();
            }
        }

        /// <summary>
        /// Despawns the current player and notify other players arround.
        /// </summary>
        /// <param name="currentMap"></param>
        private void DespawnPlayer(Map currentMap)
        {
            IEnumerable<IEntity> entitiesAround = from x in currentMap.Context.Entities
                                                  where this.Player.ObjectComponent.Position.IsInCircle(x.ObjectComponent.Position, VisibilitySystem.VisibilityRange) && x != this.Player
                                                  select x;

            foreach (IEntity entity in entitiesAround)
            {
                if (entity.Type == WorldEntityType.Player)
                {
                    var otherPlayerEntity = entity as IPlayerEntity;

                    WorldPacketFactory.SendDespawnObjectTo(otherPlayerEntity, this.Player);
                }

                entity.ObjectComponent.Entities.Remove(this.Player);
            }

            currentMap.Context.DeleteEntity(this.Player);
        }

        /// <inheritdoc />
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.Player != null && World.WorldServer.Maps.TryGetValue(this.Player.ObjectComponent.MapId, out Map currentMap))
                {
                    this.DespawnPlayer(currentMap);
                }

                this.Save();
            }

            base.Dispose(disposing);
        }
    }
}
