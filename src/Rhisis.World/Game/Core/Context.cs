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
    public class Context : IContext, IDisposable
    {
        private static readonly object _syncPlayersLock = new object();

        private bool _disposedValue;

        private readonly IList<ISystem> _systems;
        private readonly IList<IEntity> _playersEntities;
        private readonly IDictionary<int, IEntity> _entities;
        private readonly CancellationToken _cancellationToken;
        private readonly CancellationTokenSource _cancellationTokenSource;

        public double Time { get; private set; }

        public IReadOnlyList<ISystem> Systems => this._systems as IReadOnlyList<ISystem>;

        public IReadOnlyList<IEntity> Entities => this._entities.Values as IReadOnlyList<IEntity>;

        public Context()
        {
            this._systems = new List<ISystem>();
            this._playersEntities = new List<IEntity>();
            this._entities = new ConcurrentDictionary<int, IEntity>();
            this._cancellationTokenSource = new CancellationTokenSource();
            this._cancellationToken = this._cancellationTokenSource.Token;
        }

        public TEntity CreateEntity<TEntity>() where TEntity : class, IEntity
        {
            var entity = Activator.CreateInstance(typeof(TEntity), this) as IEntity;

            this._entities.Add(entity.Id, entity);

            if (entity.Type == WorldEntityType.Player)
            {
                lock (_syncPlayersLock)
                {
                    this._playersEntities.Add(entity);
                }
            }
            
            return (TEntity)entity;
        }

        public bool DeleteEntity(IEntity entity)
        {
            bool removed = this._entities.Remove(entity.Id);

            if (entity.Type == WorldEntityType.Player)
            {
                lock (_syncPlayersLock)
                {
                    removed = this._playersEntities.Remove(entity);
                }
            }

            return removed;
        }

        public IEntity FindEntity(int id) => this._entities.TryGetValue(id, out IEntity value) ? value : null;

        public void StartSystemUpdate(int delay)
        {
            Task.Run(async () =>
            {
                double deltaTime = 0f;
                double currentTime = 0f;
                double previousTime = 0f;

                while (true)
                {
                    if (this._cancellationToken.IsCancellationRequested)
                        break;

                    currentTime = Rhisis.Core.IO.Time.TimeInMilliseconds();
                    deltaTime = currentTime - previousTime;
                    previousTime = currentTime;

                    this.Time = deltaTime / 1000f;

                    try
                    {
                        lock (_syncPlayersLock)
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

                    await Task.Delay(delay).ConfigureAwait(false);
                }
            }, this._cancellationToken);
        }

        public void StopSystemUpdate()
        {
            this._cancellationTokenSource.Cancel();
        }

        public void AddSystem(ISystem system) => this._systems.Add(system);

        public void RemoveSystem(ISystem system) => this._systems.Remove(system);

        public void NotifySystem<T>(IEntity entity, EventArgs e) where T : class, INotifiableSystem
        {
            if (this._systems.FirstOrDefault(x => x.GetType() == typeof(T)) is INotifiableSystem system && system.Match(entity))
                system.Execute(entity, e);
        }

        /// <summary>
        /// Dispose the resources of this context.
        /// </summary>
        /// <param name="disposing"></param>
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

        /// <summary>
        /// Dispose the resources of this context.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
