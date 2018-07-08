using System;
using System.Collections.Generic;

namespace Rhisis.World.Game.Core
{
    public abstract class Context : IContext
    {
        private readonly IList<IEntity> _entities;
        private bool disposedValue = false;

        /// <inheritdoc />
        public double GameTime { get; protected set; }

        /// <inheritdoc />
        public IEnumerable<IEntity> Entities => this._entities;

        /// <summary>
        /// Creates a new <see cref="Context"/> instance.
        /// </summary>
        public Context()
        {
            this._entities = new List<IEntity>();
        }

        /// <inheritdoc />
        public TEntity CreateEntity<TEntity>() where TEntity : IEntity
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public TEntity DeleteEntity<TEntity>(int id) where TEntity : IEntity
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public TEntity DeleteEntity<TEntity>(TEntity entity) where TEntity : IEntity
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public TEntity FindEntity<TEntity>(int id) where TEntity : IEntity
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public abstract void Update();

        /// <inheritdoc />
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose the context resources.
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing)
                {
                    this._entities.Clear();
                }

                this.disposedValue = true;
            }
        }

        /// <inheritdoc />
        public void NotifySystem<TSystem>(IEntity entity, SystemEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}