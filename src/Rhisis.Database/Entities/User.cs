using Rhisis.Database.Interfaces;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rhisis.Database.Entities
{
    [Table("users")]
    public class User : IDatabaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public int Authority { get; set; }

        public ICollection<Character> Characters { get; set; }

        public User()
        {
            this.Characters = new HashSet<Character>();
        }
    }
}
