using Rhisis.World.Game.Core;
using Rhisis.World.Game.Core.Systems;
using System;

namespace Rhisis.World.Systems.Battle
{
    [System(SystemType.Notifiable)]
    public class BattleSystem : ISystem
    {
        /// <inheritdoc />
        public WorldEntityType Type => WorldEntityType.Player | WorldEntityType.Monster;

        /// <inheritdoc />
        public void Execute(IEntity entity, SystemEventArgs args)
        {
            throw new NotImplementedException();
        }
    }
}
