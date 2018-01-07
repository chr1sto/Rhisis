using System.Collections.Generic;
using System.Text;
using Rhisis.Core.IO;
using Rhisis.World.Game.Entities;

namespace Rhisis.World.Game.Chat
{
    public static class CreateItemChatCommand
    {
        [ChatCommand("/createitem")]
        [ChatCommand("/ci")]
        [ChatCommand("/item")]
        public static void CreateItem(IPlayerEntity player, string[] parameters)
        {
            Logger.Debug("{0} want to create an item", player.ObjectComponent.Name);
        }
    }
}