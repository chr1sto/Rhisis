using System.Runtime.Serialization;

namespace Rhisis.Core.Structures.Game
{
    /// <summary>
    /// Represents a NPC Shop data.
    /// </summary>
    [DataContract]
    public class ShopData
    {
        /// <summary>
        /// Gets or sets the shop name.
        /// </summary>
        [DataMember(Name = "name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the shop items.
        /// </summary>
        [DataMember(Name = "items")]
        public ShopItemData[] Items { get; set; }
    }
}
