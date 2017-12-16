using Rhisis.World.Core.Systems;
using Rhisis.World.Game.Core;
using Rhisis.World.Game.Core.Interfaces;
using Rhisis.World.Game.Entities;
using Rhisis.World.Packets;
using System;
using System.Linq.Expressions;

namespace Rhisis.World.Systems
{
    [System]
    public class VisibilitySystem : SystemBase
    {
        public static readonly float VisibilityRange = 75f;

        /// <summary>
        /// Gets the <see cref="VisibilitySystem"/> match filter.
        /// </summary>
        protected override Expression<Func<IEntity, bool>> Filter => x => x.Type == WorldEntityType.Player || x.Type == WorldEntityType.Monster;

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
            foreach (var otherEntity in this.Context.Entities)
            {
                if (entity.Id != otherEntity.Id && otherEntity.ObjectComponent.Spawned)
                {
                    if (this.CanSee(entity, otherEntity))
                    {
                        if (!entity.ObjectComponent.Entities.Contains(otherEntity))
                            this.SpawnOtherEntity(entity, otherEntity);
                    }
                    else
                    {
                        if (entity.ObjectComponent.Entities.Contains(otherEntity))
                            this.DespawnOtherEntity(entity, otherEntity);
                    }
                }
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

            entity.ObjectComponent.Entities.Add(otherEntity);
            WorldPacketFactory.SendSpawnObject(player.PlayerComponent.Connection, otherEntity);

            if (otherEntity.Type != WorldEntityType.Player && !otherEntity.ObjectComponent.Entities.Contains(entity))
                otherEntity.ObjectComponent.Entities.Add(entity);

            if (otherEntity is IMovableEntity movableEntity &&
                movableEntity.MovableComponent.DestinationPosition != movableEntity.ObjectComponent.Position)
                WorldPacketFactory.SendDestinationPosition(player.PlayerComponent.Connection, movableEntity);
        }

        /// <summary>
        /// Despawns the other entity for the current entity.
        /// </summary>
        /// <param name="entity">Current entity</param>
        /// <param name="otherEntity">other entity</param>
        private void DespawnOtherEntity(IEntity entity, IEntity otherEntity)
        {
            var player = entity as IPlayerEntity;

            WorldPacketFactory.SendDespawnObject(player.PlayerComponent.Connection, otherEntity);
            entity.ObjectComponent.Entities.Remove(otherEntity);
            
            if (otherEntity.Type != WorldEntityType.Player && otherEntity.ObjectComponent.Entities.Contains(entity))
                otherEntity.ObjectComponent.Entities.Remove(entity);
        }

        /// <summary>
        /// Check if the entity can see the other entity.
        /// </summary>
        /// <param name="entity">Current entity</param>
        /// <param name="otherEntity">Other entity</param>
        /// <returns>Can see or not the other entity</returns>
        private bool CanSee(IEntity entity, IEntity otherEntity)
        {
            return entity.ObjectComponent.Position.IsInCircle(otherEntity.ObjectComponent.Position, VisibilityRange);
        }
    }
}
