using Rhisis.World.Game.Components;
using System;
using System.Collections.Generic;

namespace Rhisis.World.Game.Core.Interfaces
{
    public interface IEntity : IDisposable, IEqualityComparer<IEntity>
    {
        int Id { get; }

        WorldEntityType Type { get; }

        IContext Context { get; }

        ObjectComponent ObjectComponent { get; set; }
    }
}
