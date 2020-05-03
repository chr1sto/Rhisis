using Rhisis.Network.Packets;
using Rhisis.Network.Packets.World;
using Rhisis.World.Client;
using Rhisis.World.Systems.NpcShop;
using Sylver.HandlerInvoker.Attributes;

namespace Rhisis.World.Handlers
{
    [Handler]
    public sealed class NpcShopHandler
    {
        private readonly INpcShopSystem _npcShopSystem;

        /// <summary>
        /// Creates a new <see cref="NpcShopHandler"/>.
        /// </summary>
        /// <param name="npcShopSystem"></param>
        public NpcShopHandler(INpcShopSystem npcShopSystem)
        {
            _npcShopSystem = npcShopSystem;
        }

        /// <summary>
        /// Player requests to open a NPC shop.
        /// </summary>
        /// <param name="serverClient">Client.</param>
        /// <param name="packet">Incoming packet.</param>
        [HandlerAction(PacketType.OPENSHOPWND)]
        public void OnOpenShopWindow(IWorldServerClient serverClient, OpenShopWindowPacket packet)
        {
            _npcShopSystem.OpenShop(serverClient.Player, packet.ObjectId);
        }

        /// <summary>
        /// Player closes a opened NPC shop.
        /// </summary>
        /// <param name="serverClient">Client.</param>
        /// <param name="packet">Incoming packet.</param>
        [HandlerAction(PacketType.CLOSESHOPWND)]
        public void OnCloseShopWindow(IWorldServerClient serverClient)
        {
            _npcShopSystem.CloseShop(serverClient.Player);
        }

        /// <summary>
        /// Player buys an item from a NPC shop.
        /// </summary>
        /// <param name="serverClient">Client.</param>
        /// <param name="packet">Incoming packet.</param>
        [HandlerAction(PacketType.BUYITEM)]
        public void OnBuyItem(IWorldServerClient serverClient, BuyItemPacket packet)
        {
            var npcShopItem = new NpcShopItemInfo
            {
                ItemId = packet.ItemId,
                Slot = packet.Slot,
                Tab = packet.Tab
            };

            _npcShopSystem.BuyItem(serverClient.Player, npcShopItem, packet.Quantity);
        }

        /// <summary>
        /// Player sells an item at a NPC shop.
        /// </summary>
        /// <param name="serverClient">Client.</param>
        /// <param name="packet">Incoming packet.</param>
        [HandlerAction(PacketType.SELLITEM)]
        public void OnSellItem(IWorldServerClient serverClient, SellItemPacket packet)
        {
            _npcShopSystem.SellItem(serverClient.Player, packet.ItemUniqueId, packet.Quantity);
        }
    }
}
