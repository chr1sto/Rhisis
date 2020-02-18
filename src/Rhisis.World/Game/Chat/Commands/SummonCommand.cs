using Microsoft.Extensions.Logging;
using Rhisis.Core.Common;
using Rhisis.World.Game.Entities;
using Rhisis.World.Systems.Teleport;
using System;

namespace Rhisis.World.Game.Chat
{
    [ChatCommand("/summon", AuthorityType.GameMaster)]
    [ChatCommand("/su", AuthorityType.GameMaster)]
    public class SummonCommand : IChatCommand
    {
        private readonly ITeleportSystem _teleportSystem;
        private readonly IWorldServer _worldServer;
        private readonly ILogger<SummonCommand> _logger;

        /// <summary>
        /// Creates a new <see cref="SummonCommand"/> instance.
        /// </summary>
        /// <param name="logger">logger system.</param>
        /// <param name="teleportSystem">Teleport system.</param>
        /// <param name="worldServer">World server.</param>
        public SummonCommand(ILogger<SummonCommand> logger, ITeleportSystem teleportSystem, IWorldServer worldServer)
        {
            _logger = logger;
            _teleportSystem = teleportSystem;
            _worldServer = worldServer;
        }

        /// <inheritdoc />
        public void Execute(IPlayerEntity player, object[] parameters)
        {
            if (parameters.Length == 1) 
            {
                IPlayerEntity playerToSummon = _worldServer.GetPlayerEntity(parameters[0].ToString());

                if (playerToSummon == null)
                {
                    throw new ArgumentException($"The player doesn't exist or is not connected.", nameof(parameters));
                }
                
                _teleportSystem.Teleport(playerToSummon, player.Object.MapId, player.Object.Position.X, player.Object.Position.Y, player.Object.Position.Z, player.Object.Angle);
                _logger.LogTrace($"{playerToSummon.Object.Name} is summoned by {player.Object.Name}.");
            }
            else 
            {
                throw new ArgumentException("Too many or not enough arguments.");
            }
        }
    }
}