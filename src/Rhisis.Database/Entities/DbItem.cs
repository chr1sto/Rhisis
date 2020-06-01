using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;

namespace Rhisis.Database.Entities
{
    [Table("items")]
    [DebuggerDisplay("Id: {ItemId} - slot: {ItemSlot} - x{ItemCount}")]
    public sealed class DbItem : DbEntity
    {        
        /// <summary>
        /// Gets or sets the real item Id.
        /// </summary>
        [Required]
        public int ItemId { get; set; }
        
        /// <summary>
        /// Gets or sets the amount of items.
        /// </summary>
        [Required]
        public int ItemCount { get; set; }
        
        /// <summary>
        /// Gets or sets the item slot.
        /// </summary>
        public int ItemSlot { get; set; }
        
        /// <summary>
        /// Gets or sets the item creator id.
        /// </summary>
        public int CreatorId { get; set; }
        
        /// <summary>
        /// Gets or sets the item refine.
        /// </summary>
        [DefaultValue(0)]
        public byte Refine { get; set; }

        /// <summary>
        /// Gets or sets the item element.
        /// </summary>
        [DefaultValue(0)]
        public byte Element { get; set; }

        /// <summary>
        /// Gets or sets the item element refine.
        /// </summary>
        [DefaultValue(0)]
        public byte ElementRefine { get; set; }

        /// <summary>
        /// Gets or sets a flag that indicates if the item is deleted.
        /// </summary>
        [DefaultValue(false)]
        public bool IsDeleted { get; set; }

        /// <summary>
        /// Gets or sets the character Id associated to this item.
        /// </summary>
        public int CharacterId { get; set; }

        /// <summary>
        /// Gets or sets the character associated to this item.
        /// </summary>
        [ForeignKey(nameof(CharacterId))]
        public DbCharacter Character { get; set; }

        public DbItem()
        {
        }

        public DbItem(int itemId, int itemSlot)
            : this(itemId, itemSlot, 1)
        {
        }

        public DbItem(int itemId, int itemSlot, int itemCount)
            : this(itemId, itemSlot, itemCount, 0, 0, 0)
        {
        }

        public DbItem(int itemId, int itemSlot, int itemCount, byte refine)
            : this(itemId, itemSlot, itemCount, refine, 0, 0)
        {
        }

        public DbItem(int itemId, int itemSlot, int itemCount, byte refine, byte element, byte elementRefine)
        {
            ItemId = itemId;
            ItemSlot = itemSlot;
            ItemCount = itemCount;
            Refine = refine;
            Element = element;
            ElementRefine = elementRefine;
        }
    }
}
