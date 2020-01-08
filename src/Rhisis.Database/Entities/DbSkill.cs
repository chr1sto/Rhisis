using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rhisis.Database.Entities
{
    [Table("Skills")]
    public class DbSkill : DbEntity
    {
        /// <summary>
        /// Gets or sets the skill id.
        /// </summary>
        [Required]
        public int SkillId { get; set; }

        /// <summary>
        /// Gets or sets the skill level.
        /// </summary>
        [Required]
        public byte Level { get; set; }

        /// <summary>
        /// Gets or sets the character id.
        /// </summary>
        [Required]
        public int CharacterId { get; set; }

        /// <summary>
        /// Gets or sets the character.
        /// </summary>
        [ForeignKey(nameof(CharacterId))]
        public DbCharacter Character { get; set; }
    }
}
