using Microsoft.Extensions.Logging;
using Rhisis.Core.Common;
using Rhisis.World.Game.Entities;
using Rhisis.Core.Data;
using System;
using Rhisis.World.Packets;

namespace Rhisis.World.Game.Chat
{
    [ChatCommand("/freeze", AuthorityType.GameMaster)]
    [ChatCommand("/fr", AuthorityType.GameMaster)]
    public class FreezeChatCommand : IChatCommand
    {
        private readonly ILogger<FreezeChatCommand> _logger;
        private readonly IWorldServer _worldServer;
        private readonly IPlayerDataPacketFactory _playerDataPacketFactory;

        /// <summary>
        /// Creates a new <see cref="FreezeChatCommand"/> instance.
        /// </summary>
        /// <param name="logger">Logger.</param>
        /// <param name="worldServer">World server system.</param>
        /// <param name="playerDataPacketFactory">Player data packet factory system.</param>
        public FreezeChatCommand(ILogger<FreezeChatCommand> logger, IWorldServer worldServer, IPlayerDataPacketFactory playerDataPacketFactory)
        {
            _logger = logger;
            _worldServer = worldServer;
            _playerDataPacketFactory = playerDataPacketFactory;
        }

        /// <inheritdoc />
        public void Execute(IPlayerEntity player, object[] parameters)
        {
            if (parameters.Length == 1) 
            {
                IPlayerEntity playerToFreeze = _worldServer.GetPlayerEntity(parameters[0].ToString());
                if (!player.PlayerData.Mode.HasFlag(ModeType.DONMOVE_MODE))
                {
                    playerToFreeze.PlayerData.Mode |= ModeType.DONMOVE_MODE;
                    _playerDataPacketFactory.SendModifyMode(playerToFreeze);
                    _logger.LogTrace($"Player '{playerToFreeze.Object.Name}' is now freezed.");
                }
                else 
                {
                    _logger.LogTrace($"Player '{playerToFreeze.Object.Name}' is already freezed.");
                }
            }
            else
            {
                throw new ArgumentException("Too many or not enough arguments.");
            }
        }
    }
}