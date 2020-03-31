using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Rhisis.Core.DependencyInjection;
using Rhisis.Core.Resources;
using Rhisis.Core.Structures.Configuration.World;
using Rhisis.World.Game.Factories;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;

namespace Rhisis.World.Game.Maps
{
    /// <summary>
    /// Provides a mechanism to load and manage maps.
    /// </summary>
    [Injectable(ServiceLifetime.Singleton)]
    public class MapManager : IMapManager
    {
        private readonly ILogger<MapManager> _logger;
        private readonly WorldConfiguration _worldConfiguration;
        private readonly IGameResources _gameResources;
        private readonly IMapFactory _mapFactory;
        private readonly IDictionary<int, IMapInstance> _maps;

        /// <inheritdoc />
        public IEnumerable<IMapInstance> Maps => _maps.Values;

        /// <summary>
        /// Creates a new <see cref="MapManager"/> instance.
        /// </summary>
        /// <param name="logger">Logger.</param>
        /// <param name="worldConfiguration">World server configuration.</param>
        /// <param name="gameResources">Game resources.</param>
        /// <param name="mapFactory">Map factory.</param>
        public MapManager(ILogger<MapManager> logger, IOptions<WorldConfiguration> worldConfiguration, IGameResources gameResources, IMapFactory mapFactory)
        {
            _logger = logger;
            _worldConfiguration = worldConfiguration.Value;
            _gameResources = gameResources;
            _mapFactory = mapFactory;
            _maps = new ConcurrentDictionary<int, IMapInstance>();
        }

        /// <inheritdoc />
        public IMapInstance GetMap(int id) => _maps.TryGetValue(id, out IMapInstance map) ? map : null;

        /// <inheritdoc />
        public void Load()
        {
            string worldScriptPath = GameResourcesConstants.Paths.WorldScriptPath;
            var worldsPaths = new Dictionary<string, string>();

            using (var textFile = new TextFile(worldScriptPath))
            {
                foreach (var text in textFile.Texts)
                {
                    worldsPaths.Add(text.Key, text.Value.Replace('"', ' ').Trim());
                }
            }

            foreach (string mapDefineName in _worldConfiguration.Maps)
            {
                if (!worldsPaths.TryGetValue(mapDefineName, out string mapName))
                {
                    _logger.LogWarning(GameResourcesConstants.Errors.UnableLoadMapMessage, mapDefineName, $"map is not declared inside '{worldScriptPath}' file");
                    continue;
                }

                if (!_gameResources.Defines.TryGetValue(mapDefineName, out int mapId))
                {
                    _logger.LogWarning(GameResourcesConstants.Errors.UnableLoadMapMessage, mapDefineName, $"map has no define id inside '{GameResourcesConstants.Paths.DataSub0Path}/defineWorld.h' file");
                    continue;
                }

                if (_maps.ContainsKey(mapId))
                {
                    _logger.LogWarning(GameResourcesConstants.Errors.UnableLoadMapMessage, mapDefineName, $"another map with id '{mapId}' already exist.");
                    continue;
                }

                IMapInstance map = _mapFactory.Create(Path.Combine(GameResourcesConstants.Paths.MapsPath, mapName), mapName, mapId);

                _maps.Add(mapId, map);
            }

            _logger.LogInformation("-> {0} maps loaded.", _maps.Count);
        }
    }
}
