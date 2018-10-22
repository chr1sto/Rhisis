using Rhisis.Core.Common;
using Rhisis.World.Game.Core;
using Rhisis.World.Game.Regions;
using System.Collections.Generic;

namespace Rhisis.World.Game.Maps.Regions
{
    public interface IMapRespawnRegion : IRegion
    {
        WorldObjectType ObjectType { get; }

        int ModelId { get; }

        int Time { get; }

        int Count { get; }

        IList<IEntity> Entities { get; }
    }
}
