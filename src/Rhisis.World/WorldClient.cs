using Ether.Network;
using Ether.Network.Packets;
using Rhisis.Core.Exceptions;
using Rhisis.Core.Helpers;
using Rhisis.Core.IO;
using Rhisis.Core.Network;
using Rhisis.Core.Network.Packets;
using Rhisis.Database;
using Rhisis.World.Game;
using Rhisis.World.Game.Core;
using Rhisis.World.Game.Entities;
using Rhisis.World.Packets;
using Rhisis.World.Systems;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.World
{
    public sealed class WorldClient : NetConnection
    {
        private readonly uint _sessionId;

        /// <summary>
        /// Gets or sets the player entity.
        /// </summary>
        public IPlayerEntity Player { get; set; }

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
        public void InitializeClient()
        {
            CommonPacketFactory.SendWelcome(this, this._sessionId);
        }

        /// <summary>
        /// Handles incoming messages.
        /// </summary>
        /// <param name="packet">Incoming packet</param>
        public override void HandleMessage(NetPacketBase packet)
        {
            var pak = packet as FFPacket;
            var packetHeader = new PacketHeader(pak);

            if (!FFPacket.VerifyPacketHeader(packetHeader, (int)this._sessionId))
            {
                Logger.Warning("Invalid header for packet: {0}", packetHeader.Header);
                return;
            }

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
            this.Player.ObjectComponent.Spawned = false;

            using (var db = DatabaseService.GetContext())
            {
                var character = db.Characters.Get(this.Player.PlayerComponent.Id);

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

                    db.SaveChanges();
                }
            }
        }

        /// <summary>
        /// Despawns the current player and notify other players arround.
        /// </summary>
        /// <param name="currentMap"></param>
        private void DespawnPlayer(Map currentMap)
        {
            var entitiesAround = from x in currentMap.Context.Entities
                                 where this.Player.ObjectComponent.Position.IsInCircle(x.ObjectComponent.Position, VisibilitySystem.VisibilityRange)
                                 select x;

            foreach (var entity in entitiesAround)
            {
                if (entity.Type == WorldEntityType.Player)
                {
                    var otherPlayerEntity = entity as IPlayerEntity;

                    WorldPacketFactory.SendDespawnObject(otherPlayerEntity.PlayerComponent.Connection, this.Player);
                }

                entity.ObjectComponent.Entities.Remove(this.Player);
            }

            currentMap.Context.DeleteEntity(this.Player);
        }

        /// <summary>
        /// Disposes the <see cref="WorldClient"/> resources.
        /// </summary>
        public override void Dispose()
        {
            if (this.Player != null && WorldServer.Maps.TryGetValue(this.Player.ObjectComponent.MapId, out Map currentMap))
            {
                this.DespawnPlayer(currentMap);
            }

            this.Save();
            base.Dispose();
        }
    }
}
