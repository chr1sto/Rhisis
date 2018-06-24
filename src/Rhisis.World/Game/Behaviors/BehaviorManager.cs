using Rhisis.Core.Reflection;
using Rhisis.World.Game.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Rhisis.World.Game.Behaviors
{
    public sealed class BehaviorManager<T> : IDisposable where T : IEntity
    {
        private readonly BehaviorType _behaviorType;
        private readonly IDictionary<int, IBehavior<T>> _behaviors;

        /// <summary>
        /// Gets the default behavior.
        /// </summary>
        public IBehavior<T> DefaultBehavior { get; private set; }
        
        /// <summary>
        /// Gets the total amount of behaviors in this manager.
        /// </summary>
        public int Count => this._behaviors.Count + 1;

        /// <summary>
        /// Creates a new <see cref="BehaviorManager{T}"/> instance.
        /// </summary>
        /// <param name="behaviorType"></param>
        public BehaviorManager(BehaviorType behaviorType)
        {
            this._behaviorType = behaviorType;
            this._behaviors = new Dictionary<int, IBehavior<T>>();
        }

        /// <summary>
        /// Load behaviors.
        /// </summary>
        public void Load()
        {
            IEnumerable<Type> behaviors = ReflectionHelper.GetClassesAssignableFrom(typeof(IBehavior<>));

            foreach (var type in behaviors)
            {
                var behavior = Activator.CreateInstance(type) as IBehavior<T>;
                var behaviorAttributes = type.GetCustomAttributes<BehaviorAttribute>();

                if (behaviorAttributes != null)
                {
                    foreach (var attribute in behaviorAttributes)
                    {
                        if (attribute.Type != this._behaviorType)
                            continue;

                        if (attribute.IsDefault)
                        {
                            if (this.DefaultBehavior == null)
                                this.DefaultBehavior = behavior;
                            else throw new InvalidOperationException($"Default behavior already set for type '{this._behaviorType.ToString()}'");
                        }
                        else
                        {
                            this._behaviors.Add(attribute.MoverId, behavior);
                        }
                    }
                }
            }

            if (this.DefaultBehavior == null)
                throw new InvalidOperationException($"Default behavior not set for type '{this._behaviorType.ToString()}'");
        }

        /// <summary>
        /// Gets a specific behavior.
        /// </summary>
        /// <param name="moverId"></param>
        /// <returns></returns>
        public IBehavior<T> GetBehavior(int moverId)
        {
            return this._behaviors.TryGetValue(moverId, out IBehavior<T> behavior) ? behavior : null;
        }

        /// <summary>
        /// Dispose the behavior manager resources.
        /// </summary>
        public void Dispose()
        {
            this._behaviors.Clear();
        }
    }
}
