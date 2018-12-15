using System.Runtime.Serialization;

namespace Rhisis.Core.Structures.Game
{
    [DataContract]
    public class JobData
    {
        [DataMember(Order = 0)]
        public int Id { get; set; }

        [DataMember(Order = 1)]
        public float AttackSpeed { get; set; }

        [DataMember(Order = 2)]
        public float MaxHpFactor { get; set; }

        [DataMember(Order = 3)]
        public float MaxMpFactor { get; set; }

        [DataMember(Order = 4)]
        public float MaxFpFactor { get; set; }

        [DataMember(Order = 5)]
        public float DefenseFactor { get; set; }

        [DataMember(Order = 6)]
        public float HpRecoveryFactor { get; set; }

        [DataMember(Order = 7)]
        public float MpRecoveryFactor { get; set; }

        [DataMember(Order = 8)]
        public float FpRecoveryFactor { get; set; }

        [DataMember(Order = 9)]
        public float MeleeSword { get; set; }

        [DataMember(Order = 10)]
        public float MeleeAxe { get; set; }

        [DataMember(Order = 11)]
        public float MeleeStaff { get; set; }

        [DataMember(Order = 12)]
        public float MeleeStick { get; set; }

        [DataMember(Order = 13)]
        public float MeleeKnuckle { get; set; }

        [DataMember(Order = 14)]
        public float MagicWand { get; set; }

        [DataMember(Order = 15)]
        public float Blocking { get; set; }

        [DataMember(Order = 16)]
        public float MeleeYoyo { get; set; }

        [DataMember(Order = 17)]
        public float Critical { get; set; }
    }
}
