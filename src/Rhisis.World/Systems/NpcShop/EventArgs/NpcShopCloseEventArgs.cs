using Rhisis.World.Game.Core.Systems;

namespace Rhisis.World.Systems.NpcShop.EventArgs
{
    internal sealed class NpcShopCloseEventArgs : SystemEventArgs
    {
        /// <inheritdoc />
        public override bool GetCheckArguments()
        {
            return true;
        }
    }
}
