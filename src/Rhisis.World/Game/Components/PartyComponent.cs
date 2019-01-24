using Rhisis.World.Game.Structures;

namespace Rhisis.World.Game.Components
{
    public class PartyComponent
    {
        /// <summary>
        /// Gets or sets the party.
        /// </summary>
        public Party Party { get; set; }

        /// <summary>
        /// Gets a value indicating whether the player is inside a party.
        /// </summary>
        public bool IsInParty => Party != null;
    }
}
