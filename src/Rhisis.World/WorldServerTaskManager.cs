using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Rhisis.Core.DependencyInjection;
using Rhisis.Core.Extensions;
using Rhisis.World.Game.Entities;
using Rhisis.World.Game.Maps;
using Rhisis.World.Systems;
using Rhisis.World.Systems.Visibility;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Rhisis.World
{
    [Injectable(ServiceLifetime.Singleton)]
    public class WorldServerTaskManager : IWorldServerTaskManager
    {
        private readonly CancellationToken _cancellationToken;
        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly ILogger<WorldServerTaskManager> _logger;
        private readonly IMapManager _mapManager;
        private readonly IVisibilitySystem _visibilitySystem;
        private readonly IRespawnSystem _respawnSystem;

        /// <summary>
        /// Creates a new <see cref="WorldServerTaskManager"/> instance.
        /// </summary>
        /// <param name="logger">Logger.</param>
        /// <param name="mapManager">Map manager.</param>
        /// <param name="visibilitySystem">Visibility System.</param>
        /// <param name="respawnSystem">Respawn System.</param>
        public WorldServerTaskManager(ILogger<WorldServerTaskManager> logger, IMapManager mapManager, IVisibilitySystem visibilitySystem, IRespawnSystem respawnSystem)
        {
            _cancellationTokenSource = new CancellationTokenSource();
            _cancellationToken = _cancellationTokenSource.Token;
            _logger = logger;
            _mapManager = mapManager;
            _visibilitySystem = visibilitySystem;
            _respawnSystem = respawnSystem;
        }

        /// <inheritdoc />
        public void Start()
        {
            TaskHelper.CreateLongRunningTask(UpdateServerHeartbeat, TimeSpan.FromSeconds(1), _cancellationToken);
            TaskHelper.CreateLongRunningTask(UpdateServerObjects, TimeSpan.FromMilliseconds(67), _cancellationToken);
        }

        /// <inheritdoc />
        public void Stop()
        {
            _cancellationTokenSource.Cancel();
        }

        /// <summary>
        /// Update the server heartbeat every second.
        /// </summary>
        /// <returns></returns>
        private Task UpdateServerHeartbeat()
        {
            return ForeachEntities(entity =>
            {
                try
                {
                    _visibilitySystem.Execute(entity);
                    _respawnSystem.Execute(entity);
                }
                catch (Exception e)
                {
                    _logger.LogError(e, $"An error occured while updating server timers for entity: '{entity}'.");
                }
            });
        }

        /// <summary>
        /// Updates the server living objects.
        /// </summary>
        /// <returns></returns>
        private Task UpdateServerObjects()
        {
            return ForeachEntities(entity =>
            {
                try
                {
                    if (entity is ILivingEntity livingEntity)
                    {
                        livingEntity.Behavior?.Update();
                    }
                }
                catch (Exception e)
                {
                    _logger.LogError(e, $"An error occured while updating object: '{entity}'");
                }
            });
        }

        /// <summary>
        /// Loop throught each entities of each map layers.
        /// </summary>
        /// <param name="actionToExecute">Action to execute.</param>
        /// <returns></returns>
        private Task ForeachEntities(Action<IWorldEntity> actionToExecute)
        {
            foreach (IMapInstance map in _mapManager.Maps)
            {
                foreach (IWorldEntity mapEntity in map.Entities.Values)
                {
                    actionToExecute(mapEntity);
                }

                foreach (IMapLayer layer in map.Layers)
                {
                    foreach (IWorldEntity layerEntity in layer.Entities.Values)
                    {
                        actionToExecute(layerEntity);
                    }
                }
            }

            return Task.CompletedTask;
        }
    }
}
