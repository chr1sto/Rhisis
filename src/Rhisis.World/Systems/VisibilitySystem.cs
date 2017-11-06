using Rhisis.World.Core.Systems;
using System;
using System.Collections.Generic;
using System.Text;
using Rhisis.World.Core.Entities;
using Rhisis.World.Core.Components;
using Rhisis.World.Core;
using Rhisis.Core.IO;

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
            Logger.Debug("Updating visibility system");
        }
    }
}
