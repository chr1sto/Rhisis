using System;

namespace Rhisis.World.Game.Maps
{
    public interface IMapLayer
    {
        IMapInstance Parent { get; }
    }
}
