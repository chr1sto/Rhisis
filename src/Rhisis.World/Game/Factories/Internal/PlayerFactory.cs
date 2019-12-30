using Microsoft.Extensions.DependencyInjection;
using Rhisis.Core.Common;
using Rhisis.Core.Data;
using Rhisis.Core.DependencyInjection;
using Rhisis.Core.Extensions;
using Rhisis.Core.IO;
using Rhisis.Core.Resources;
using Rhisis.Core.Structures;
using Rhisis.Database;
using Rhisis.Database.Entities;
using Rhisis.World.Game.Behaviors;
using Rhisis.World.Game.Components;
using Rhisis.World.Game.Entities;
using Rhisis.World.Game.Maps;
using Rhisis.World.Systems.Recovery;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.World.Game.Factories.Internal
{
    [Injectable(ServiceLifetime.Singleton)]
    public sealed class PlayerFactory : IPlayerFactory
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IDatabase _database;
        private readonly IGameResources _gameResources;
        private readonly IMapManager _mapManager;
        private readonly IBehaviorManager _behaviorManager;
        private readonly ObjectFactory _playerFactory;

        /// <summary>
        /// Creates a new <see cref="PlayerFactory"/> instance.
        /// </summary>
        /// <param name="serviceProvider">Service provider.</param>
        /// <param name="database">Rhisis database access layer.</param>
        /// <param name="gameResources">Game resources.</param>
        /// <param name="mapManager">Map manager.</param>
        /// <param name="behaviorManager">Behavior manager.</param>
        public PlayerFactory(IServiceProvider serviceProvider, IDatabase database, IGameResources gameResources, IMapManager mapManager, IBehaviorManager behaviorManager)
        {
            this._serviceProvider = serviceProvider;
            this._database = database;
            this._gameResources = gameResources;
            this._mapManager = mapManager;
            this._behaviorManager = behaviorManager;
            this._playerFactory = ActivatorUtilities.CreateFactory(typeof(PlayerEntity), Type.EmptyTypes);
        }

        /// <inheritdoc />
        public IPlayerEntity CreatePlayer(DbCharacter character)
        {
            var player = this._playerFactory(this._serviceProvider, null) as IPlayerEntity;

            IMapInstance map = this._mapManager.GetMap(character.MapId);

            if (map == null)
            {
                throw new InvalidOperationException($"Cannot find map with id '{character.MapId}'.");
            }

            IMapLayer mapLayer = map.GetMapLayer(character.MapLayerId) ?? map.DefaultMapLayer;

            player.Object = new ObjectComponent
            {
                ModelId = character.Gender == 0 ? 11 : 12, // TODO: remove these magic numbers
                Type = WorldObjectType.Mover,
                MapId = character.MapId,
                CurrentMap = map,
                LayerId = mapLayer.Id,
                Position = new Vector3(character.PosX, character.PosY, character.PosZ),
                Angle = character.Angle,
                Size = ObjectComponent.DefaultObjectSize,
                Name = character.Name,
                Spawned = false,
                Level = character.Level,
                MovingFlags = ObjectState.OBJSTA_STAND
            };

            player.Health = new HealthComponent
            {
                Hp = character.Hp,
                Mp = character.Mp,
                Fp = character.Fp
            };

            player.VisualAppearance = new VisualAppearenceComponent
            {
                Gender = character.Gender,
                SkinSetId = character.SkinSetId,
                HairId = character.HairId,
                HairColor = character.HairColor,
                FaceId = character.FaceId,
            };

            player.PlayerData = new PlayerDataComponent
            {
                Id = character.Id,
                Gender = character.Gender.ToString().ToEnum<GenderType>(),
                Slot = character.Slot,
                Gold = character.Gold,
                Authority = (AuthorityType)character.User.Authority,
                Experience = character.Experience,
                JobData = this._gameResources.Jobs[character.ClassId]
            };

            player.Moves = new MovableComponent
            {
                Speed = this._gameResources.Movers[player.Object.ModelId].Speed,
                DestinationPosition = player.Object.Position.Clone(),
                LastMoveTime = Time.GetElapsedTime(),
                NextMoveTime = Time.GetElapsedTime() + 10
            };

            player.Attributes.ResetAttribute(DefineAttributes.STR, character.Strength);
            player.Attributes.ResetAttribute(DefineAttributes.STA, character.Stamina);
            player.Attributes.ResetAttribute(DefineAttributes.DEX, character.Dexterity);
            player.Attributes.ResetAttribute(DefineAttributes.INT, character.Intelligence);

            player.Statistics = new StatisticsComponent(character);
            player.Timers.NextHealTime = Time.TimeInSeconds() + RecoverySystem.NextIdleHealStand;

            player.Behavior = this._behaviorManager.GetDefaultBehavior(BehaviorType.Player, player);

            var gameServices = _serviceProvider.GetRequiredService<IEnumerable<IGameSystemLifeCycle>>().OrderBy(x => x.Order);

            foreach (IGameSystemLifeCycle service in gameServices)
            {
                service.Initialize(player);
            }

            mapLayer.AddEntity(player);

            return player;
        }

        /// <inheritdoc />
        public void SavePlayer(IPlayerEntity player)
        {
            if (player == null)
                return;

            DbCharacter character = this._database.Characters.Get(player.PlayerData.Id);

            if (character != null)
            {
                character.LastConnectionTime = player.PlayerData.LoggedInAt;
                character.PlayTime += (long)(DateTime.UtcNow - player.PlayerData.LoggedInAt).TotalSeconds;

                character.PosX = player.Object.Position.X;
                character.PosY = player.Object.Position.Y;
                character.PosZ = player.Object.Position.Z;
                character.Angle = player.Object.Angle;
                character.MapId = player.Object.MapId;
                character.MapLayerId = player.Object.LayerId;
                character.Gender = player.VisualAppearance.Gender;
                character.HairColor = player.VisualAppearance.HairColor;
                character.HairId = player.VisualAppearance.HairId;
                character.FaceId = player.VisualAppearance.FaceId;
                character.SkinSetId = player.VisualAppearance.SkinSetId;
                character.Level = player.Object.Level;

                character.Gold = player.PlayerData.Gold;
                character.Experience = player.PlayerData.Experience;

                character.Strength = player.Attributes[DefineAttributes.STR];
                character.Stamina = player.Attributes[DefineAttributes.STA];
                character.Dexterity = player.Attributes[DefineAttributes.DEX];
                character.Intelligence = player.Attributes[DefineAttributes.INT];
                character.StatPoints = player.Statistics.StatPoints;
                character.SkillPoints = player.Statistics.SkillPoints;

                character.Hp = player.Health.Hp;
                character.Mp = player.Health.Mp;
                character.Fp = player.Health.Fp;

                this._database.Complete();

                var gameSystems = _serviceProvider.GetRequiredService<IEnumerable<IGameSystemLifeCycle>>().OrderBy(x => x.Order);

                foreach (IGameSystemLifeCycle system in gameSystems)
                {
                    system.Save(player);
                }
            }
        }
    }
}
