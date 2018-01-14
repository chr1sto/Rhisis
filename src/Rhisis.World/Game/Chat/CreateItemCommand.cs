using Rhisis.Core.IO;
using Rhisis.World.Game.Entities;
using Rhisis.World.Systems;
using Rhisis.World.Systems.Events.Inventory;

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

            if (parameters.Length <= 0)
            {
                Logger.Error("Chat: /createitem command must have at least one parameter.");
                return;
            }

            if (!player.InventoryComponent.HasAvailableSlots())
            {
                // TODO: send message to tell there is no available slots
                return;
            }

            var quantity = 1;

            if (!int.TryParse(parameters[0], out int itemId))
                return;

            if (parameters.Length >= 2)
                int.TryParse(parameters[1], out quantity);

            var createItemEvent = new InventoryCreateItemEventArgs(itemId, quantity, player.PlayerComponent.Id);
            player.Context.NotifySystem<InventorySystem>(player, createItemEvent);
        }
    }
}