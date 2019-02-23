using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rhisis.Database.Entities
{
    [Table("characters")]
    public sealed class DbCharacter : DbEntity
    {
        public int UserId { get; set; }

        [Required]
        public string Name { get; set; }
        
        [Required]
        public byte Gender { get; set; }
        
        [Required]
        [DefaultValue(1)]
        public int Level { get; set; }
        
        public long Experience { get; set; }
        
        public int ClassId { get; set; }
        
        public int Gold { get; set; }
        
        [Required]
        public int Slot { get; set; }
        
        public int Strength { get; set; }
        
        public int Stamina { get; set; }
        
        public int Dexterity { get; set; }
        
        public int Intelligence { get; set; }
        
        public int Hp { get; set; }
        
        public int Mp { get; set; }
        
        public int Fp { get; set; }

        [Required]
        public int SkinSetId { get; set; }

        [Required]
        public int HairId { get; set; }

        [Required]
        public int HairColor { get; set; }

        [Required]
        public int FaceId { get; set; }
        
        public int MapId { get; set; }

        public int MapLayerId { get; set; }
        
        public float PosX { get; set; }
        
        public float PosY { get; set; }
        
        public float PosZ { get; set; }
        
        public float Angle { get; set; }
        
        public int BankCode { get; set; }
        
        public int StatPoints { get; set; }
        
        public int SkillPoints { get; set; }

        public DbUser User { get; set; }

        public ICollection<DbItem> Items { get; set; }

        public ICollection<DbMail> ReceivedMails { get; set; }

        public ICollection<DbMail> SentMails { get; set; }

        public ICollection<DbShortcut> TaskbarShortcuts { get; set; }

        public DbCharacter()
        {
            this.Items = new HashSet<DbItem>();
            this.ReceivedMails = new HashSet<DbMail>();
            this.SentMails = new HashSet<DbMail>();
            this.TaskbarShortcuts = new HashSet<DbShortcut>();
        }
    }
}
