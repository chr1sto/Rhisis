using Rhisis.World.Core;
using Rhisis.World.Core.Components;
using Rhisis.World.Core.Entities;
using Rhisis.World.Core.Systems;
using Rhisis.World.Packets;
using System;
using System.Linq.Expressions;

namespace Rhisis.World.Systems
{
    [System]
    public class VisibilitySystem : UpdateSystemBase
    {
        public static readonly float VisibilityRange = 75f;

        protected override Expression<Func<IEntity, bool>> Filter => x => x.HasComponent<PlayerComponent>() && x.HasComponent<ObjectComponent>();

        public VisibilitySystem(IContext context)
            : base(context)
        {
        }

        public override void Execute(IEntity entity)
        {
            var entityObjectComponent = entity.GetComponent<ObjectComponent>();
            var entityPlayerComponent = entity.GetComponent<PlayerComponent>();

            foreach (IEntity otherEntity in this.Entities)
            {
                var otherEntityObjectComponent = otherEntity.GetComponent<ObjectComponent>();

                if (entity.Id == otherEntity.Id || (otherEntityObjectComponent != null && !otherEntityObjectComponent.Spawned))
                    continue;

                if (entityObjectComponent.Position.IsInCircle(otherEntityObjectComponent.Position, VisibilityRange))
                {
                    if (!entityObjectComponent.Entities.Contains(otherEntity))
                        this.SpawnOtherEntity(entity, otherEntity);
                }
                else
                {
                    if (entityObjectComponent.Entities.Contains(otherEntity))
                        this.DespawnOtherEntity(entity, otherEntity);
                }
            }
        }
        
        private void SpawnOtherEntity(IEntity entity, IEntity otherEntity)
        {
            var entityObjectComponent = entity.GetComponent<ObjectComponent>();
            var entityPlayerComponent = entity.GetComponent<PlayerComponent>();
            var otherEntityObjectComponent = otherEntity.GetComponent<ObjectComponent>();

            // Spawn for the current player entity.
            WorldPacketFactory.SendSpawnObject(entityPlayerComponent.Connection, otherEntity);
            entityObjectComponent.Entities.Add(otherEntity);

            // Spawn for other entities
            if (otherEntity.EntityType != WorldEntityType.Player && !otherEntityObjectComponent.Entities.Contains(entity))
                otherEntityObjectComponent.Entities.Add(entity);

            // Send entity moves if it's moving while he spawns or enter the viewport
            var otherEntityMovableComponent = otherEntity.GetComponent<MovableComponent>();
            if (otherEntityMovableComponent != null && otherEntityMovableComponent.DestinationPosition != otherEntityObjectComponent.Position)
                WorldPacketFactory.SendDestinationPosition(entityPlayerComponent.Connection, otherEntity);
        }

        private void DespawnOtherEntity(IEntity entity, IEntity otherEntity)
        {
            var entityObjectComponent = entity.GetComponent<ObjectComponent>();
            var entityPlayerComponent = entity.GetComponent<PlayerComponent>();
            var otherEntityObjectComponent = otherEntity.GetComponent<ObjectComponent>();

            // Despawn for the current player entity.
            WorldPacketFactory.SendDespawnObject(entityPlayerComponent.Connection, otherEntity);
            entityObjectComponent.Entities.Remove(otherEntity);

            // Despawn for the other entities
            if (otherEntity.EntityType != WorldEntityType.Player && otherEntityObjectComponent.Entities.Contains(entity))
                otherEntityObjectComponent.Entities.Remove(entity);
        }
    }
}
