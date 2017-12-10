using Rhisis.World.Core.Entities;
using Rhisis.World.Core.Systems;
using System;
using System.Collections.Generic;

namespace Rhisis.World.Core
{
    public interface IContext : IDisposable
    {
        double Time { get; }

        ICollection<IEntity> Entities { get; }

        ICollection<ISystem> Systems { get; }

        IEntity CreateEntity();

        IEntity CreateEntity(WorldEntityType entityType);

        bool DeleteEntity(IEntity entity);

        IEntity FindEntity(int id);

        void AddSystem(ISystem system);

        void RemoveSystem(ISystem system);

        void NotifySystem<T>(IEntity entity, EventArgs e) where T : IReactiveSystem;

        void StartSystemUpdate(int delay);

        void StopSystemUpdate();
    }
}
