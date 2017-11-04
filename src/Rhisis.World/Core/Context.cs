using Rhisis.World.Core.Entities;
using Rhisis.World.Core.Systems;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Rhisis.World.Core
{
    public class Context : IContext, IDisposable
    {
        private static readonly Lazy<IContext> lazyInstance = new Lazy<IContext>(() => new Context());

        public static IContext Shared => lazyInstance.Value;

        private readonly IDictionary<Guid, IEntity> _entities;
        private readonly IList<ISystem> _systems;
        private bool _disposedValue;

        public IReadOnlyCollection<IEntity> Entities => this._entities.Values as IReadOnlyCollection<IEntity>;

        public IReadOnlyCollection<ISystem> Systems => this._systems as IReadOnlyCollection<ISystem>;

        public Context()
        {
            this._entities = new ConcurrentDictionary<Guid, IEntity>();
            this._systems = new List<ISystem>();
        }

        ~Context()
        {
            this.Dispose(false);
        }

        public IEntity CreateEntity()
        {
            var entity = new Entity();

            if (this._entities.TryAdd(entity.Id, entity))
                return entity;

            return null;
        }

        public bool DeleteEntity(IEntity entity) => this._entities.Remove(entity.Id);

        public IEntity FindEntity(Guid id)
        {
            throw new NotImplementedException();
        }

        public void AddSystem(ISystem system) => this._systems.Add(system);

        public void RemoveSystem(ISystem system) => this._systems.Remove(system);
        
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    foreach (var entity in this.Entities)
                        entity.Dispose();

                    this._entities.Clear();
                    this._systems.Clear();
                }

                _disposedValue = true;
            }
        }
        
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
