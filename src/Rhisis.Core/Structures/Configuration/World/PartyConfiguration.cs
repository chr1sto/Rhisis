using System.Runtime.Serialization;

namespace Rhisis.Core.Structures.Configuration.World
{
    [DataContract]
    public class PartyConfiguration
    {
        public const int DefaultMaxPartyMemberCount = 8;

        /// <summary>
        /// Gets or sets the maximum amount of members in a party.
        /// </summary>
        public int MaxPartyMemberCount { get; set; }
        
        /// <summary>
        /// Creates a new <see cref="PartyConfiguration"/> instance.
        /// </summary>
        public PartyConfiguration()
        {
            MaxPartyMemberCount = DefaultMaxPartyMemberCount;
        }
    }
}
