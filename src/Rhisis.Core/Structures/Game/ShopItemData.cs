using System.Runtime.Serialization;

namespace Rhisis.Core.Structures.Game
{
    /// <summary>
    /// Represents a shop item.
    /// </summary>
    [DataContract]
    public class ShopItemData
    {
        /// <summary>
        /// Gets or sets the item id.
        /// </summary>
        [DataMember(Name = "itemId")]
        public int ItemId { get; set; }

        /// <summary>
        /// Gets or sets the item's refine.
        /// </summary>
        [DataMember(Name = "refine")]
        public int Refine { get; set; }

        /// <summary>
        /// Gets or sets the item's element.
        /// </summary>
        [DataMember(Name = "element")]
        public int Element { get; set; }

        /// <summary>
        /// Gets or sets the item's element refine.
        /// </summary>
        [DataMember(Name = "elementRefine")]
        public int ElementRefine { get; set; }
    }
}
