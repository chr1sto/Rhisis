using Rhisis.Database.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rhisis.Database.Entities
{
    [Table("items")]
    public sealed class Item : IDatabaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        public int ItemId { get; set; }
        
        public int CharacterId { get; set; }
        
        public int ItemCount { get; set; }
        
        public int ItemSlot { get; set; }
        
        public int CreatorId { get; set; }
        
        public byte Refine { get; set; }
        
        public byte Element { get; set; }
        
        public byte ElementRefine { get; set; }

        public Character Character { get; set; }

        public Item()
        {
        }

        public Item(int itemId, int itemSlot)
            : this(itemId, itemSlot, 1)
        {
        }

        public Item(int itemId, int itemSlot, int itemCount)
            : this(itemId, itemSlot, itemCount, 0, 0, 0)
        {
        }

        public Item(int itemId, int itemSlot, int itemCount, byte refine)
            : this(itemId, itemSlot, itemCount, refine, 0, 0)
        {
        }

        public Item(int itemId, int itemSlot, int itemCount, byte refine, byte element, byte elementRefine)
        {
            this.ItemId = itemId;
            this.ItemSlot = itemSlot;
            this.ItemCount = itemCount;
            this.Refine = refine;
            this.Element = element;
            this.ElementRefine = elementRefine;
        }

        public override string ToString()
        {
            return $"Id: {this.ItemId} - slot: {this.ItemSlot} - x{this.ItemCount}";
        }
    }
}
