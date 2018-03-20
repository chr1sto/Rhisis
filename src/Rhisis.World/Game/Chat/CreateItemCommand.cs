using System.Linq;
using Rhisis.Core.IO;
using Rhisis.Core.Structures.Game;
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

            if (!player.Inventory.HasAvailableSlots())
            {
                // TODO: send message to tell there is no available slots
                return;
            }

            var quantity = 1;

            if (!int.TryParse(parameters[0], out int itemId))
            {
                ItemData itemData = WorldServer.Items.Values.FirstOrDefault(x =>
                    string.Equals(x.Name, parameters[0], System.StringComparison.OrdinalIgnoreCase));

                itemId = itemData?.Id ?? -1;
            }

            if (parameters.Length >= 2)
                int.TryParse(parameters[1], out quantity);

            var createItemEvent = new InventoryCreateItemEventArgs(itemId, quantity, player.PlayerComponent.Id);
            player.Context.NotifySystem<InventorySystem>(player, createItemEvent);
        }
    }
}