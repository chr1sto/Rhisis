using Rhisis.World.Core.Entities;
using Rhisis.World.Core.Systems;
using System;
using System.Collections.Generic;

namespace Rhisis.World.Core
{
    public interface IContext : IDisposable
    {
        IReadOnlyCollection<IEntity> Entities { get; }

        IReadOnlyCollection<ISystem> Systems { get; }

        IEntity CreateEntity();

        bool DeleteEntity(IEntity entity);

        IEntity FindEntity(Guid id);

        void AddSystem(ISystem system);

        void RemoveSystem(ISystem system);
    }
}
