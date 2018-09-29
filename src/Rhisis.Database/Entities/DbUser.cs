using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rhisis.Database.Entities
{
    [Table("users")]
    public class DbUser : DbEntity
    {
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
