using NLog;
using Rhisis.Core.Common;
using Rhisis.Core.Data;
using Rhisis.Core.Structures.Game;
using Rhisis.World.Game.Entities;
using Rhisis.World.Packets;
using Rhisis.World.Systems.Inventory;
using Rhisis.World.Systems.Inventory.EventArgs;
using System.Linq;

namespace Rhisis.World.Game.Chat
{
    public static class CreateItemChatCommand
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        [ChatCommand("/createitem", AuthorityType.GameMaster)]
        [ChatCommand("/ci", AuthorityType.GameMaster)]
        [ChatCommand("/item", AuthorityType.GameMaster)]
        public static void CreateItem(IPlayerEntity player, string[] parameters)
        {
            Logger.Debug("{0} want to create an item", player.Object.Name);

            if (parameters.Length <= 0)
            {
                Logger.Error("Chat: /createitem command must have at least one parameter.");
                return;
            }

            if (!player.Inventory.HasAvailableSlots())
            {
                WorldPacketFactory.SendDefinedText(player, DefineText.TID_GAME_LACKSPACE);
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

            var createItemEvent = new InventoryCreateItemEventArgs(itemId, quantity, player.PlayerData.Id);
            player.Context.NotifySystem<InventorySystem>(player, createItemEvent);
        }
    }
}