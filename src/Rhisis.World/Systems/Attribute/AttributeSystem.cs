using Microsoft.Extensions.Logging;
using Rhisis.Core.DependencyInjection;
using Rhisis.World.Game.Core;
using Rhisis.World.Game.Core.Systems;
using Rhisis.World.Game.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rhisis.World.Systems.Attribute
{
    /// <summary>
    /// Defines the attribute system that manages all attributes for every living mover entities.
    /// </summary>
    [System(SystemType.Notifiable)]
    public sealed class AttributeSystem : ISystem
    {
        private readonly ILogger<AttributeSystem> _logger;

        /// <inheritdoc />
        public WorldEntityType Type => WorldEntityType.Mover;

        /// <summary>
        /// Creates a new <see cref="AttributeSystem"/> instance.
        /// </summary>
        public AttributeSystem()
        {
            this._logger = DependencyContainer.Instance.Resolve<ILogger<AttributeSystem>>();
        }

        /// <inheritdoc />
        public void Execute(IEntity entity, SystemEventArgs args)
        {
            if (!(entity is ILivingEntity livingEntity))
            {
                this._logger.LogError($"Cannot execute {nameof(AttributeSystem)}. {entity.Object.Name} is not a living entity.");
                return;
            }

            if (!args.CheckArguments())
            {
                this._logger.LogError($"Cannot execute {nameof(AttributeSystem)} action: {args.GetType()} due to invalid arguments.");
                return;
            }

            throw new NotImplementedException();
        }
    }
}
