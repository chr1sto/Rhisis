using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;

namespace Rhisis.Database.Entities
{
    [Table("quests")]
    [DebuggerDisplay("Id: {QuestId} | Finished: {Finished}")]
    public sealed class DbQuest : DbEntity
    {
        /// <summary>
        /// Gets or sets the quest id.
        /// </summary>
        [Column]
        [Required]
        public int QuestId { get; set; }

        /// <summary>
        /// Gets or sets a value that indicates if the quest is finished.
        /// </summary>
        [Column(TypeName = "BIT")]
        public bool Finished { get; set; }

        /// <summary>
        /// Gets or sets a value that indicates if the quest is checked in the user interface.
        /// </summary>
        [Column(TypeName = "BIT")]
        public bool IsChecked { get; set; }

        /// <summary>
        /// Gets or sets a value that indicates if the quest is deleted.
        /// </summary>
        [Column(TypeName = "BIT")]
        public bool IsDeleted { get; set; }

        /// <summary>
        /// Gets or sets the quest start date.
        /// </summary>
        [Column(TypeName = "DATETIME")]
        public DateTime StartTime { get; set; }

        /// <summary>
        /// Gets or sets a value that indicates that the patrol has been done.
        /// </summary>
        [Column(TypeName = "BIT")]
        public bool IsPatrolDone { get; set; }

        /// <summary>
        /// Gets or sets the amount of monsters killed for the first monster type.
        /// </summary>
        [Column(TypeName = "TINYINT")]
        public int MonsterKilled1 { get; set; }

        /// <summary>
        /// Gets or sets the amount of monsters killed for the second monster type.
        /// </summary>
        [Column(TypeName = "TINYINT")]
        public int MonsterKilled2 { get; set; }

        /// <summary>
        /// Gets or sets the character id the quest belongs to.
        /// </summary>
        [Column]
        [Required]
        public int CharacterId { get; set; }

        /// <summary>
        /// Gets or sets the character the quest belongs to.
        /// </summary>
        [ForeignKey(nameof(CharacterId))]
        public DbCharacter Character { get; set; }
    }
}
