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
    public class VisibilitySystem : SystemBase
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
                var entityObjectComponent = entity.GetComponent<ObjectComponent>();
                var entityPlayerComponent = entity.GetComponent<PlayerComponent>();

                if (entityObjectComponent == null)
                    continue;

                IEnumerable<IEntity> otherEntitiesAround = from x in this.Entities
                                                           where CanSee(entity, x)
                                                           select x;

                IEnumerable<IEntity> otherEntitiesOut = from x in entityObjectComponent.Entities
                                                        where !otherEntitiesAround.Contains(x)
                                                        select x;

                for (int i = otherEntitiesOut.Count(); i > 0; i--)
                {
                    if (entityPlayerComponent != null)
                        WorldPacketFactory.SendDespawn(entityPlayerComponent.Connection, otherEntitiesOut.ElementAt(0));

                    entityObjectComponent.Entities.RemoveAt(0);
                }

                foreach (IEntity entityInRange in otherEntitiesAround)
                {
                    if (!entityObjectComponent.Entities.Contains(entityInRange))
                    {
                        entityObjectComponent.Entities.Add(entityInRange);

                        if (entityPlayerComponent != null)
                            WorldPacketFactory.SendSpawn(entityPlayerComponent.Connection, entityInRange);
                    }
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
