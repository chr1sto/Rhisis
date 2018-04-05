using Rhisis.World.Game.Core;
using Rhisis.World.Game.Core.Interfaces;
using Rhisis.World.Game.Entities;
using Rhisis.World.Packets;
using System;
using System.Linq.Expressions;
using Rhisis.World.Game.Maps;

namespace Rhisis.World.Systems
{
    [System]
    public class VisibilitySystem : SystemBase
    {
        public static readonly float VisibilityRange = 75f;

        /// <summary>
        /// Gets the <see cref="VisibilitySystem"/> match filter.
        /// </summary>
        protected override Expression<Func<IEntity, bool>> Filter => x => x.Type == WorldEntityType.Player || x.Type == WorldEntityType.Monster || x.Type == WorldEntityType.Npc;

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

            this.UpdateContextVisibility(entity, currentMapLayer);
            this.UpdateContextVisibility(entity, this.Context);
        }

        /// <summary>
        /// Update context visibility.
        /// </summary>
        /// <param name="entity">Current entity</param>
        /// <param name="context">Context containing entities</param>
        private void UpdateContextVisibility(IEntity entity, IContext context)
        {
            if (context == null)
                return;

            foreach (IEntity otherEntity in context.Entities)
            {
                if (entity.Id == otherEntity.Id || !otherEntity.Object.Spawned)
                    continue;

                if (otherEntity.Type == WorldEntityType.Player && entity.Object.LayerId != otherEntity.Object.LayerId)
                    continue;

                this.CanSee(entity, otherEntity);
            }
        }

        /// <summary>
        /// Check if the entity can see the other entity.
        /// </summary>
        /// <param name="entity">Current entity</param>
        /// <param name="otherEntity">Other entity</param>
        /// <returns>Can see or not the other entity</returns>
        private void CanSee(IEntity entity, IEntity otherEntity)
        {
            bool canSee = entity.Object.Position.IsInCircle(otherEntity.Object.Position, VisibilityRange) && entity != otherEntity;

            if (canSee)
            {
                if (!entity.Object.Entities.Contains(otherEntity))
                    this.SpawnOtherEntity(entity, otherEntity);
            }
            else
            {
                if (entity.Object.Entities.Contains(otherEntity))
                    this.DespawnOtherEntity(entity, otherEntity);
            }
        }

        /// <summary>
        /// Spawn the other entity for the current entity.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="otherEntity"></param>
        private void SpawnOtherEntity(IEntity entity, IEntity otherEntity)
        {
            var player = entity as IPlayerEntity;

            entity.Object.Entities.Add(otherEntity);
            WorldPacketFactory.SendSpawnObjectTo(player, otherEntity);

            if (otherEntity.Type != WorldEntityType.Player && !otherEntity.Object.Entities.Contains(entity))
                otherEntity.Object.Entities.Add(entity);

            if (otherEntity is IMovableEntity movableEntity &&
                movableEntity.MovableComponent.DestinationPosition != movableEntity.Object.Position)
                WorldPacketFactory.SendDestinationPosition(player.Connection, movableEntity);
        }

        /// <summary>
        /// Despawns the other entity for the current entity.
        /// </summary>
        /// <param name="entity">Current entity</param>
        /// <param name="otherEntity">other entity</param>
        private void DespawnOtherEntity(IEntity entity, IEntity otherEntity)
        {
            var player = entity as IPlayerEntity;

            WorldPacketFactory.SendDespawnObjectTo(player, otherEntity);
            entity.Object.Entities.Remove(otherEntity);
            
            if (otherEntity.Type != WorldEntityType.Player && otherEntity.Object.Entities.Contains(entity))
                otherEntity.Object.Entities.Remove(entity);
        }
    }
}
