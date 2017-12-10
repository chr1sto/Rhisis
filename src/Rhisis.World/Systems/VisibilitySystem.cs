using Rhisis.World.Core;
using Rhisis.World.Core.Components;
using Rhisis.World.Core.Entities;
using Rhisis.World.Core.Systems;
using Rhisis.World.Packets;
using System.Linq;

namespace Rhisis.World.Systems
{
    [System]
    public class VisibilitySystem : UpdateSystemBase
    {
        public static readonly float VisibilityRange = 75f;

        public VisibilitySystem(IContext context)
            : base(context)
        {
        }

        public override void Execute(IEntity entity)
        {
            var entityObjectComponent = entity.GetComponent<ObjectComponent>();

            foreach (IEntity otherEntity in this.Entities)
            {
                var otherEntityObjectComponent = otherEntity.GetComponent<ObjectComponent>();

                if (entity.GetHashCode() != otherEntity.GetHashCode() && otherEntityObjectComponent.Spawned)
                {
                    if (entityObjectComponent.Position.IsInCircle(otherEntityObjectComponent.Position, VisibilityRange))
                    {
                        if (!entityObjectComponent.Entities.Contains(otherEntity))
                            this.SpawnOtherEntity(entity, entityObjectComponent, otherEntity, otherEntityObjectComponent);
                    }
                    else
                    {
                        if (entityObjectComponent.Entities.Contains(otherEntity))
                            this.DespawnOtherEntity(entity, entityObjectComponent, otherEntity, otherEntityObjectComponent);
                    }
                }
            }
        }
        
        private void SpawnOtherEntity(IEntity entity, ObjectComponent entityObjectComponent, IEntity otherEntity, ObjectComponent otherEntityObjectComponent)
        {
            var entityPlayerComponent = entity.GetComponent<PlayerComponent>();

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

        private void DespawnOtherEntity(IEntity entity, ObjectComponent entityObjectComponent, IEntity otherEntity, ObjectComponent otherEntityObjectComponent)
        {
            var entityPlayerComponent = entity.GetComponent<PlayerComponent>();

            // Despawn for the current player entity.
            WorldPacketFactory.SendDespawnObject(entityPlayerComponent.Connection, otherEntity);
            entityObjectComponent.Entities.Remove(otherEntity);

            // Despawn for the other entities
            if (otherEntity.EntityType != WorldEntityType.Player && otherEntityObjectComponent.Entities.Contains(entity))
                otherEntityObjectComponent.Entities.Remove(entity);
        }

        private bool EntityHasOther(ObjectComponent entityObjectComponent, IEntity otherEntity)
        {
            for (int i = 0; i < entityObjectComponent.Entities.Count; i++)
            {
                if (entityObjectComponent.Entities[i].Id == otherEntity.Id)
                    return true;
            }

            return false;
        }
    }
}
