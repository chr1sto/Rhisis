using Microsoft.Extensions.Logging;
using Rhisis.Core.Common;
using Rhisis.World.Game.Entities;
using Rhisis.Core.Data;
using Rhisis.World.Packets;

namespace Rhisis.World.Game.Chat
{
    [ChatCommand("/undying", AuthorityType.GameMaster)]
    [ChatCommand("/ud", AuthorityType.GameMaster)]
    public class UndyingChatCommand : IChatCommand
    {
        private readonly ILogger<UndyingChatCommand> _logger;
        private readonly IPlayerDataPacketFactory _playerDataPacketFactory;

        /// <summary>
        /// Creates a new <see cref="UndyingChatCommand"/> instance.
        /// </summary>
        /// <param name="logger">Logger.</param>
        /// <param name="playerDataPacketFactory">Player data packet factory system.</param>
        public UndyingChatCommand(ILogger<UndyingChatCommand> logger, IPlayerDataPacketFactory playerDataPacketFactory)
        {
            _logger = logger;
            _playerDataPacketFactory = playerDataPacketFactory;
        }

        /// <inheritdoc />
        public void Execute(IPlayerEntity player, object[] parameters)
        {
            if (!player.PlayerData.Mode.HasFlag(ModeType.MATCHLESS_MODE))
            {
                player.PlayerData.Mode |= ModeType.MATCHLESS_MODE;
                _playerDataPacketFactory.SendModifyMode(player);
                _logger.LogTrace($"Player '{player.Object.Name}' is now in undying mode.");
            }
            else {
                _logger.LogTrace($"Player '{player.Object.Name}' is already in undying mode.");
            }
        }
    }
}