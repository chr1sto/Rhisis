using System;
using System.Collections.Generic;
using System.Text;
using Rhisis.Database.Structures;

namespace Rhisis.World.Game.Components
{
    public sealed class TradeComponent
    {
        public int TargetId { get; set; }

        public TradeComponent(Character character)
        {
            this.TargetId = 0;
        }

        public TradeComponent()
        {
        }
    }
}
