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

                IEnumerable<IEntity> otherEntitiesAround = from x in this.Entities
                                                           let otherEntityPosition = x.GetComponent<ObjectComponent>().Position
                                                           where entityObjectComponent.Position.IsInCircle(otherEntityPosition, 75f) &&
                                                                 !entityObjectComponent.Entities.Contains(x)
                                                           select x;

                IEnumerable<IEntity> otherEntitiesOut = from x in entityObjectComponent.Entities
                                                        where !otherEntitiesAround.Contains(x)
                                                        select x;

                foreach (IEntity entityOutOfRange in otherEntitiesOut)
                {
                    entityObjectComponent.Entities.Remove(entityOutOfRange);

                    if (entityPlayerComponent != null)
                        WorldPacketFactory.SendDespawn(entityPlayerComponent.Connection, entityOutOfRange);
                }

                foreach (IEntity entityInRange in otherEntitiesAround)
                {
                    entityObjectComponent.Entities.Add(entity);

                    if (entityPlayerComponent != null)
                        WorldPacketFactory.SendSpawn(entityPlayerComponent.Connection, entityInRange);
                }
            }
        }
    }
}
