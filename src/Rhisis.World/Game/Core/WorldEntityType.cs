using System;

namespace Rhisis.World.Game.Core
{
    /// <summary>
    /// Defines the different world entities.
    /// </summary>
    [Flags]
    public enum WorldEntityType
    {
        Unknown = 0,
        Player = 1,
        Monster = 2,
        Npc = 4,
        Drop = 8,
        Pet = 16,
        Movers = Player | Monster | Npc | Pet,
        All = Player | Monster | Npc | Drop | Pet,
    }
}
