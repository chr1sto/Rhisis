using System;
using System.Collections.Generic;
using System.Text;
using Rhisis.Core.Structures.Game;
using Rhisis.World.Game.Core;

namespace Rhisis.World.Systems.NpcShop.EventArgs
{
    internal sealed class NpcShopCloseEventArgs : SystemEventArgs
    {
        /// <inheritdoc />
        public override bool CheckArguments() => true;
    }
}
