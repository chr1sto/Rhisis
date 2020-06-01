using Microsoft.Extensions.Logging;
using Rhisis.Core.Common;
using Rhisis.World.Game.Entities;
using Rhisis.Core.Data;
using Rhisis.World.Packets;

namespace Rhisis.World.Game.Chat
{
    [ChatCommand("/noinvisible", AuthorityType.GameMaster)]
    [ChatCommand("/noinv", AuthorityType.GameMaster)]
    public class NoInvisibleChatCommand : IChatCommand
    {
        private readonly ILogger<NoInvisibleChatCommand> _logger;
        private readonly IPlayerDataPacketFactory _playerDataPacketFactory;
        private readonly IMoverPacketFactory _moverPacketFactory;

        /// <summary>
        /// Creates a new <see cref="NoInvisibleChatCommand"/> instance.
        /// </summary>
        /// <param name="logger">Logger.</param>
        /// <param name="playerDataPacketFactory">Player data packet factory system.</param>
        /// <param name="moverPacketFactory">Mover packet factory system.</param>
        public NoInvisibleChatCommand(ILogger<NoInvisibleChatCommand> logger, IPlayerDataPacketFactory playerDataPacketFactory, IMoverPacketFactory moverPacketFactory)
        {
            _logger = logger;
            _playerDataPacketFactory = playerDataPacketFactory;
            _moverPacketFactory = moverPacketFactory;
        }

        /// <inheritdoc />
        public void Execute(IPlayerEntity player, object[] parameters)
        {
            if (player.PlayerData.Mode.HasFlag(ModeType.TRANSPARENT_MODE))
            {
                player.PlayerData.Mode &= ~ ModeType.TRANSPARENT_MODE;
                _playerDataPacketFactory.SendModifyMode(player);
                _moverPacketFactory.SendDestinationPosition(player);
                _moverPacketFactory.SendMoverPositionAngle(player, sendOwnPlayer : false);
                _logger.LogTrace($"Player '{player.Object.Name}' is not anymore invisible.");
            }
            else {
                _logger.LogTrace($"Player '{player.Object.Name}' is currently not invisible.");
            }
        }
    }
}