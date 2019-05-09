using Rhisis.Database.Entities;

namespace Rhisis.World.Game.Components
{
    public class StatisticsComponent
    {
        public ushort Strength { get; set; }

        public ushort Stamina { get; set; }

        public ushort Dexterity { get; set; }

        public ushort Intelligence { get; set; }

        public ushort StatPoints { get; set; }

        public ushort SkillPoints { get; set; }

        public StatisticsComponent()
        {
        }

        public StatisticsComponent(DbCharacter character)
        {
            this.Strength = (ushort)character.Strength;
            this.Stamina = (ushort)character.Stamina;
            this.Dexterity = (ushort)character.Dexterity;
            this.Intelligence = (ushort)character.Intelligence;
            this.StatPoints = (ushort)character.StatPoints;
            this.SkillPoints = (ushort)character.SkillPoints;
        }
    }
}
