using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rhisis.Database.Entities
{
    [Table("users")]
    public class DbUser : DbEntity
    {
        [Required]
        [MaxLength(42)]
        [Column(TypeName = "VARCHAR(42)")]
        public string Username { get; set; }

        [Required]
        [MaxLength(42)]
        [Column(TypeName = "VARCHAR(42)")]
        public string Password { get; set; }

        [Required]
        public int Authority { get; set; }

        public ICollection<DbCharacter> Characters { get; set; }

        public DbUser()
        {
            this.Characters = new HashSet<DbCharacter>();
        }
    }
}
