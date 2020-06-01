using Rhisis.Core.Structures.Game;
using Rhisis.Core.Structures.Game.Quests;
using Rhisis.World.Game.Components;
using System.Collections.Generic;

namespace Rhisis.World.Game.Entities
{
    public interface INpcEntity : ILivingEntity, IWorldEntity
    {
        /// <summary>
        /// Gets or sets the npc's shop item containers.
        /// </summary>
        /// <remarks>
        /// One item container represents one shop tab.
        /// </remarks>
        ItemContainerComponent[] Shop { get; set; }

        /// <summary>
        /// Gets the NPC data.
        /// </summary>
        NpcData NpcData { get; }

        /// <summary>
        /// Gets the NPC quests.
        /// </summary>
        IEnumerable<IQuestScript> Quests { get; }
    }
}
