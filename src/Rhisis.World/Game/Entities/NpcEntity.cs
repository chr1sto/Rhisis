using Rhisis.World.Game.Components;
using Rhisis.World.Game.Core;
using Rhisis.World.Game.Core.Interfaces;

namespace Rhisis.World.Game.Entities
{
    public class NpcEntity : Entity, INpcEntity
    {
        /// <inheritdoc />
        public override WorldEntityType Type => WorldEntityType.Npc;

        /// <inheritdoc />
        public ItemContainerComponent[] Shop { get; set; }

        /// <summary>
        /// Creates a new <see cref="NpcEntity"/> instance.
        /// </summary>
        /// <param name="context"></param>
        public NpcEntity(IContext context)
            : base(context)
        {
        }
    }
}
