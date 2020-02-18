using Microsoft.Extensions.Logging;
using Rhisis.Core.Common;
using Rhisis.World.Game.Entities;
using Rhisis.World.Systems.PlayerData;
using Rhisis.Core.Data;

namespace Rhisis.World.Game.Chat
{
    [ChatCommand("/noundying", AuthorityType.GameMaster)]
    [ChatCommand("/noud", AuthorityType.GameMaster)]
    public class NoUndyingChatCommand : IChatCommand
    {
        private readonly ILogger<NoUndyingChatCommand> _logger;
        private readonly IPlayerDataSystem _playerDataSystem;

        /// <summary>
        /// Creates a new <see cref="NoUndyingChatCommand"/> instance.
        /// </summary>
        /// <param name="logger">Logger.</param>
        /// <param name="playerDataSystem">Player data system.</param>
        public NoUndyingChatCommand(ILogger<NoUndyingChatCommand> logger, IPlayerDataSystem playerDataSystem)
        {
            _logger = logger;
            _playerDataSystem = playerDataSystem;
        }

        /// <inheritdoc />
        public void Execute(IPlayerEntity player, object[] parameters)
        {
            if (player.PlayerData.Mode == ModeType.MATCHLESS_MODE)
            {
                player.PlayerData.Mode = ModeType.NONE;
                _logger.LogTrace($"Player '{player.Object.Name}' is not anymore in undying mode.");
            }
            else {
                _logger.LogTrace($"Player '{player.Object.Name}' is currently not in undying mode.");
            }
        }
    }
}