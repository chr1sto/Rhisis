using Rhisis.World.Game.Common;

namespace Rhisis.World.Systems.Battle
{
    public class AttackResult
    {
        public int AttackMin { get; set; }

        public int AttackMax { get; set; }

        public int Damages { get; set; }

        public AttackFlags Flags { get; set; }
    }
}
