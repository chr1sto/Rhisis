using System;
using System.Collections.Generic;
using System.Text;
using Rhisis.Database.Structures;
using Rhisis.World.Systems.Inventory;

namespace Rhisis.World.Game.Components
{
    public sealed class TradeComponent
    {
        public enum TradeConfirm { Error, Ok }

        public enum TradeState { Item, Ok, Confirm }

        public int TargetId { get; set; }

        public ItemContainerComponent Items { get; private set; }

        /// <summary>
        /// Confirmation state
        /// </summary>
        public TradeConfirm Confirm { get; set; }

        /// <summary>
        /// Exchange state/progress
        /// </summary>
        public TradeState State { get; set; }

        public TradeComponent()
        {
            this.TargetId = 0;
            Confirm = TradeConfirm.Error;
            State = TradeState.Item;
            Items = new ItemContainerComponent(InventorySystem.MaxItems);
        }
    }
}
