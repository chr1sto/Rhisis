using System;
using System.Collections.Generic;
using System.Text;
using Rhisis.Database.Structures;
using Rhisis.World.Game.Core;

namespace Rhisis.World.Systems.Inventory.EventArgs
{
    internal sealed class InventoryMoveEventArgs : SystemEventArgs
    {
        public byte SourceSlot { get; }

        public byte DestinationSlot { get; }

        public InventoryMoveEventArgs(byte sourceSlot, byte destinationSlot)
        {
            SourceSlot = sourceSlot;
            DestinationSlot = destinationSlot;
        }

        /// <inheritdoc />
        public override bool CheckArguments() => SourceSlot < InventorySystem.EquipOffset &&
                                                 DestinationSlot < InventorySystem.EquipOffset;
    }
}
