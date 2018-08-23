using Rhisis.Database.Interfaces;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rhisis.Database.Entities
{
    [Table("users")]
    public class DbUser : IDatabaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public int Authority { get; set; }

        public ICollection<DbCharacter> Characters { get; set; }

        public DbUser()
        {
            this.Characters = new HashSet<DbCharacter>();
        }
    }
}
