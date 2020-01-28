using Rhisis.Core.Data;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace Rhisis.Core.Structures.Game
{
    [DataContract]
    [DebuggerDisplay("Skill '{SkillId}' level: {Level}")]
    public class SkillLevelData
    {
        /// <summary>
        /// Gets the skill level id.
        /// </summary>
        [DataMember(Name = "dwID")]
        public int SkillLevelId { get; set; }

        /// <summary>
        /// Gets the skill id.
        /// </summary>
        [DataMember(Name = "dwName")]
        public int SkillId { get; internal set; }

        /// <summary>
        /// Gets the skill level.
        /// </summary>
        [DataMember(Name = "dwSkillLvl")]
        public int Level { get; internal set; }

        /// <summary>
        /// Gets the skill minimum damages/ability.
        /// </summary>
        [DataMember(Name = "dwAbilityMin")]
        public int AbilityMin { get; internal set; }

        /// <summary>
        /// Gets the skill minimum damages/ability.
        /// </summary>
        [DataMember(Name = "dwAtkAbilityMax")]
        public int AbilityMax { get; internal set; }

        /// <summary>
        /// Gets the skill minimum damages/ability when used in PVP.
        /// </summary>
        [DataMember(Name = "dwAbilityMinPVP")]
        public int AbilityMinPVP { get; internal set; }

        /// <summary>
        /// Gets the skill maximum damages/ability when used in PVP.
        /// </summary>
        [DataMember(Name = "dwAbilityMaxPVP")]
        public int AbilityMaxPVP { get; internal set; }

        /// <summary>
        /// Gets the skill attack speed.
        /// </summary>
        [DataMember(Name = "dwAttackSpeed")]
        public int AttackSpeed { get; internal set; }

        /// <summary>
        /// Gets the skill damage shifting option.
        /// </summary>
        [DataMember(Name = "dwDmgShift")]
        public bool DamageShift { get; internal set; }

        /// <summary>
        /// Gets the skill (critical?) probability.
        /// </summary>
        [DataMember(Name = "nProbability")]
        public byte Probability { get; internal set; }

        /// <summary>
        /// Gets the skill (critical?) probability in PVP.
        /// </summary>
        [DataMember(Name = "nProbabilityPVP")]
        public byte ProbabilityPVP { get; internal set; }

        /// <summary>
        /// Gets the skill taunt factor.
        /// </summary>
        [DataMember(Name = "dwTaunt")]
        public int Taunt { get; internal set; }

        /// <summary>
        /// Gets the skill first dest param bonus.
        /// </summary>
        [DataMember(Name = "dwDestParam1")]
        public DefineAttributes DestParam1 { get; set; }

        /// <summary>
        /// Gets the skill first dest param bonus value.
        /// </summary>
        [DataMember(Name = "nAdjParamVal1")]
        public int DestParam1Value { get; set; }

        /// <summary>
        /// Gets the skill second dest param bonus.
        /// </summary>
        [DataMember(Name = "dwDestParam2")]
        public DefineAttributes DestParam2 { get; set; }

        /// <summary>
        /// Gets the skill second dest param bonus value.
        /// </summary>
        [DataMember(Name = "nAdjParamVal2")]
        public int DestParam2Value { get; set; }

        /// <summary>
        /// Gets the skill required MP to be casted.
        /// </summary>
        [DataMember(Name = "dwReqMp")]
        public int RequiredMP { get; set; }

        /// <summary>
        /// Gets the skill required FP to be casted.
        /// </summary>
        [DataMember(Name = "dwRepFp")]
        public int RequiredFP { get; set; }

        /// <summary>
        /// Gets the skill cooldown time.
        /// </summary>
        [DataMember(Name = "dwCooldown")]
        public int CooldownTime { get; set; }

        /// <summary>
        /// Gets the skill casting time.
        /// </summary>
        [DataMember(Name = "dwCastingTime")]
        public int CastingTime { get; set; }

        /// <summary>
        /// Gets the skill range.
        /// </summary>
        [DataMember(Name = "dwSkillRange")]
        public int SkillRange { get; set; }

        /// <summary>
        /// Gets the skill circle time. (Maybe for AOE attacks?)
        /// </summary>
        [DataMember(Name = "dwCircleTime")]
        public int CircleTime { get; set; }

        /// <summary>
        /// Gets the skill pain time. (maybe for bleeding skills?)
        /// </summary>
        [DataMember(Name = "dwPainTime")]
        public int PainTime { get; set; }

        /// <summary>
        /// Gets the recovery skill time.
        /// </summary>
        [DataMember(Name = "dwSkillTime")]
        public int SkillTime { get; set; }

        /// <summary>
        /// Gets the skill count. (?)
        /// </summary>
        [DataMember(Name = "dwSkillCount")]
        public int SkillCount { get; set; }

        /// <summary>
        /// Gets the skill experience count.
        /// </summary>
        [DataMember(Name = "dwSkillExp")]
        public int SkillExp { get; set; }

        /// <summary>
        /// Gets the job experience count. (?)
        /// </summary>
        [DataMember(Name = "dwExp")]
        public int Experience { get; set; }

        /// <summary>
        /// Gets the skill recovery time in action slot.
        /// </summary>
        [DataMember(Name = "dwComboSkillTime")]
        public int ComboSkillTime { get; set; }
    }
}
