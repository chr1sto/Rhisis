using Rhisis.Core.Structures.Game.Quests;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rhisis.Database.Entities
{
    [Table("QuestActions")]
    public class DbQuestAction : DbEntity
    {
        /// <summary>
        /// Gets the quest action type.
        /// </summary>
        [Column]
        [Required]
        public QuestActionType Type { get; set; }

        /// <summary>
        /// Gets or sets the current quest action index.
        /// </summary>
        /// <remarks>
        /// This can be used to store the index of the monster to kill 
        /// when its a "KillMonster" quest type.
        /// </remarks>
        [Column]
        public int? Index { get; set; }

        /// <summary>
        /// Gets or sets the current quest action value.
        /// </summary>
        [Column]
        [Required]
        public int Value { get; set; }

        /// <summary>
        /// Gets or sets the quest action attached quest id.
        /// </summary>
        [Column]
        [Required]
        public int QuestId { get; set; }

        /// <summary>
        /// Gets or sets the question action attached quest.
        /// </summary>
        [ForeignKey(nameof(QuestId))]
        public DbQuest Quest { get; set; }
    }
}
