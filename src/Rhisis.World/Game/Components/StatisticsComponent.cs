using Rhisis.Database.Entities;

namespace Rhisis.World.Game.Components
{
    public class StatisticsComponent
    {
        public ushort StatPoints { get; set; }

        public ushort SkillPoints { get; set; }

        public StatisticsComponent()
        {
        }

        public StatisticsComponent(DbCharacter character)
        {
            StatPoints = (ushort)character.StatPoints;
            SkillPoints = (ushort)character.SkillPoints;
        }
    }
}
