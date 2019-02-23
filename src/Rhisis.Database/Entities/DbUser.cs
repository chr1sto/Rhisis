using Rhisis.Database.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rhisis.Database.Entities
{
    [Table("users")]
    public class DbUser : DbEntity
    {
        /// <summary>
        /// Gets or sets the user's identification name.
        /// </summary>
        [Required]
        [Encrypted]
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the user's password.
        /// </summary>
        [Required]
        [Encrypted]
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the user's email address.
        /// </summary>
        [Required]
        [Encrypted]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the last time the user logged in.
        /// </summary>
        [Column(TypeName = "DATETIME")]
        public DateTime LastConnectionTime { get; set; }

        /// <summary>
        /// Gets or sets the user's authority.
        /// </summary>
        [Required]
        public int Authority { get; set; }

        /// <summary>
        /// Gets or sets the user's characters list.
        /// </summary>
        public ICollection<DbCharacter> Characters { get; set; }

        /// <summary>
        /// Creates a new <see cref="DbUser"/> instance.
        /// </summary>
        public DbUser()
        {
            this.Characters = new HashSet<DbCharacter>();
        }
    }
}
