using Rhisis.World.Core.Systems;
using Rhisis.World.Game.Core;
using Rhisis.World.Game.Core.Interfaces;
using Rhisis.World.Game.Entities;
using Rhisis.World.Packets;
using Rhisis.World.Systems.Events;
using System;
using System.Linq.Expressions;

namespace Rhisis.World.Systems
{
    [System]
    public class ChatSystem : NotifiableSystemBase
    {
        protected override Expression<Func<IEntity, bool>> Filter => x => x.Type == WorldEntityType.Player;

        public ChatSystem(IContext context)
            : base(context)
        {
        }

        public override void Execute(IEntity entity, EventArgs e)
        {
            var chatEvent = e as ChatEventArgs;

            if (string.IsNullOrEmpty(chatEvent.Message))
                return;
            
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
