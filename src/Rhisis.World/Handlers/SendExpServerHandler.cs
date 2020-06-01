using Rhisis.Network.Packets;
using Rhisis.Network.Packets.World;
using Rhisis.World.Client;
using Sylver.HandlerInvoker.Attributes;
using Rhisis.Core.Common;
using Microsoft.Extensions.Logging;
using System;
using Rhisis.World.Systems.Experience;

namespace Rhisis.World.Handlers
{
    /// <summary>
    /// Handles every packets related to the exp server system.
    /// </summary>
    [Handler]
    public sealed class SendExpServerHandler
    {
        private readonly IExperienceSystem _expSystem;
        private readonly ILogger<SendExpServerHandler> _logger;

        /// <summary>
        /// Creates a new <see cref="SendExpServerHandler"/> instance.
        /// </summary>
        /// <param name="expSystem">Experience system.</param>
        public SendExpServerHandler(IExperienceSystem expSystem, ILogger<SendExpServerHandler> logger)
        {
            _expSystem = expSystem;
            _logger = logger;
        }

        /// <summary>
        /// Handles a Send Exp Server request.
        /// </summary>
        /// <param name="serverClient"></param>
        /// <param name="packet"></param>
        [HandlerAction(PacketType.SEND_TO_SERVER_EXP)]
        public void OnSendExpServerHandler(IWorldServerClient serverClient, ExperiencePacket packet)
        {
            if (serverClient.Player.PlayerData.Authority == AuthorityType.GameMaster || serverClient.Player.PlayerData.Authority == AuthorityType.Administrator)
            {
                _logger.LogTrace($"{serverClient.Player.Object.Name} gives himself {packet.Experience} experience point.");
                _expSystem.GiveExeperience(serverClient.Player, packet.Experience);
            } 
            else 
            {
                throw new ArgumentException($"{serverClient.Player.Object.Name} cannot send to himself exp server while beeing {serverClient.Player.PlayerData.Authority}.");
            }
        }
    }
}