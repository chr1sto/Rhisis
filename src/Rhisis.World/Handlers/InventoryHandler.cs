using Microsoft.Extensions.Logging;
using Rhisis.Network.Packets;
using Rhisis.Network.Packets.World;
using Rhisis.World.Client;
using Rhisis.World.Systems.Inventory;
using Rhisis.World.Systems.PlayerData;
using Sylver.HandlerInvoker.Attributes;

namespace Rhisis.World.Handlers
{
    /// <summary>
    /// Handles all inventory packets.
    /// </summary>
    [Handler]
    public class InventoryHandler
    {
        private readonly ILogger<InventoryHandler> _logger;
        private readonly IInventorySystem _inventorySystem;
        private readonly IPlayerDataSystem _playerDataSystem;

        /// <summary>
        /// Creates a new <see cref="InventoryHandler"/> instance.
        /// </summary>
        /// <param name="logger">Logger.</param>
        /// <param name="inventorySystem">Inventory System.</param>
        /// <param name="playerDataSystem">Player data system.</param>
        public InventoryHandler(ILogger<InventoryHandler> logger, IInventorySystem inventorySystem, IPlayerDataSystem playerDataSystem)
        {
            _logger = logger;
            _inventorySystem = inventorySystem;
            _playerDataSystem = playerDataSystem;
        }

        /// <summary>
        /// Handles the move item request.
        /// </summary>
        /// <param name="serverClient"></param>
        /// <param name="packet"></param>
        [HandlerAction(PacketType.MOVEITEM)]
        public void OnMoveItem(IWorldServerClient serverClient, MoveItemPacket packet)
        {
            _inventorySystem.MoveItem(serverClient.Player, packet.SourceSlot, packet.DestinationSlot);
        }

        /// <summary>
        /// Handles the equip/unequip request.
        /// </summary>
        /// <param name="serverClient"></param>
        /// <param name="packet"></param>
        [HandlerAction(PacketType.DOEQUIP)]
        public void OnDoEquip(IWorldServerClient serverClient, EquipItemPacket packet)
        {
            _inventorySystem.EquipItem(serverClient.Player, packet.UniqueId, packet.Part);
            _playerDataSystem.CalculateDefense(serverClient.Player);
        }

        /// <summary>
        /// Handles the drop item request.
        /// </summary>
        /// <param name="serverClient"></param>
        /// <param name="packet"></param>
        [HandlerAction(PacketType.DROPITEM)]
        public void OnDropItem(IWorldServerClient serverClient, DropItemPacket packet)
        {
            _inventorySystem.DropItem(serverClient.Player, packet.ItemUniqueId, packet.ItemQuantity);
        }

        /// <summary>
        /// Handles the delete item request.
        /// </summary>
        /// <param name="serverClient"></param>
        /// <param name="packet"></param>
        [HandlerAction(PacketType.REMOVEINVENITEM)]
        public void OnDeleteItem(IWorldServerClient serverClient, RemoveInventoryItemPacket packet)
        {
            _inventorySystem.DeleteItem(serverClient.Player, packet.ItemUniqueId, packet.ItemQuantity);
        }

        /// <summary>
        /// Handles the use item request.
        /// </summary>
        /// <param name="serverClient"></param>
        /// <param name="packet"></param>
        [HandlerAction(PacketType.DOUSEITEM)]
        public void OnUseItem(IWorldServerClient serverClient, DoUseItemPacket packet)
        {
            if (!string.IsNullOrWhiteSpace(serverClient.Player.PlayerData.CurrentShopName))
            {
                _logger.LogTrace($"Player {serverClient.Player} tried to use an item while visiting a NPC shop.");
                return;
            }

            _inventorySystem.UseItem(serverClient.Player, packet.UniqueItemId, packet.Part);
            _playerDataSystem.CalculateDefense(serverClient.Player);
        }
    }
}
