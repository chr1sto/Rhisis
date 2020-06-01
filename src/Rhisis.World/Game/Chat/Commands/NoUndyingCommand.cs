using Microsoft.Extensions.Logging;
using Rhisis.Core.Common;
using Rhisis.World.Game.Entities;
using Rhisis.Core.Data;
using Rhisis.World.Packets;

namespace Rhisis.World.Game.Chat
{
    [ChatCommand("/noundying", AuthorityType.GameMaster)]
    [ChatCommand("/noud", AuthorityType.GameMaster)]
    public class NoUndyingChatCommand : IChatCommand
    {
        private readonly ILogger<NoUndyingChatCommand> _logger;
        private readonly IPlayerDataPacketFactory _playerDataPacketFactory;

        /// <summary>
        /// Creates a new <see cref="NoUndyingChatCommand"/> instance.
        /// </summary>
        /// <param name="logger">Logger.</param>
        /// <param name="playerDataPacketFactory">Player data packey factory system.</param>
        public NoUndyingChatCommand(ILogger<NoUndyingChatCommand> logger, IPlayerDataPacketFactory playerDataPacketFactory)
        {
            _logger = logger;
            _playerDataPacketFactory = playerDataPacketFactory;
        }

        /// <inheritdoc />
        public void Execute(IPlayerEntity player, object[] parameters)
        {
            if (player.PlayerData.Mode.HasFlag(ModeType.MATCHLESS_MODE))
            {
                player.PlayerData.Mode &= ~ ModeType.MATCHLESS_MODE;
                _playerDataPacketFactory.SendModifyMode(player);
                _logger.LogTrace($"Player '{player.Object.Name}' is not anymore in undying mode.");
            }
            else {
                _logger.LogTrace($"Player '{player.Object.Name}' is currently not in undying mode.");
            }
        }
    }
}