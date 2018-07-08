using Rhisis.World.Game.Core;

namespace Rhisis.World.Systems.Statistics
{
    internal sealed class StatisticsModifyEventArgs : SystemEventArgs
    {
        public ushort Strenght { get; }

        public ushort Stamina { get; }

        public ushort Dexterity { get; }

        public ushort Intelligence { get; }

        public StatisticsModifyEventArgs(ushort strenght, ushort stamina, ushort dexterity, ushort intelligence)
        {
            Strenght = strenght;
            Stamina = stamina;
            Dexterity = dexterity;
            Intelligence = intelligence;
        }

        public override bool CheckArguments() => true;
    }
}
