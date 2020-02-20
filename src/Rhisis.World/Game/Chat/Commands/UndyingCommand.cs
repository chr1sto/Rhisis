using Microsoft.Extensions.Logging;
using Rhisis.Core.Common;
using Rhisis.World.Game.Entities;
using Rhisis.World.Systems.PlayerData;
using Rhisis.Core.Data;

namespace Rhisis.World.Game.Chat
{
    [ChatCommand("/undying", AuthorityType.GameMaster)]
    [ChatCommand("/ud", AuthorityType.GameMaster)]
    public class UndyingChatCommand : IChatCommand
    {
        private readonly ILogger<UndyingChatCommand> _logger;
        private readonly IPlayerDataSystem _playerDataSystem;

        /// <summary>
        /// Creates a new <see cref="UndyingChatCommand"/> instance.
        /// </summary>
        /// <param name="logger">Logger.</param>
        /// <param name="playerDataSystem">Player data system.</param>
        public UndyingChatCommand(ILogger<UndyingChatCommand> logger, IPlayerDataSystem playerDataSystem)
        {
            _logger = logger;
            _playerDataSystem = playerDataSystem;
        }

        /// <inheritdoc />
        public void Execute(IPlayerEntity player, object[] parameters)
        {
            if (!player.PlayerData.Mode.HasFlag(ModeType.MATCHLESS_MODE))
            {
                player.PlayerData.Mode |= ModeType.MATCHLESS_MODE;
                _logger.LogTrace($"Player '{player.Object.Name}' is now in undying mode.");
            }
            else {
                _logger.LogTrace($"Player '{player.Object.Name}' is already in undying mode.");
            }
        }
    }
}