using Microsoft.Extensions.Logging;
using Rhisis.Core.Common;
using Rhisis.World.Game.Entities;
using Rhisis.World.Systems.PlayerData;
using System;
using Rhisis.Core.Data;

namespace Rhisis.World.Game.Chat
{
    [ChatCommand("/noonekill", AuthorityType.Administrator)]
    public class NoOneKillChatCommand : IChatCommand
    {
        private readonly ILogger<NoOneKillChatCommand> _logger;
        private readonly IPlayerDataSystem _playerDataSystem;

        /// <summary>
        /// Creates a new <see cref="NoOneKillChatCommand"/> instance.
        /// </summary>
        /// <param name="logger">Logger.</param>
        /// <param name="playerDataSystem">Player data system.</param>
        public NoOneKillChatCommand(ILogger<NoOneKillChatCommand> logger, IPlayerDataSystem playerDataSystem)
        {
            _logger = logger;
            _playerDataSystem = playerDataSystem;
        }

        /// <inheritdoc />
        public void Execute(IPlayerEntity player, object[] parameters)
        {
            if (player.PlayerData.Mode == ModeType.ONEKILL_MODE)
            {
                player.PlayerData.Mode = ModeType.NONE;
                _logger.LogTrace($"Player '{player.Object.Name}' is not anymore in OneKill mode.");
            }
            else {
                _logger.LogTrace($"Player '{player.Object.Name}' is currently not in OneKill mode.");
            }
        }
    }
}