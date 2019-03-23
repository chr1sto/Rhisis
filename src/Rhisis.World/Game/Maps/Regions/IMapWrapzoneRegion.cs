using Rhisis.Core.Structures;
namespace Rhisis.World.Game.Maps.Regions
{
    public interface IMapWrapzoneRegion : IMapRegion
    {
        int DestMapId { get; }

        Vector3 DestPosition { get; }
    }
}
