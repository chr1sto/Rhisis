using Rhisis.World.Core.Entities;
using System;

namespace Rhisis.World.Core.Systems
{
    public interface IUpdateSystem : ISystem
    {
        Func<IEntity, bool> Filter { get; }
        
        void Execute(IEntity entity);

        bool Match(IEntity entity);
    }
}
