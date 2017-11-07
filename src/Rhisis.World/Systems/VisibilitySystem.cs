using Rhisis.World.Core.Systems;
using System;
using System.Collections.Generic;
using Rhisis.World.Core.Entities;
using Rhisis.World.Core.Components;
using Rhisis.World.Core;
using Rhisis.Core.IO;
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
                    // Send despawn packet
                }

                foreach (IEntity entityInRange in otherEntitiesAround)
                {
                    entityObjectComponent.Entities.Add(entity);
                    // Send spawn packet
                }
            }
        }
    }
}
