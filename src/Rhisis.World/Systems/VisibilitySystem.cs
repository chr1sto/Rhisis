using Rhisis.World.Core;
using Rhisis.World.Core.Entities;
using Rhisis.World.Core.Systems;

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

        }
    }
}
