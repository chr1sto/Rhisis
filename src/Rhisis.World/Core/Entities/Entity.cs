using Rhisis.World.Core.Components;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.World.Core.Entities
{
    /// <summary>
    /// Defines a basic and empty entity.
    /// </summary>
    public class Entity : IEntity, IDisposable
    {
        private readonly ICollection<IComponent> _components;
        private bool _disposedValue;

        /// <summary>
        /// Event fired when a component is attached to this entity.
        /// </summary>
        public event EventHandler<IComponent> ComponentAdded;

        /// <summary>
        /// Event fired when a component is detached from this entity.
        /// </summary>
        public event EventHandler<IComponent> ComponentRemoved;

        /// <summary>
        /// Gets the entity id.
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// Gets the list of the components attached to this entity.
        /// </summary>
        public IReadOnlyCollection<IComponent> Components => this._components as IReadOnlyCollection<IComponent>;

        /// <summary>
        /// Creates a new <see cref="Entity"/> instance.
        /// </summary>
        public Entity()
        {
            this.Id = Guid.NewGuid();
            this._components = new List<IComponent>();
        }

        /// <summary>
        /// Destructs the <see cref="Entity"/>.
        /// </summary>
        ~Entity()
        {
            this.Dispose(false);
        }

        /// <summary>
        /// Gets the component of type passed as template parameter.
        /// </summary>
        /// <typeparam name="T">Component type</typeparam>
        /// <returns>Component</returns>
        public T GetComponent<T>() where T : IComponent => (T)this._components.FirstOrDefault(x => x.GetType() == typeof(T));

        /// <summary>
        /// Adds a component to the entity.
        /// </summary>
        /// <typeparam name="T">Component type</typeparam>
        /// <param name="component">Component to add</param>
        /// <returns>Component</returns>
        public T AddComponent<T>(T component) where T : IComponent
        {
            if (this.HasComponent<T>())
                return default(T);

            this._components.Add(component);
            this.ComponentAdded?.Invoke(this, component);

            return component;
        }

        /// <summary>
        /// Removes a component from the entity.
        /// </summary>
        /// <typeparam name="T">Component type</typeparam>
        /// <param name="component">Component to remove</param>
        public void RemoveComponent<T>(T component) where T : IComponent
        {
            if (this.HasComponent<T>())
            {
                this._components.Remove(component);
                this.ComponentRemoved?.Invoke(this, component);
            }
        }

        /// <summary>
        /// Check if the component exists in the entity.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public bool HasComponent<T>() where T : IComponent => this._components.Any(x => x.GetType() == typeof(T));

        /// <summary>
        /// Disposes the <see cref="Entity"/> resources.
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    this._components.Clear();
                }

                _disposedValue = true;
            }
        }

        /// <summary>
        /// Disposes the <see cref="Entity"/> resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
