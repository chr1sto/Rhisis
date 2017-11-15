using Rhisis.Core.IO;
using Rhisis.World.Core;
using Rhisis.World.Core.Components;
using Rhisis.World.Core.Entities;
using Rhisis.World.Core.Systems;
using Rhisis.World.Packets;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.World.Systems
{
    [System]
    public class VisibilitySystem : UpdateSystemBase
    {
        public override Func<IEntity, bool> Filter => x => x.HasComponent<ObjectComponent>();

        public VisibilitySystem(IContext context)
            : base(context)
        {
        }

        public override void Execute()
        {
            foreach (var entity in this.Entities)
            {
                var entityPlayerComponent = entity.GetComponent<PlayerComponent>();

                if (entityPlayerComponent == null)
                    continue;

                var entityObjectComponent = entity.GetComponent<ObjectComponent>();
                IEnumerable<IEntity> otherEntitiesAround = this.Entities.Where(x => this.CanSee(entity, x));
                IEnumerable<IEntity> otherEntitiesOut = entityObjectComponent.Entities.Where(x => !otherEntitiesAround.Contains(x));

                foreach (IEntity entityInRange in otherEntitiesAround)
                {
                    if (!entityObjectComponent.Entities.Contains(entityInRange))
                    {
                        entityObjectComponent.Entities.Add(entityInRange);

                        if (entityPlayerComponent != null)
                            WorldPacketFactory.SendSpawnObject(entityPlayerComponent.Connection, entityInRange);
                    }

                    // Also add the current entity to the entity in range to gain performance
                    var otherEntityObjectComponent = entityInRange.GetComponent<ObjectComponent>();

                    if (!otherEntityObjectComponent.Entities.Contains(entity))
                    {
                        otherEntityObjectComponent.Entities.Add(entity);

                        var otherEntityPlayerComponent = entityInRange.GetComponent<PlayerComponent>();
                        if (otherEntityPlayerComponent != null)
                            WorldPacketFactory.SendSpawnObject(otherEntityPlayerComponent.Connection, entity);
                    }
                }

                for (int i = otherEntitiesOut.Count(); i > 0; i--)
                {
                    if (entityPlayerComponent != null)
                        WorldPacketFactory.SendDespawnObject(entityPlayerComponent.Connection, otherEntitiesOut.ElementAt(0));

                    var otherEntityOut = otherEntitiesOut.ElementAt(0);

                    var otherEntityOutObjectComponent = otherEntityOut.GetComponent<ObjectComponent>();
                    var otherEntityOutPlayerComponent = otherEntityOut.GetComponent<PlayerComponent>();

                    if (otherEntityOutPlayerComponent != null)
                        WorldPacketFactory.SendDespawnObject(otherEntityOutPlayerComponent.Connection, entity);
                    otherEntityOutObjectComponent.Entities.Remove(entity);

                    entityObjectComponent.Entities.RemoveAt(0);
                }
            }
        }

        private bool CanSee(IEntity entity, IEntity otherEntity)
        {
            var entityObjectComponent = entity.GetComponent<ObjectComponent>();
            var otherEntityObjectComponent = otherEntity.GetComponent<ObjectComponent>();

            return entityObjectComponent.Position.IsInCircle(otherEntityObjectComponent.Position, 75f)
                && entityObjectComponent.ObjectId != otherEntityObjectComponent.ObjectId;
        }
    }
}
