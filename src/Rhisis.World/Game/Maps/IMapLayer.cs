using System;
using Rhisis.World.Game.Core.Interfaces;

namespace Rhisis.World.Game.Maps
{
    public interface IMapLayer : IContext
    {
        int Id { get; }

        IMapInstance Parent { get; }
    }
}
