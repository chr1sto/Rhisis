using Rhisis.World.Game.Components;
using Sylver.Network.Common;

namespace Rhisis.World.Game.Entities
{
    public interface IPlayerEntity : IWorldEntity, IMovableEntity, ILivingEntity
    {
        /// <summary>
        /// Gets or sets the player's visual appearance component.
        /// </summary>
        VisualAppearenceComponent VisualAppearance { get; set; }

        /// <summary>
        /// Gets or sets the player's data component.
        /// </summary>
        PlayerDataComponent PlayerData { get; set; }

        /// <summary>
        /// Gets or sets the player's inventory.
        /// </summary>
        InventoryContainerComponent Inventory { get; set; }

        /// <summary>
        /// Gets or sets the player's trade component.
        /// </summary>
        TradeComponent Trade { get; set; }

        /// <summary>
        /// Gets or sets the player's taskbar component.
        /// </summary>
        TaskbarComponent Taskbar { get; set; }

        /// <summary>
        /// Gets or sets the player's statistics component.
        /// </summary>
        StatisticsComponent Statistics { get; set; }

        /// <summary>
        /// Gets or sets the player's quest diary.
        /// </summary>
        QuestDiaryComponent QuestDiary { get; set; }

        /// <summary>
        /// Gets or sets the player's skill tree.
        /// </summary>
        SkillTreeComponent SkillTree { get; set; }

        /// <summary>
        /// Gets or sets the player's connection.
        /// </summary>
        INetUser Connection { get; set; }
    }
}
