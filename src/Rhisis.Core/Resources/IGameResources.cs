using Rhisis.Core.Structures.Game;
using System.Collections.Generic;

namespace Rhisis.Core.Resources
{
    public interface IGameResources
    {
        IDictionary<int, MoverData> Movers { get; }

        IDictionary<int, ItemData> Items { get; }

        void Load();
    }
}
