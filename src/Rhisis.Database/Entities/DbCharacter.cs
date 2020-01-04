using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rhisis.Database.Entities
{
    [Table("Characters")]
    public sealed class DbCharacter : DbEntity
    {
        /// <summary>
        /// Gets or sets the character name.
        /// </summary>
        [Required]
        [Encrypted]
        public string Name { get; set; }
        
        /// <summary>
        /// Gets or sets the character gender.
        /// </summary>
        [Required]
        public byte Gender { get; set; }
        
        /// <summary>
        /// Gets or sets the character level.
        /// </summary>
        [Required]
        [DefaultValue(1)]
        public int Level { get; set; }
        
        /// <summary>
        /// Gets or sets the character level experience progression.
        /// </summary>
        public long Experience { get; set; }
        
        /// <summary>
        /// Gets or sets the character class.
        /// </summary>
        public int ClassId { get; set; }
        
        /// <summary>
        /// Gets or sets the character gold amount.
        /// </summary>
        public int Gold { get; set; }
        
        /// <summary>
        /// Gets or sets the character slot.
        /// </summary>
        [Required]
        public int Slot { get; set; }
        
        /// <summary>
        /// Gets or sets the character strength.
        /// </summary>
        public int Strength { get; set; }
        
        /// <summary>
        /// Gets or sets the character stamina.
        /// </summary>
        public int Stamina { get; set; }
        
        /// <summary>
        /// Gets or sets the character dexterity.
        /// </summary>
        public int Dexterity { get; set; }
        
        /// <summary>
        /// Gets or sets the character intelligence.
        /// </summary>
        public int Intelligence { get; set; }
        
        /// <summary>
        /// Gets or sets the character hit points.
        /// </summary>
        public int Hp { get; set; }
        
        /// <summary>
        /// Gets or sets the character magic points.
        /// </summary>
        public int Mp { get; set; }
        
        /// <summary>
        /// Gets or sets the character fatigue points.
        /// </summary>
        public int Fp { get; set; }

        /// <summary>
        /// Gets or sets the character skin set id.
        /// </summary>
        [Required]
        public int SkinSetId { get; set; }

        /// <summary>
        /// Gets or sets the character hair id.
        /// </summary>
        [Required]
        public int HairId { get; set; }

        /// <summary>
        /// Gets or sets the character hair color.
        /// </summary>
        [Required]
        public int HairColor { get; set; }

        /// <summary>
        /// Gets or sets the character hair id.
        /// </summary>
        [Required]
        public int FaceId { get; set; }
        
        /// <summary>
        /// Gets or sets the character map id.
        /// </summary>
        public int MapId { get; set; }

        /// <summary>
        /// Gets or sets character map layer id.
        /// </summary>
        public int MapLayerId { get; set; }
        
        /// <summary>
        /// Gets or sets the character X position.
        /// </summary>
        public float PosX { get; set; }
        
        /// <summary>
        /// Gets or sets the character Y position.
        /// </summary>
        public float PosY { get; set; }
        
        /// <summary>
        /// Gets or sets the character Z position.
        /// </summary>
        public float PosZ { get; set; }
        
        /// <summary>
        /// Gets or sets the character orientation angle.
        /// </summary>
        public float Angle { get; set; }
        
        /// <summary>
        /// Gets or sets the character bank code.
        /// </summary>
        public int BankCode { get; set; }
        
        /// <summary>
        /// Gets or sets the character remaining statistics points.
        /// </summary>
        public int StatPoints { get; set; }
        
        /// <summary>
        /// Gets or sets the character remaining skill points.
        /// </summary>
        public int SkillPoints { get; set; }
        
        /// <summary>
        /// Gets or sets the last connection time.
        /// </summary>
        [Column(TypeName = "DATETIME")]
        public DateTime LastConnectionTime { get; set; }

        /// <summary>
        /// Gets or sets the play time amount in seconds.
        /// </summary>
        [Column(TypeName = "BIGINT")]
        public long PlayTime { get; set; }

        /// <summary>
        /// Gets or sets a flag that indicates if the character is deleted.
        /// </summary>
        [DefaultValue(false)]
        public bool IsDeleted { get; set; }

        /// <summary>
        /// Gets the character associated user id.
        /// </summary>
        [Required]
        public int UserId { get; set; }

        /// <summary>
        /// Gets the character associated user.
        /// </summary>
        [ForeignKey(nameof(UserId))]
        public DbUser User { get; set; }

        /// <summary>
        /// Gets the character items.
        /// </summary>
        public ICollection<DbItem> Items { get; set; }

        /// <summary>
        /// Gets the character received mails.
        /// </summary>
        public ICollection<DbMail> ReceivedMails { get; set; }

        /// <summary>
        /// Gets the character sent mails.
        /// </summary>
        public ICollection<DbMail> SentMails { get; set; }

        /// <summary>
        /// Gets the character shortcuts.
        /// </summary>
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
