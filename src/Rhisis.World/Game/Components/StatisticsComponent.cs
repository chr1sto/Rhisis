using System;
using System.Collections.Generic;
using System.Text;
using Rhisis.Database.Structures;

namespace Rhisis.World.Game.Components
{
    public class StatisticsComponent
    {
        public int Strenght { get; set; }

        public int Stamina { get; set; }

        public int Dexterity { get; set; }

        public int Intelligence { get; set; }

        public int StatPoints { get; set; }

        public StatisticsComponent(Character character)
        {
            this.Strenght = character.Strength;
            this.Stamina = character.Stamina;
            this.Dexterity = character.Dexterity;
            this.Intelligence = character.Intelligence;
            this.StatPoints = character.StatPoints;
        }

        public StatisticsComponent()
        {
        }
    }
}
