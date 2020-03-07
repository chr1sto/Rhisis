using Microsoft.Extensions.Logging;
using Rhisis.Core.Common;
using Rhisis.World.Game.Entities;
using Rhisis.World.Systems.PlayerData;
using Rhisis.Core.Data;

namespace Rhisis.World.Game.Chat
{
    [ChatCommand("/ExpUpStop", AuthorityType.Administrator)]
    [ChatCommand("/es", AuthorityType.Administrator)]
    public class ExpUpStopChatCommand : IChatCommand
    {
        private readonly ILogger<ExpUpStopChatCommand> _logger;
        private readonly IPlayerDataSystem _playerDataSystem;

        /// <summary>
        /// Creates a new <see cref="ExpUpStopChatCommand"/> instance.
        /// </summary>
        /// <param name="logger">Logger.</param>
        /// <param name="playerDataSystem">Player data system.</param>
        public ExpUpStopChatCommand(ILogger<ExpUpStopChatCommand> logger, IPlayerDataSystem playerDataSystem)
        {
            _logger = logger;
            _playerDataSystem = playerDataSystem;
        }

        /// <inheritdoc />
        public void Execute(IPlayerEntity player, object[] parameters)
        {
            if (!player.PlayerData.Mode.HasFlag(ModeType.MODE_EXPUP_STOP))
            {
                player.PlayerData.Mode |= ModeType.MODE_EXPUP_STOP;
                _logger.LogTrace($"Player '{player.Object.Name}' is now in Exp Up Stop mode.");
            }
            else {
                player.PlayerData.Mode &= ~ ModeType.MODE_EXPUP_STOP;
                _logger.LogTrace($"Player '{player.Object.Name}' isn't in Exp Up Stop mode.");
            }
        }
    }
}