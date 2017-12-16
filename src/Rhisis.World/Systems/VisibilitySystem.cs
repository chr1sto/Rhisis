using System;
using System.Linq.Expressions;
using Rhisis.World.Core.Systems;
using Rhisis.World.Game.Core;
using Rhisis.World.Game.Core.Interfaces;

namespace Rhisis.World.Systems
{
    [System]
    public class VisibilitySystem : SystemBase
    {
        public static readonly float VisibilityRange = 75f;

        protected override Expression<Func<IEntity, bool>> Filter => x => x.Type == WorldEntityType.All;

        public VisibilitySystem(IContext context)
            : base(context)
        {
        }

        public override void Execute(IEntity entity)
        {

        }
    }
}
