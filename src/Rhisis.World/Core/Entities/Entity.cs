﻿using Rhisis.Core.Helpers;
using Rhisis.World.Core.Components;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.World.Core.Entities
{
    /// <summary>
    /// Defines a basic and empty entity.
    /// </summary>
    public class Entity : IEntity, IDisposable, IEqualityComparer<IEntity>
    {
        private readonly IDictionary<Type, IComponent> _components;
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
        public int Id { get; }

        /// <summary>
        /// Gets or sets the entity type.
        /// </summary>
        public WorldEntityType EntityType { get; set; }

        /// <summary>
        /// Gets the current entity context.
        /// </summary>
        public IContext Context { get; }

        /// <summary>
        /// Gets the list of the components attached to this entity.
        /// </summary>
        public IReadOnlyCollection<IComponent> Components => this._components as IReadOnlyCollection<IComponent>;

        /// <summary>
        /// Creates a new <see cref="Entity"/> instance.
        /// </summary>
        /// <param name="context">Current context of the entity</param>
        internal Entity(IContext context)
        {
            this.Id = RandomHelper.GenerateUniqueId();
            this.Context = context;
            this._components = new Dictionary<Type, IComponent>();
        }

        /// <summary>
        /// Gets the component of type passed as template parameter.
        /// </summary>
        /// <typeparam name="T">Component type</typeparam>
        /// <returns>Component</returns>
        public T GetComponent<T>() where T : IComponent
        {
            if (this._components.TryGetValue(typeof(T), out IComponent value))
                return (T)value;

            return default(T);
        }

        /// <summary>
        /// Adds a component to the entity.
        /// </summary>
        /// <typeparam name="T">Component type</typeparam>
        /// <param name="component">Component to add</param>
        /// <returns>Component</returns>
        public T AddComponent<T>(T component) where T : IComponent
        {
            if (this.HasComponent<T>())
                throw new ArgumentException("This component type is already attached to the entity.", nameof(component));

            this._components.Add(typeof(T), component);
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
                this._components.Remove(component.GetType());
                this.ComponentRemoved?.Invoke(this, component);
            }
        }

        /// <summary>
        /// Check if the component is attached to this entity.
        /// </summary>
        /// <typeparam name="T">Component type</typeparam>
        /// <returns></returns>
        public bool HasComponent<T>() where T : IComponent => this.HasComponent(typeof(T));

        /// <summary>
        /// Check if the component is attached to this entity.
        /// </summary>
        /// <param name="componentType">Component type</param>
        /// <returns></returns>
        private bool HasComponent(Type componentType) => this._components.ContainsKey(componentType);

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
                    this.ComponentRemoved?.Invoke(this, null);
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

        public bool Equals(IEntity x, IEntity y)
        {
            return x.Id == y.Id;
        }

        public int GetHashCode(IEntity obj)
        {
            int hCode = obj.Id ^ (int)obj.EntityType;

            return hCode.GetHashCode();
        }
    }
}
