using Microsoft.Extensions.Logging;
using Rhisis.Core.DependencyInjection;
using Rhisis.World.Game.Core;
using Rhisis.World.Game.Core.Systems;
using Rhisis.World.Game.Entities;

namespace Rhisis.World.Systems.Teleport
{
    /// <summary>
    /// Teleport system used to teleport player to a target map at a given position.
    /// </summary>
    [System(SystemType.Notifiable)]
    public sealed class TeleportSystem : ISystem
    {
        private readonly ILogger<TeleportSystem> _logger;

        /// <summary>
        /// Creates a new <see cref="TeleportSystem"/> instance.
        /// </summary>
        public TeleportSystem()
        {
            this._logger = DependencyContainer.Instance.Resolve<ILogger<TeleportSystem>>();
        }

        /// <inheritdoc />
        public WorldEntityType Type => WorldEntityType.Player;

        /// <inheritdoc />
        public void Execute(IEntity entity, SystemEventArgs args)
        {
            if (!(entity is IPlayerEntity player))
            {
                this._logger.LogError($"Cannot execute {nameof(TeleportSystem)}. {entity.Object.Name} is not a player.");
                return;
            }

            if (!args.CheckArguments())
            {
                this._logger.LogError($"Cannot execute {nameof(TeleportSystem)} action: {args.GetType()} due to invalid arguments.");
                return;
            }

            switch (args)
            {
                case TeleportEventArgs e:
                    this.Teleport(player, e);
                    break;
            }
        }

        /// <summary>
        /// Teleports the player to the given map and position.
        /// </summary>
        /// <param name="player">Player.</param>
        /// <param name="e">Teleport args.</param>
        private void Teleport(IPlayerEntity player, TeleportEventArgs e)
        {
            // TODO
        }
    }
}
