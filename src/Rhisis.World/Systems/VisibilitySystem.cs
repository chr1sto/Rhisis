using Rhisis.World.Game.Core;
using Rhisis.World.Game.Entities;
using Rhisis.World.Game.Maps;
using Rhisis.World.Packets;
using System;

namespace Rhisis.World.Systems
{
    [System]
    public class VisibilitySystem : SystemBase
    {
        public static readonly float VisibilityRange = (float)Math.Pow(75f, 2f);

        /// <inheritdoc />
        protected override WorldEntityType Type => WorldEntityType.Mover;

        /// <summary>
        /// Creates a new <see cref="VisibilitySystem"/> instance.
        /// </summary>
        /// <param name="context"></param>
        public VisibilitySystem(IContext context)
            : base(context)
        {
        }

        /// <summary>
        /// Executes the <see cref="VisibilitySystem"/> logic.
        /// </summary>
        /// <param name="entity">Current entity</param>
        public override void Execute(IEntity entity)
        {
            var currentMap = entity.Context as IMapInstance;
            IMapLayer currentMapLayer = currentMap?.GetMapLayer(entity.Object.LayerId);

            UpdateContextVisibility(entity, currentMapLayer);
            UpdateContextVisibility(entity, this.Context);
        }

        /// <summary>
        /// Update context visibility.
        /// </summary>
        /// <param name="entity">Current entity</param>
        /// <param name="context">Context containing entities</param>
        private static void UpdateContextVisibility(IEntity entity, IContext context)
        {
            if (context == null)
                return;

            foreach (IEntity otherEntity in context.Entities)
            {
                if (entity.Id == otherEntity.Id || !otherEntity.Object.Spawned)
                    continue;

                if (otherEntity.Type == WorldEntityType.Player && entity.Object.LayerId != otherEntity.Object.LayerId)
                    continue;

                bool canSee = entity.Object.Position.IsInCircle(otherEntity.Object.Position, VisibilityRange) && entity != otherEntity;

                if (canSee)
                {
                    if (!entity.Object.Entities.Contains(otherEntity))
                        SpawnOtherEntity(entity, otherEntity);
                }
                else
                {
                    if (entity.Object.Entities.Contains(otherEntity))
                        DespawnOtherEntity(entity, otherEntity);
                }
            }
        }

        /// <summary>
        /// Spawn the other entity for the current entity.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="otherEntity"></param>
        private static void SpawnOtherEntity(IEntity entity, IEntity otherEntity)
        {
            var player = entity as IPlayerEntity;

            entity.Object.Entities.Add(otherEntity);
            WorldPacketFactory.SendSpawnObjectTo(player, otherEntity);

            if (otherEntity.Type != WorldEntityType.Player && !otherEntity.Object.Entities.Contains(entity))
                otherEntity.Object.Entities.Add(entity);

            if (otherEntity is IMovableEntity movableEntity &&
                movableEntity.MovableComponent.DestinationPosition != movableEntity.Object.Position)
                WorldPacketFactory.SendDestinationPosition(movableEntity);
        }

        /// <summary>
        /// Despawns the other entity for the current entity.
        /// </summary>
        /// <param name="entity">Current entity</param>
        /// <param name="otherEntity">other entity</param>
        private static void DespawnOtherEntity(IEntity entity, IEntity otherEntity)
        {
            var player = entity as IPlayerEntity;

            WorldPacketFactory.SendDespawnObjectTo(player, otherEntity);
            entity.Object.Entities.Remove(otherEntity);
            
            if (otherEntity.Type != WorldEntityType.Player && otherEntity.Object.Entities.Contains(entity))
                otherEntity.Object.Entities.Remove(entity);
        }
    }
}
