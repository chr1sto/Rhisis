using Rhisis.Core.IO;
using Rhisis.World.Game.Core.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Rhisis.World.Game.Core
{
    /// <summary>
    /// Implementation of a context.
    /// </summary>
    public class Context : IContext
    {
        private static readonly object SyncPlayersLock = new object();

        private bool _disposedValue;

        private readonly IList<ISystem> _systems;
        private readonly IList<IEntity> _playersEntities;
        private readonly IDictionary<int, IEntity> _entities;
        private readonly CancellationToken _cancellationToken;
        private readonly CancellationTokenSource _cancellationTokenSource;

        /// <inheritdoc />
        public double Time { get; private set; }

        /// <inheritdoc />
        public IReadOnlyList<ISystem> Systems => this._systems as IReadOnlyList<ISystem>;

        /// <inheritdoc />
        public IReadOnlyList<IEntity> Entities => this._entities.Values as IReadOnlyList<IEntity>;

        /// <summary>
        /// Creates a new <see cref="Context"/> instance.
        /// </summary>
        public Context()
        {
            this._systems = new List<ISystem>();
            this._playersEntities = new List<IEntity>();
            this._entities = new ConcurrentDictionary<int, IEntity>();
            this._cancellationTokenSource = new CancellationTokenSource();
            this._cancellationToken = this._cancellationTokenSource.Token;
        }

        /// <inheritdoc />
        public TEntity CreateEntity<TEntity>() where TEntity : class, IEntity
        {
            var entity = Activator.CreateInstance(typeof(TEntity), this) as IEntity;

            if (entity == null)
                return null;

            this._entities.Add(entity.Id, entity);

            if (entity.Type == WorldEntityType.Player)
            {
                lock (SyncPlayersLock)
                {
                    this._playersEntities.Add(entity);
                }
            }
            
            return (TEntity)entity;
        }

        /// <inheritdoc />
        public bool DeleteEntity(IEntity entity)
        {
            bool removed = this._entities.Remove(entity.Id);

            if (entity.Type == WorldEntityType.Player)
            {
                lock (SyncPlayersLock)
                {
                    removed = this._playersEntities.Remove(entity);
                }
            }

            return removed;
        }

        /// <inheritdoc />
        public T FindEntity<T>(int id) where T : IEntity => this._entities.TryGetValue(id, out IEntity value) ? (T)value : default(T);

        /// <inheritdoc />
        public void StartSystemUpdate(int delay)
        {
            Task.Run(async () =>
            {
                double previousTime = 0f;

                while (true)
                {
                    if (this._cancellationToken.IsCancellationRequested)
                        break;

                    double currentTime = Rhisis.Core.IO.Time.TimeInMilliseconds();
                    double deltaTime = currentTime - previousTime;
                    previousTime = currentTime;

                    this.Time = deltaTime / 1000f;

                    try
                    {
                        lock (SyncPlayersLock)
                        {
                            foreach (var entity in this._playersEntities)
                            {
                                foreach (var system in this._systems)
                                {
                                    if (!(system is INotifiableSystem) && system.Match(entity))
                                        system.Execute(entity);
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Logger.Error("Context error: {0}", e.Message);
                        Logger.Debug(e.StackTrace);
                    }

                    await Task.Delay(delay, this._cancellationToken).ConfigureAwait(false);
                }
            }, this._cancellationToken);
        }

        /// <inheritdoc />
        public void StopSystemUpdate()
        {
            this._cancellationTokenSource.Cancel();
        }

        /// <inheritdoc />
        public void AddSystem(ISystem system) => this._systems.Add(system);

        /// <inheritdoc />
        public void RemoveSystem(ISystem system) => this._systems.Remove(system);

        /// <inheritdoc />
        public void NotifySystem<T>(IEntity entity, SystemEventArgs e) where T : class, INotifiableSystem
        {
            if (this._systems.FirstOrDefault(x => x.GetType() == typeof(T)) is INotifiableSystem system && system.Match(entity))
                system.Execute(entity, e);
        }

        /// <inheritdoc />
        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposedValue)
            {
                if (disposing)
                {
                    this.StopSystemUpdate();

                    foreach (var entity in this.Entities)
                        entity.Dispose();

                    this._entities.Clear();
                    this._systems.Clear();
                }
                
                this._disposedValue = true;
            }
        }

        /// <inheritdoc />
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
