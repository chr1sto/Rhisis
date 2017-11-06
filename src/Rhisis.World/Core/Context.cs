using Rhisis.World.Core.Entities;
using Rhisis.World.Core.Systems;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Rhisis.World.Core
{
    public class Context : IContext, IDisposable
    {
        private static readonly object _syncSystemLock = new object();
        private static readonly Lazy<IContext> lazyInstance = new Lazy<IContext>(() => new Context());

        /// <summary>
        /// Gets a shared context instance.
        /// </summary>
        public static IContext Shared => lazyInstance.Value;

        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly CancellationToken _cancellationToken;
        private readonly IDictionary<Guid, IEntity> _entities;
        private readonly IList<ISystem> _systems;
        private bool _disposedValue;
        
        /// <summary>
        /// Gets the entities of the present context.
        /// </summary>
        public IReadOnlyCollection<IEntity> Entities => this._entities.Values as IReadOnlyCollection<IEntity>;

        /// <summary>
        /// Gets the systems of the present context.
        /// </summary>
        public IReadOnlyCollection<ISystem> Systems => this._systems as IReadOnlyCollection<ISystem>;

        /// <summary>
        /// Creates and initializes a new <see cref="Context"/>.
        /// </summary>
        public Context()
        {
            this._cancellationTokenSource = new CancellationTokenSource();
            this._cancellationToken = this._cancellationTokenSource.Token;
            this._entities = new ConcurrentDictionary<Guid, IEntity>();
            this._systems = new List<ISystem>();
        }

        /// <summary>
        /// Destroys and cleans the current <see cref="Context"/>.
        /// </summary>
        ~Context()
        {
            this.Dispose(false);
        }

        public IEntity CreateEntity()
        {
            var entity = new Entity();

            entity.ComponentAdded += (sender, e) => this.RefreshSystems();
            entity.ComponentRemoved += (sender, e) => this.RefreshSystems();

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

        public void StartSystemUpdate(int delay)
        {
            Task.Factory.StartNew(async () =>
            {
                while (true)
                {
                    if (this._cancellationToken.IsCancellationRequested)
                        break;

                    lock (_syncSystemLock)
                    {
                        foreach (var system in this._systems)
                            system.Execute();
                    }

                    await Task.Delay(50);
                }
            }, this._cancellationToken);
        }

        public void StopSystemUpdate()
        {
            this._cancellationTokenSource.Cancel();
        }

        private void RefreshSystems()
        {
            lock (_syncSystemLock)
            {
                foreach (var system in this._systems)
                    system.Refresh();
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    this.StopSystemUpdate();

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
