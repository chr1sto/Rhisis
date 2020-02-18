using Microsoft.Extensions.Logging;
using Rhisis.Core.Common;
using Rhisis.World.Game.Entities;
using Rhisis.World.Systems.PlayerData;
using System;
using Rhisis.Core.Data;

namespace Rhisis.World.Game.Chat
{
    [ChatCommand("/onekill", AuthorityType.Administrator)]
    public class OneKillChatCommand : IChatCommand
    {
        private readonly ILogger<OneKillChatCommand> _logger;
        private readonly IPlayerDataSystem _playerDataSystem;

        /// <summary>
        /// Creates a new <see cref="OneKillChatCommand"/> instance.
        /// </summary>
        /// <param name="logger">Logger.</param>
        /// <param name="playerDataSystem">Player data system.</param>
        public OneKillChatCommand(ILogger<OneKillChatCommand> logger, IPlayerDataSystem playerDataSystem)
        {
            _logger = logger;
            _playerDataSystem = playerDataSystem;
        }

        /// <inheritdoc />
        public void Execute(IPlayerEntity player, object[] parameters)
        {
            if (player.PlayerData.Mode != ModeType.ONEKILL_MODE)
            {
                player.PlayerData.Mode = ModeType.ONEKILL_MODE;
                _logger.LogTrace($"Player '{player.Object.Name}' is now in OneKill mode.");
            }
            else {
                _logger.LogTrace($"Player '{player.Object.Name}' is already in OneKill mode.");
            }
        }
    }
}