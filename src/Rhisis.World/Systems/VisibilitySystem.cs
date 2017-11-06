using Rhisis.World.Core.Systems;
using System;
using System.Collections.Generic;
using System.Text;
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
                var otherEntitiesAround = from x in this.Entities
                                          where entity.GetComponent<ObjectComponent>().Position.IsInCircle(x.GetComponent<ObjectComponent>().Position, 75f)
                                          select x;

                foreach (var otherEntity in otherEntitiesAround)
                {

                }
            }
        }
    }
}
