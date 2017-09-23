using Rhisis.Database.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rhisis.Database.Structures
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
    }
}
