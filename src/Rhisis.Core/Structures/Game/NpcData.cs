using System.Collections.Generic;

namespace Rhisis.Core.Structures.Game
{
    public class NpcData
    {
        /// <summary>
        /// Gets or sets the NPC id.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the NPC name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the NPC model id.
        /// </summary>
        public int ModelId { get; set; }

        /// <summary>
        /// Gets or sets the NPC hair id.
        /// </summary>
        public int HairId { get; set; }

        /// <summary>
        /// Gets or sets the NPC hair color.
        /// </summary>
        public int HairColor { get; set; }

        /// <summary>
        /// Gets or sets the NPC face id.
        /// </summary>
        public int FaceId { get; set; }

        /// <summary>
        /// Gets or sets the NPC viewable models.
        /// </summary>
        public ICollection<int> Items { get; set; }

        /// <summary>
        /// Gets the NPC Shop.
        /// </summary>
        public ShopData Shop { get; }

        /// <summary>
        /// Gets a value that indicates if the NPC has a shop.
        /// </summary>
        public bool HasShop => this.Shop != null;

        /// <summary>
        /// Creates a new <see cref="NpcData"/> instance.
        /// </summary>
        /// <param name="id">Npc id</param>
        /// <param name="name">Npc Name</param>
        public NpcData(string id, string name)
            : this(id, name, null)
        {   
        }

        /// <summary>
        /// Creates a new <see cref="NpcData"/> instance.
        /// </summary>
        /// <param name="id">Npc id</param>
        /// <param name="name">Npc Name</param>
        /// <param name="shop">Npc Shop</param>
        public NpcData(string id, string name, ShopData shop)
        {
            this.Id = id;
            this.Name = name;
            this.Shop = shop;
        }
    }
}
