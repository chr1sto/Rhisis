using Microsoft.Extensions.Logging;
using Rhisis.Core.Resources;
using Rhisis.World.Game.Behaviors;
using Rhisis.World.Game.Entities;

namespace Rhisis.World.Game.Loaders
{
    public sealed class BehaviorLoader : IGameResourceLoader
    {
        private readonly ILogger<BehaviorLoader> _logger;

        /// <summary>
        /// Gets the monsters behavior.
        /// </summary>
        public BehaviorManager<IMonsterEntity> MonsterBehaviors { get; }

        /// <summary>
        /// Gets the npcs behaviors.
        /// </summary>
        public BehaviorManager<INpcEntity> NpcBehaviors { get; }

        /// <summary>
        /// Gets the players behaviors.
        /// </summary>
        public BehaviorManager<IPlayerEntity> PlayerBehaviors { get; }

        /// <summary>
        /// Creates a new <see cref="BehaviorLoader"/> instance.
        /// </summary>
        /// <param name="logger">Logger</param>
        public BehaviorLoader(ILogger<BehaviorLoader> logger)
        {
            this._logger = logger;
            this.MonsterBehaviors = new BehaviorManager<IMonsterEntity>(BehaviorType.Monster);
            this.NpcBehaviors = new BehaviorManager<INpcEntity>(BehaviorType.Npc);
            this.PlayerBehaviors = new BehaviorManager<IPlayerEntity>(BehaviorType.Player);
        }

        /// <inheritdoc />
        public void Load()
        {
            this.MonsterBehaviors.Load();
            this.NpcBehaviors.Load();
            this.PlayerBehaviors.Load();

            this._logger.LogInformation("-> {0} behaviors loaded.", MonsterBehaviors.Count + NpcBehaviors.Count + PlayerBehaviors.Count);
        }

        /// <inheritdoc />
        public void Dispose()
        {
            // Nothing to dispose.
        }

    }
}
