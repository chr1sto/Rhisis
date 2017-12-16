using System;

namespace Rhisis.World.Game.Core.Interfaces
{
    public interface INotifiableSystem : ISystem
    {
        void Execute(IEntity entity, EventArgs e);
    }
}
