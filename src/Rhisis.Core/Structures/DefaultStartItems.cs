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
            this.StartWeapon = -1;
            this.StartSuit = -1;
            this.StartHand = -1;
            this.StartShoes = -1;
            this.StartHat = -1;
        }

        /// <summary>
        /// Creates a new default start items based on the gender type.
        /// </summary>
        /// <param name="gender">Character gender.</param>
        public DefaultStartItems(GenderType gender)
        {
            if (gender == GenderType.Male)
            {
                this.StartWeapon = 21;
                this.StartSuit = 502;
                this.StartHand = 506;
                this.StartShoes = 510;
                this.StartHat = -1;
            }
            else
            {
                this.StartWeapon = 21;
                this.StartSuit = 504;
                this.StartHand = 508;
                this.StartShoes = 512;
                this.StartHat = -1;
            }
        }
    }
}
