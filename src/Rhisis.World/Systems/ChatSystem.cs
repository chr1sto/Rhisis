using Rhisis.Core.Exceptions;
using Rhisis.World.Core.Systems;
using Rhisis.World.Game.Core;
using Rhisis.World.Game.Core.Interfaces;
using Rhisis.World.Game.Entities;
using Rhisis.World.Packets;
using Rhisis.World.Systems.Events;
using System;

namespace Rhisis.World.Systems
{
    [System]
    public class ChatSystem : NotifiableSystemBase
    {
        public ChatSystem(IContext context)
            : base(context)
        {
        }

        public override void Execute(IEntity entity, EventArgs e)
        {
            var chatEvent = e as ChatEventArgs;

            if (string.IsNullOrEmpty(chatEvent.Message))
                return;

            if (entity.Type != WorldEntityType.Player)
                throw new RhisisException($"The entity is not a player.");

            var player = entity as IPlayerEntity;

            if (chatEvent.Message.StartsWith("/"))
            {
                // command
            }
            else
            {
                WorldPacketFactory.SendChat(player, chatEvent.Message);
            }
        }
    }
}
