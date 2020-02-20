using Microsoft.Extensions.Logging;
using Rhisis.Core.Common;
using Rhisis.World.Game.Entities;
using Rhisis.Core.Data;
using Rhisis.World.Packets;

namespace Rhisis.World.Game.Chat
{
    [ChatCommand("/onekill", AuthorityType.GameMaster)]
    [ChatCommand("/ok", AuthorityType.GameMaster)]
    public class OneKillChatCommand : IChatCommand
    {
        private readonly ILogger<OneKillChatCommand> _logger;
        private readonly IPlayerDataPacketFactory _playerDataPacketFactory;

        /// <summary>
        /// Creates a new <see cref="OneKillChatCommand"/> instance.
        /// </summary>
        /// <param name="logger">Logger.</param>
        /// <param name="playerDataPacketFactory">Player data packet factory system.</param>
        public OneKillChatCommand(ILogger<OneKillChatCommand> logger, IPlayerDataPacketFactory playerDataPacketFactory)
        {
            _logger = logger;
            _playerDataPacketFactory = playerDataPacketFactory;
        }

        /// <inheritdoc />
        public void Execute(IPlayerEntity player, object[] parameters)
        {
            if (!player.PlayerData.Mode.HasFlag(ModeType.ONEKILL_MODE))
            {
                player.PlayerData.Mode |= ModeType.ONEKILL_MODE;
                _playerDataPacketFactory.SendModifyMode(player);
                _logger.LogTrace($"Player '{player.Object.Name}' is now in OneKill mode.");
            }
            else {
                _logger.LogTrace($"Player '{player.Object.Name}' is already in OneKill mode.");
            }
        }
    }
}