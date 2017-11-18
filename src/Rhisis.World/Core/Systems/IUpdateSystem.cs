using Rhisis.World.Core.Entities;
using System;

namespace Rhisis.World.Core.Systems
{
    public interface IUpdateSystem : ISystem
    {
        void Execute(IEntity entity);

        bool Match(IEntity entity);
    }
}
