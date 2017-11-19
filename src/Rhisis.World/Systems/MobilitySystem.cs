using Rhisis.World.Core;
using Rhisis.World.Core.Components;
using Rhisis.World.Core.Entities;
using Rhisis.World.Core.Systems;
using System;
using System.Linq.Expressions;

namespace Rhisis.World.Systems
{
    [System]
    public class MobilitySystem : UpdateSystemBase
    {
        protected override Expression<Func<IEntity, bool>> Filter => x => x.HasComponent<ObjectComponent>() && x.HasComponent<MovableComponent>();

        public MobilitySystem(IContext context)
            : base(context)
        {
        }

        public override void Execute(IEntity entity)
        {
            // TODO
        }
    }
}
