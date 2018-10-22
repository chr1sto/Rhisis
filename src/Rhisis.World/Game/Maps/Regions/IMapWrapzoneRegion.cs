using Rhisis.Core.Structures;
using Rhisis.World.Game.Regions;

namespace Rhisis.World.Game.Maps.Regions
{
    public interface IMapWrapzoneRegion : IRegion
    {
        int DestMapId { get; }

        Vector3 DestPosition { get; }
    }
}
