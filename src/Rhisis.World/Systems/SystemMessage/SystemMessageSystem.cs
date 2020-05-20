using Rhisis.Core.DependencyInjection;
using Rhisis.Core.Extensions;
using Rhisis.World.Game.Entities;
using Rhisis.World.Packets;
using System;

namespace Rhisis.World.Systems.SystemMessage
{
    [Injectable]
    public sealed class SystemMessageSystem : ISystemMessageSystem
    {
        private readonly ISystemMessagePacketFactory _systemMessagePacketFactory;

        /// <summary>
        /// Creates a new <see cref="SystemMessageSystem"/> instance.
        /// </summary>
        /// <param name="systemMessagePacketFactory">system message packet factory.</param>
        public SystemMessageSystem(ISystemMessagePacketFactory systemMessagePacketFactory)
        {
            _systemMessagePacketFactory = systemMessagePacketFactory;
        }

        /// <inheritdoc />
        public void SystemMessage(IPlayerEntity player, string sysMessage)
        {
            if (string.IsNullOrEmpty(sysMessage))
            {
                throw new ArgumentNullException(nameof(sysMessage), $"Cannot send an empty message for all the worldserver's players.");
            }

            _systemMessagePacketFactory.SendSystemMessage(player, sysMessage.TakeCharacters(511));
        }
    }
}
