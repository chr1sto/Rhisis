using Rhisis.World.Game.Core;

namespace Rhisis.World.Systems.NpcShop
{
    public class NpcShopEventArgs : SystemEventArgs
    {
        /// <summary>
        /// Gets the NPC shop action type.
        /// </summary>
        public NpcShopActionType ActionType { get; }

        /// <summary>
        /// Creates a new <see cref="NpcShopEventArgs"/> instance.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="args"></param>
        public NpcShopEventArgs(NpcShopActionType type, params object[] args) 
            : base(args)
        {
            this.ActionType = type;
        }

        /// <summary>
        /// Check the arguments.
        /// </summary>
        /// <returns></returns>
        public override bool CheckArguments() => true;
    }
}
