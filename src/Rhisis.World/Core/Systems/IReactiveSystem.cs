using Rhisis.World.Core.Entities;
using System;

namespace Rhisis.World.Core.Systems
{
    public interface IReactiveSystem : ISystem
    {
        void Execute(IEntity entity, EventArgs e);
    }
}
