using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Rhisis.Core.DependencyInjection;
using Rhisis.Core.Helpers;
using Rhisis.World.Game.Entities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Rhisis.World.Game.Behaviors
{
    [Injectable(ServiceLifetime.Singleton)]
    public sealed class BehaviorManager : IBehaviorManager
    {
        private class BehaviorEntryCache
        {
            public TypeInfo BehaviorTypeInfo { get; }

            public bool IsDefault { get; }

            public int MoverId { get; }

            public BehaviorEntryCache(TypeInfo behaviorTypeInfo, bool isDefault, int moverId = -1)
            {
                BehaviorTypeInfo = behaviorTypeInfo;
                IsDefault = isDefault;
                MoverId = moverId;
            }
        }

        private readonly ILogger<BehaviorManager> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly ConcurrentDictionary<BehaviorType, IList<BehaviorEntryCache>> _behaviors;

        /// <inheritdoc />
        public int Count { get; private set; }

        /// <summary>
        /// Creates a new <see cref="BehaviorManager"/> instance.
        /// </summary>
        /// <param name="logger">Logger.</param>
        /// <param name="serviceProvider">Service provider.</param>
        public BehaviorManager(ILogger<BehaviorManager> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _behaviors = new ConcurrentDictionary<BehaviorType, IList<BehaviorEntryCache>>();
        }

        /// <inheritdoc />
        public void Load()
        {
            IEnumerable<TypeInfo> behaviors = ReflectionHelper.GetClassesAssignableFrom<IBehavior>();

            foreach (var type in behaviors)
            {
                IEnumerable<BehaviorAttribute> behaviorAttributes = type.GetCustomAttributes<BehaviorAttribute>();

                if (behaviorAttributes != null)
                {
                    foreach (BehaviorAttribute attribute in behaviorAttributes)
                    {
                        if (!_behaviors.TryGetValue(attribute.Type, out IList<BehaviorEntryCache> behaviorsCache))
                        {
                            behaviorsCache = new List<BehaviorEntryCache>();
                            _behaviors.TryAdd(attribute.Type, behaviorsCache);
                        }

                        if (attribute.IsDefault)
                        {
                            if (behaviorsCache.Any(x => x.IsDefault))
                            {
                                throw new InvalidOperationException($"{attribute.Type} already has a default behavior.");
                            }

                            var entityBehavior = new BehaviorEntryCache(type, true);

                            behaviorsCache.Add(entityBehavior);
                        }
                        else
                        {
                            if (behaviorsCache.Any(x => x.MoverId == attribute.MoverId))
                            {
                                _logger.LogWarning($"Behavior for mover id {attribute.MoverId} and type {type} is already set.");
                                continue;
                            }

                            var entityBehavior = new BehaviorEntryCache(type, false, attribute.MoverId);

                            behaviorsCache.Add(entityBehavior);
                        }
                    }
                }
            }

            foreach (var behaviorsForType in _behaviors)
            {
                if (!behaviorsForType.Value.Any(x => x.IsDefault))
                {
                    throw new InvalidProgramException($"{behaviorsForType.Key}");
                }
            }

            Count = _behaviors.Aggregate(0, (current, next) => current + next.Value.Count);
            _logger.LogInformation("-> {0} behaviors loaded.", Count);
        }

        /// <inheritdoc />
        public IBehavior GetBehavior(BehaviorType type, IWorldEntity entity, int moverId)
        {
            BehaviorEntryCache behaviorEntry = GetBehaviorEntry(type, x => x.MoverId == moverId);

            return behaviorEntry == null ? GetDefaultBehavior(type, entity) : CreateBehaviorInstance(behaviorEntry, entity);
        }

        /// <inheritdoc />
        public IBehavior GetDefaultBehavior(BehaviorType type, IWorldEntity entity)
        {
            BehaviorEntryCache behaviorEntry = GetBehaviorEntry(type, x => x.IsDefault);

            if (behaviorEntry == null)
            {
                throw new ArgumentNullException(nameof(behaviorEntry), $"Cannot find default behavior for type {type}.");
            }

            return CreateBehaviorInstance(behaviorEntry, entity);
        }

        /// <summary>
        /// Gets a behavior cache entry based on a behavior type and a predicate.
        /// </summary>
        /// <param name="type">Behavior type.</param>
        /// <param name="predicate">Predicate.</param>
        /// <returns></returns>
        private BehaviorEntryCache GetBehaviorEntry(BehaviorType type, Func<BehaviorEntryCache, bool> predicate)
        {
            if (!_behaviors.TryGetValue(type, out IList<BehaviorEntryCache> behaviors))
            {
                throw new KeyNotFoundException($"No behaviors for type {type}.");
            }

            return behaviors.FirstOrDefault(predicate);
        }

        /// <summary>
        /// Creates a new behavior instance.
        /// </summary>
        /// <param name="behaviorEntry">Behavior entry informations.</param>
        /// <param name="entity">Entity.</param>
        /// <returns>Behavior.</returns>
        private IBehavior CreateBehaviorInstance(BehaviorEntryCache behaviorEntry, IWorldEntity entity) 
            => ActivatorUtilities.CreateInstance(_serviceProvider, behaviorEntry.BehaviorTypeInfo, entity) as IBehavior;
    }
}
