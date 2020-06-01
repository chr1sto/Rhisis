using Microsoft.Extensions.Logging;
using Rhisis.Core.Common;
using Rhisis.World.Game.Entities;
using Rhisis.Core.Data;
using Rhisis.World.Packets;

namespace Rhisis.World.Game.Chat
{
    [ChatCommand("/noonekill", AuthorityType.GameMaster)]
    [ChatCommand("/nook", AuthorityType.GameMaster)]
    public class NoOneKillChatCommand : IChatCommand
    {
        private readonly ILogger<NoOneKillChatCommand> _logger;
        private readonly IPlayerDataPacketFactory _playerDataPacketFactory;

        /// <summary>
        /// Creates a new <see cref="NoOneKillChatCommand"/> instance.
        /// </summary>
        /// <param name="logger">Logger.</param>
        /// <param name="playerDataPacketFactory">Player data packet factory system.</param>
        public NoOneKillChatCommand(ILogger<NoOneKillChatCommand> logger, IPlayerDataPacketFactory playerDataPacketFactory)
        {
            _logger = logger;
            _playerDataPacketFactory = playerDataPacketFactory;
        }

        /// <inheritdoc />
        public void Execute(IPlayerEntity player, object[] parameters)
        {
            if (player.PlayerData.Mode.HasFlag(ModeType.ONEKILL_MODE))
            {
                player.PlayerData.Mode &= ~ ModeType.ONEKILL_MODE;
                _playerDataPacketFactory.SendModifyMode(player);
                _logger.LogTrace($"Player '{player.Object.Name}' is not anymore in OneKill mode.");
            }
            else {
                _logger.LogTrace($"Player '{player.Object.Name}' is currently not in OneKill mode.");
            }
        }
    }
}