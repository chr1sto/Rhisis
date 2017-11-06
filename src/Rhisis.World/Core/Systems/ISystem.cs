using Rhisis.World.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rhisis.World.Core.Systems
{
    public interface ISystem
    {
        Func<IEntity, bool> Filter { get; }

        void Execute();

        void Refresh();
    }
}
