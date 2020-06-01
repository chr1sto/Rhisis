using System;
using Microsoft.Extensions.Logging;
using Rhisis.Core.Common;
using Rhisis.World.Game.Entities;
using Rhisis.World.Systems.SystemMessage;

namespace Rhisis.World.Game.Chat
{
    [ChatCommand("/system", AuthorityType.GameMaster)]
    [ChatCommand("/sys", AuthorityType.GameMaster)]
    public class SystemChatCommand : IChatCommand
    {
        private readonly ILogger<SystemChatCommand> _logger;
        private readonly ISystemMessageSystem _sysMessageSystem;

        /// <summary>
        /// Creates a new <see cref="SystemChatCommand"/> instance.
        /// </summary>
        /// <param name="logger">Logger.</param>
        public SystemChatCommand(ILogger<SystemChatCommand> logger, ISystemMessageSystem sysMessageSystem)
        {
            _logger = logger;
            _sysMessageSystem = sysMessageSystem;
        }

        /// <inheritdoc />
        public void Execute(IPlayerEntity player, object[] parameters)
        {
            if (parameters.Length < 1)
            {
                throw new ArgumentException("You must have at least one parameter to send a message.");
            }

            string sysMsg = $"[{player.Object.Name}] " + string.Join(" ", parameters);
            _sysMessageSystem.SystemMessage(player, sysMsg);
            _logger.LogTrace($"Player '{player.Object.Name}' sent the following system message : '{sysMsg}'.");
        }
    }
}