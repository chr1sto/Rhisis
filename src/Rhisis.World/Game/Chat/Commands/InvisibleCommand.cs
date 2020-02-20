using Microsoft.Extensions.Logging;
using Rhisis.Core.Common;
using Rhisis.World.Game.Entities;
using Rhisis.Core.Data;
using Rhisis.World.Packets;

namespace Rhisis.World.Game.Chat
{
    [ChatCommand("/invisible", AuthorityType.GameMaster)]
    [ChatCommand("/inv", AuthorityType.GameMaster)]
    public class InvisibleChatCommand : IChatCommand
    {
        private readonly ILogger<InvisibleChatCommand> _logger;
        private readonly IPlayerDataPacketFactory _playerDataPacketFactory;

        /// <summary>
        /// Creates a new <see cref="InvisibleChatCommand"/> instance.
        /// </summary>
        /// <param name="logger">Logger.</param>
        /// <param name="playerDataPacketFactory">Player data packet factory system.</param>
        public InvisibleChatCommand(ILogger<InvisibleChatCommand> logger, IPlayerDataPacketFactory playerDataPacketFactory)
        {
            _logger = logger;
            _playerDataPacketFactory = playerDataPacketFactory;
        }

        /// <inheritdoc />
        public void Execute(IPlayerEntity player, object[] parameters)
        {
            if (!player.PlayerData.Mode.HasFlag(ModeType.TRANSPARENT_MODE))
            {
                player.PlayerData.Mode |= ModeType.TRANSPARENT_MODE;
                _playerDataPacketFactory.SendModifyMode(player);
                _logger.LogTrace($"Player '{player.Object.Name}' is now invisible.");
            }
            else {
                _logger.LogTrace($"Player '{player.Object.Name}' is already invisible.");
            }
        }
    }
}