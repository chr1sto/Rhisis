using Newtonsoft.Json;
using Rhisis.Core.Common;
using System.Runtime.Serialization;

namespace Rhisis.Core.Structures
{
    /// <summary>
    /// Defines the character default start items structure configuration.
    /// </summary>
    [DataContract]
    public class DefaultStartItems
    {
        /// <summary>
        /// Gets or sets the default start weapon id.
        /// </summary>
        [DataMember]
        [JsonProperty]
        public int StartWeapon { get; set; }

        /// <summary>
        /// Gets or sets the default start suit id.
        /// </summary>
        [DataMember]
        [JsonProperty]
        public int StartSuit { get; set; }

        /// <summary>
        /// Gets or sets the default start hand (gloves) id.
        /// </summary>
        [DataMember]
        [JsonProperty]
        public int StartHand { get; set; }

        /// <summary>
        /// Gets or sets the default start shoes id.
        /// </summary>
        [DataMember]
        [JsonProperty]
        public int StartShoes { get; set; }

        /// <summary>
        /// Gets or sets the default start hat id.
        /// </summary>
        [DataMember]
        [JsonProperty]
        public int StartHat { get; set; }

        /// <summary>
        /// Creates a new default start item instance.
        /// </summary>
        public DefaultStartItems()
        {
            StartWeapon = -1;
            StartSuit = -1;
            StartHand = -1;
            StartShoes = -1;
            StartHat = -1;
        }

        /// <summary>
        /// Creates a new default start items based on the gender type.
        /// </summary>
        /// <param name="gender">Character gender.</param>
        public DefaultStartItems(GenderType gender)
        {
            if (gender == GenderType.Male)
            {
                StartWeapon = 21;
                StartSuit = 502;
                StartHand = 506;
                StartShoes = 510;
                StartHat = -1;
            }
            else
            {
                StartWeapon = 21;
                StartSuit = 504;
                StartHand = 508;
                StartShoes = 512;
                StartHat = -1;
            }
        }
    }
}
