using Microsoft.Extensions.Logging;
using Rhisis.Core.Common;
using Rhisis.World.Game.Entities;
using Rhisis.World.Systems.PlayerData;
using System;

namespace Rhisis.World.Game.Chat
{
    [ChatCommand("/getgold", AuthorityType.Administrator)]
    [ChatCommand("/gg", AuthorityType.Administrator)]
    public class GetGoldChatCommand : IChatCommand
    {
        private readonly ILogger<GetGoldChatCommand> _logger;
        private readonly IPlayerDataSystem _playerDataSystem;

        /// <summary>
        /// Creates a new <see cref="GetGoldChatCommand"/> instance.
        /// </summary>
        /// <param name="logger">Logger.</param>
        /// <param name="playerDataSystem">Player data system.</param>
        public GetGoldChatCommand(ILogger<GetGoldChatCommand> logger, IPlayerDataSystem playerDataSystem)
        {
            _logger = logger;
            _playerDataSystem = playerDataSystem;
        }

        /// <inheritdoc />
        public void Execute(IPlayerEntity player, object[] parameters)
        {
            if (parameters.Length == 1)
            {
                int gold = Convert.ToInt32(parameters[0]);

                if (!_playerDataSystem.IncreaseGold(player, gold))
                {
                    _logger.LogTrace($"Failed to create {gold} for player '{player.Object.Name}'.");
                }
                else
                {
                    _logger.LogTrace($"Player '{player.Object.Name}' created {gold} gold.");
                }
            }
        }
    }
}