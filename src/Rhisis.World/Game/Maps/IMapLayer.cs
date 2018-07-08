using Rhisis.World.Game.Core;

namespace Rhisis.World.Game.Maps
{
    public interface IMapLayer : IContext
    {
        int Id { get; }

        IMapInstance Parent { get; }
    }
}
