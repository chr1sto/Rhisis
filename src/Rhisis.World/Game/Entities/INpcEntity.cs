using Rhisis.World.Game.Components;
using Rhisis.World.Game.Core.Interfaces;

namespace Rhisis.World.Game.Entities
{
    public interface INpcEntity : IEntity
    {
        /// <summary>
        /// Gets or sets the npc's shop item containers.
        /// </summary>
        /// <remarks>
        /// One item container represents one shop tab.
        /// </remarks>
        ItemContainerComponent[] Shop { get; set; }
    }
}
