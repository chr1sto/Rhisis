using Microsoft.Extensions.Logging;
using Rhisis.Core.Data;
using Rhisis.Core.DependencyInjection;
using Rhisis.Core.Structures;
using Rhisis.World.Game.Entities;
using Rhisis.World.Game.Maps;
using Rhisis.World.Packets;
using Rhisis.World.Systems.Visibility;

namespace Rhisis.World.Systems.Teleport
{
    [Injectable]
    public sealed class TeleportSystem : ITeleportSystem
    {
        private readonly ILogger<TeleportSystem> _logger;
        private readonly IMapManager _mapManager;
        private readonly IVisibilitySystem _visibilitySystem;
        private readonly IPlayerPacketFactory _playerPacketFactory;
        private readonly IWorldSpawnPacketFactory _worldSpawnPacketFactory;
        private readonly ITextPacketFactory _textPacketFactory;

        /// <summary>
        /// Creates a new <see cref="TeleportSystem"/> instance.
        /// </summary>
        /// <param name="logger">Logger.</param>
        /// <param name="mapManager">Map manager.</param>
        /// <param name="visibilitySystem">Visibility System.</param>
        /// <param name="playerPacketFactory">Player packet factory.</param>
        /// <param name="worldSpawnPacketFactory">World spawn packet factory.</param>
        /// <param name="textPacketFactory">Text packet factory.</param>
        public TeleportSystem(ILogger<TeleportSystem> logger, IMapManager mapManager, IVisibilitySystem visibilitySystem, IPlayerPacketFactory playerPacketFactory, IWorldSpawnPacketFactory worldSpawnPacketFactory, ITextPacketFactory textPacketFactory)
        {
            _logger = logger;
            _mapManager = mapManager;
            _visibilitySystem = visibilitySystem;
            _playerPacketFactory = playerPacketFactory;
            _worldSpawnPacketFactory = worldSpawnPacketFactory;
            _textPacketFactory = textPacketFactory;
        }

        /// <inheritdoc />
        public void Teleport(IPlayerEntity player, int mapId, float x, float? y, float z, float angle = 0)
        {
            if (player.Object.MapId != mapId)
            {
                IMapInstance destinationMap = _mapManager.GetMap(mapId);

                if (destinationMap == null)
                {
                    _logger.LogError($"Cannot find map with id '{mapId}'.");
                    _textPacketFactory.SendSnoop(player, $"Cannot find map with id '{mapId}'.");
                    return;
                }

                if (!destinationMap.ContainsPosition(new Vector3(x, 0, z)))
                {
                    _logger.LogError($"Cannot teleport. Destination position is out of map bounds.");
                    return;
                }

                _visibilitySystem.DespawnEntity(player);
                player.Object.Spawned = false;
                player.Object.CurrentMap = _mapManager.GetMap(destinationMap.Id);
                player.Object.MapId = destinationMap.Id;
                player.Object.LayerId = destinationMap.DefaultMapLayer.Id;

                // TODO: get map height at x/z position
                float positionY = y ?? 100;
                player.Object.Position = new Vector3(x, positionY, z);
                player.Object.MovingFlags = ObjectState.OBJSTA_STAND;
                player.Moves.DestinationPosition.Reset();
                player.Battle.Reset();
                player.Follow.Reset();

                _playerPacketFactory.SendPlayerReplace(player);
                _worldSpawnPacketFactory.SendPlayerSpawn(player);
                player.Object.Spawned = true;
            }
            else
            {
                if (!player.Object.CurrentMap.ContainsPosition(new Vector3(x, 0, z)))
                {
                    _logger.LogError($"Cannot teleport. Destination position is out of map bounds.");
                    return;
                }

                // TODO: get map height at x/z position
                float positionY = y ?? 100;
                player.Object.Position = new Vector3(x, positionY, z);
                player.Object.MovingFlags = ObjectState.OBJSTA_STAND;
                player.Moves.DestinationPosition.Reset();
                player.Battle.Reset();
                player.Follow.Reset();
            }

            _playerPacketFactory.SendPlayerTeleport(player);
        }
    }
}
