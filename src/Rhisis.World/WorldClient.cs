using Ether.Network.Common;
using Ether.Network.Packets;
using NLog;
using Rhisis.Core.Exceptions;
using Rhisis.Core.Helpers;
using Rhisis.Core.Network;
using Rhisis.Core.Network.Packets;
using Rhisis.Database;
using Rhisis.Database.Entities;
using Rhisis.World.Game.Core;
using Rhisis.World.Game.Entities;
using Rhisis.World.Game.Maps;
using Rhisis.World.Packets;
using Rhisis.World.Systems;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.World
{
    public sealed class WorldClient : NetUser
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();
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
                if (Enum.IsDefined(typeof(PacketType), packetHeaderNumber))
                    Logger.Warn("Unimplemented World packet {0} (0x{1})", Enum.GetName(typeof(PacketType), packetHeaderNumber), packetHeaderNumber.ToString("X8"));
                else
                    Logger.Warn("Unknow World packet 0x{0}", packetHeaderNumber.ToString("X8"));
            }
            catch (RhisisPacketException packetException)
            {
                Logger.Error(packetException);
            }
        }

        /// <summary>
        /// Save the entity to the database.
        /// </summary>
        private void Save()
        {
            if (this.Player == null)
                return;

            this.Player.Object.Spawned = false;

            using (DatabaseContext db = DatabaseService.GetContext())
            {
                Character character = db.Characters.Get(this.Player.PlayerData.Id);

                if (character != null)
                {
                    character.PosX = this.Player.Object.Position.X;
                    character.PosY = this.Player.Object.Position.Y;
                    character.PosZ = this.Player.Object.Position.Z;
                    character.Angle = this.Player.Object.Angle;
                    character.MapId = this.Player.Object.MapId;
                    character.MapLayerId = this.Player.Object.LayerId;
                    character.Gender = this.Player.VisualAppearance.Gender;
                    character.HairColor = this.Player.VisualAppearance.HairColor;
                    character.HairId = this.Player.VisualAppearance.HairId;
                    character.FaceId = this.Player.VisualAppearance.FaceId;
                    character.SkinSetId = this.Player.VisualAppearance.SkinSetId;
                    character.Level = this.Player.Object.Level;

                    character.Gold = this.Player.PlayerData.Gold;

                    character.Strength = this.Player.Statistics.Strenght;
                    character.Stamina = this.Player.Statistics.Stamina;
                    character.Dexterity = this.Player.Statistics.Dexterity;
                    character.Intelligence = this.Player.Statistics.Intelligence;
                    character.StatPoints = this.Player.Statistics.StatPoints;

                    // Delete items
                    var itemsToDelete = new List<Item>(character.Items.Count);
                    itemsToDelete.AddRange(from dbItem
                        in character.Items
                        let inventoryItem = this.Player.Inventory.GetItem(x => x.DbId == dbItem.Id) ?? new Game.Structures.Item()
                        where inventoryItem.Id == -1
                        select dbItem);
                    itemsToDelete.ForEach(x => character.Items.Remove(x));

                    // Add or update items
                    foreach (var item in this.Player.Inventory.Items)
                    {
                        if (item.Id == -1)
                        {
                            continue;
                        }

                        Item dbItem = character.Items.FirstOrDefault(x => x.Id == item.DbId);

                        if (dbItem != null)
                        {
                            dbItem.CharacterId = this.Player.PlayerData.Id;
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
                                CharacterId = this.Player.PlayerData.Id,
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

                db.SaveChanges();
            }
        }

        /// <summary>
        /// Despawns the current player and notify other players arround.
        /// </summary>
        /// <param name="currentMap"></param>
        private void DespawnPlayer(IMapInstance currentMap)
        {
            IEnumerable<IEntity> entitiesAround = from x in currentMap.Entities
                                                  where this.Player.Object.Position.IsInCircle(x.Object.Position, VisibilitySystem.VisibilityRange) && x != this.Player
                                                  select x;

            foreach (IEntity entity in entitiesAround)
            {
                if (entity.Type == WorldEntityType.Player)
                {
                    var otherPlayerEntity = entity as IPlayerEntity;

                    WorldPacketFactory.SendDespawnObjectTo(otherPlayerEntity, this.Player);
                }

                entity.Object.Entities.Remove(this.Player);
            }

            currentMap.DeleteEntity(this.Player);
        }

        /// <inheritdoc />
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.Player != null && World.WorldServer.Maps.TryGetValue(this.Player.Object.MapId, out IMapInstance currentMap))
                {
                    this.DespawnPlayer(currentMap);
                }

                this.Save();
            }

            base.Dispose(disposing);
        }
    }
}
