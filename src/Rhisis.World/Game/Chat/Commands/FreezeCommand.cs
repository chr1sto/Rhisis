using Microsoft.Extensions.Logging;
using Rhisis.Core.Common;
using Rhisis.World.Game.Entities;
using Rhisis.World.Systems.PlayerData;
using Rhisis.Core.Data;
using System;

namespace Rhisis.World.Game.Chat
{
    [ChatCommand("/freeze", AuthorityType.GameMaster)]
    [ChatCommand("/fr", AuthorityType.GameMaster)]
    public class FreezeChatCommand : IChatCommand
    {
        private readonly ILogger<FreezeChatCommand> _logger;
        private readonly IPlayerDataSystem _playerDataSystem;
        private readonly IWorldServer _worldServer;

        /// <summary>
        /// Creates a new <see cref="FreezeChatCommand"/> instance.
        /// </summary>
        /// <param name="logger">Logger.</param>
        /// <param name="playerDataSystem">Player data system.</param>
        /// <param name="worldServer">World server system.</param>
        public FreezeChatCommand(ILogger<FreezeChatCommand> logger, IPlayerDataSystem playerDataSystem, IWorldServer worldServer)
        {
            _logger = logger;
            _playerDataSystem = playerDataSystem;
            _worldServer = worldServer;
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