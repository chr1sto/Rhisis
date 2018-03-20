namespace Rhisis.World.Systems.NpcShop
{
    public class NpcShopOpenEventArgs : NpcShopEventArgs
    {
        /// <summary>
        /// Gets the NPC Object id.
        /// </summary>
        public int NpcObjectId { get; }

        /// <summary>
        /// Creates a new <see cref="NpcShopOpenEventArgs"/> instance.
        /// </summary>
        /// <param name="npcObjectId"></param>
        public NpcShopOpenEventArgs(int npcObjectId) 
            : base(NpcShopActionType.Open, npcObjectId)
        {
            this.NpcObjectId = this.GetArgument<int>(0);
        }

        /// <inheritdoc />
        public override bool CheckArguments() => this.NpcObjectId > 0;
    }
}
