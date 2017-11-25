using Rhisis.Core.Exceptions;
using Rhisis.World.Core;
using Rhisis.World.Core.Components;
using Rhisis.World.Core.Entities;
using Rhisis.World.Core.Systems;
using Rhisis.World.Packets;
using Rhisis.World.Systems.Events;
using System;

namespace Rhisis.World.Systems
{
    [System]
    public class ChatSystem : ReactiveSystemBase
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

            var objectComponent = entity.GetComponent<ObjectComponent>();
            var playerComponent = entity.GetComponent<PlayerComponent>();

            if (objectComponent == null)
                throw new RhisisException($"The player doesn't have any {nameof(ObjectComponent)} attached.");

            if (playerComponent == null)
                throw new RhisisException($"The player doesn't have any {nameof(PlayerComponent)} attached.");

            if (chatEvent.Message.StartsWith("/"))
            {
                // command
            }
            else
            {
                WorldPacketFactory.SendChat(playerComponent.Connection, entity, chatEvent.Message);
            }
        }
    }
}
