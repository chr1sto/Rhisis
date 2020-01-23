using Rhisis.Core.Common;
using Rhisis.Core.Structures.Game;
using Rhisis.Core.Structures.Game.Quests;
using Rhisis.World.Game.Behaviors;
using Rhisis.World.Game.Components;
using System.Collections.Generic;

namespace Rhisis.World.Game.Entities
{
    public class NpcEntity : WorldEntity, INpcEntity
    {
        /// <inheritdoc />
        public override WorldEntityType Type => WorldEntityType.Npc;

        /// <inheritdoc />
        public ItemContainerComponent[] Shop { get; set; }

        /// <inheritdoc />
        public NpcData Data { get; set; }

        /// <inheritdoc />
        public IEnumerable<IQuestScript> Quests { get; set; }

        /// <inheritdoc />
        public IBehavior Behavior { get; set; }

        /// <inheritdoc />
        public InteractionComponent Interaction { get; set; }

        /// <inheritdoc />
        public BattleComponent Battle { get; set; }

        /// <inheritdoc />
        public HealthComponent Health { get; set; }

        /// <inheritdoc />
        public AttributeComponent Attributes { get; set; }

        /// <inheritdoc />
        public MovableComponent Moves { get; set; }

        /// <inheritdoc />
        public FollowComponent Follow { get; set; }

        /// <inheritdoc />
        public TimerComponent Timers { get; set; }

        /// <summary>
        /// Creates a new <see cref="NpcEntity"/> instance.
        /// </summary>
        /// <param name="context"></param>
        public NpcEntity()
        {
            Object.Type = WorldObjectType.Mover;
            Timers = new TimerComponent();
            Interaction = new InteractionComponent();
            Battle = new BattleComponent();
            Health = new HealthComponent();
            Attributes = new AttributeComponent();
            Moves = new MovableComponent();
            Follow = new FollowComponent();
        }

        /// <inheritdoc />
        public override string ToString() => $"{Object.Name}";
    }
}
