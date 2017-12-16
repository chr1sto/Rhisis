using System;
using System.Collections.Generic;

namespace Rhisis.World.Game.Core.Interfaces
{
    public interface IContext : IDisposable
    {
        double Time { get; }

        IReadOnlyList<ISystem> Systems { get; }

        IReadOnlyList<IEntity> Entities { get; }

        TEntity CreateEntity<TEntity>() where TEntity : class, IEntity;

        bool DeleteEntity(IEntity entity);

        IEntity FindEntity(int id);

        void StartSystemUpdate(int delay);

        void StopSystemUpdate();

        void AddSystem(ISystem system);

        void RemoveSystem(ISystem system);

        void NotifySystem<T>(IEntity entity, EventArgs e) where T : class, INotifiableSystem;
    }
}
