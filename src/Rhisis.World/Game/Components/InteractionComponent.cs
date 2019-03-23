using Rhisis.World.Game.Core;

namespace Rhisis.World.Game.Components
{
    public class InteractionComponent
    {
        /// <summary>
        /// The TargetEntity to interact with.
        /// </summary>
        public IEntity TargetEntity { get; set; }
    }
}