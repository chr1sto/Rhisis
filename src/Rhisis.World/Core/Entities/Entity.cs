using Rhisis.World.Core.Components;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.World.Core.Entities
{
    /// <summary>
    /// Defines a basic and empty entity.
    /// </summary>
    public class Entity : IEntity
    {
        private readonly Guid _id;
        private readonly ICollection<IComponent> _components;

        /// <summary>
        /// Gets the entity id.
        /// </summary>
        public Guid Id => this._id;

        /// <summary>
        /// Gets the list of the components attached to this entity.
        /// </summary>
        public IReadOnlyCollection<IComponent> Components => this._components as IReadOnlyCollection<IComponent>;

        /// <summary>
        /// Creates a new <see cref="Entity"/> instance.
        /// </summary>
        public Entity()
        {
            this._id = Guid.NewGuid();
            this._components = new List<IComponent>();
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
                this._components.Remove(component);
        }

        /// <summary>
        /// Check if the component exists in the entity.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public bool HasComponent<T>() where T : IComponent => this._components.Any(x => x.GetType() == typeof(T));
    }
}
