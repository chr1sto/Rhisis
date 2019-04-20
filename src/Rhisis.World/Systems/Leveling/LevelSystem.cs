using Microsoft.Extensions.Logging;
using Rhisis.Core.DependencyInjection;
using Rhisis.World.Game.Core;
using Rhisis.World.Game.Core.Systems;
using Rhisis.World.Game.Entities;
using System;

namespace Rhisis.World.Systems.Leveling
{
    [System(SystemType.Notifiable)]
    public sealed class LevelSystem : ISystem
    {
        private readonly ILogger<LevelSystem> _logger = DependencyContainer.Instance.Resolve<ILogger<LevelSystem>>();

        /// <inheritdoc />
        public WorldEntityType Type => WorldEntityType.Player;

        /// <inheritdoc />
        public void Execute(IEntity entity, SystemEventArgs args)
        {
            if (!(entity is IPlayerEntity player))
            {
                this._logger.LogError($"Cannot execute Level System. {entity.Object.Name} is not a player.");
                return;
            }

            if (!args.CheckArguments())
            {
                this._logger.LogError($"Cannot execute Level System action: {args.GetType()} due to invalid arguments.");
                return;
            }

            throw new NotImplementedException();
        }
    }
}
