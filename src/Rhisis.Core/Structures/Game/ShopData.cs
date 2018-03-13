using System.Collections.Generic;
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
        public List<ShopItemData>[] Items { get; set; }

        public ShopData(string shopName, int shopTabs = 4)
        {
            this.Name = shopName;
            this.Items = new List<ShopItemData>[shopTabs];

            for (var i = 0; i < shopTabs; i++)
            {
                this.Items[i] = new List<ShopItemData>();
            }
        }
    }
}
