using Rhisis.Core.Attributes;
using Rhisis.Core.Data;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace Rhisis.Core.Structures.Game
{
    [DataContract]
    [DebuggerDisplay("{Job}:{Name} (id: {Id})")]
    public class SkillData
    {
        /// <summary>
        /// Gets the skill version.
        /// </summary>
        [DataMember(Name = "ver")]
        public int Version { get; internal set; }

        /// <summary>
        /// Gets the skill id.
        /// </summary>
        [DataMember(Name = "dwID")]
        public int Id { get; internal set; }

        /// <summary>
        /// Gets the skill index.
        /// </summary>
        [DataIndex]
        [DataMember]
        public int Index { get; internal set; }

        /// <summary>
        /// Gets the skill name.
        /// </summary>
        [DataMember(Name = "szName")]
        public string Name { get; internal set; }

        /// <summary>
        /// Gets the skill job type. (Expert, profesionnal, master, hero)
        /// </summary>
        [DataMember(Name = "dwItemKind1")]
        public DefineJob.JobType JobType { get; internal set; }

        /// <summary>
        /// Gets the skill job. (Vagrant, Mercenary, Assist, etc...)
        /// </summary>
        [DataMember(Name = "dwItemKind2")]
        public DefineJob.Job Job { get; internal set; }

        /// <summary>
        /// Gets the skill type group. (Sword, axe, magic)
        /// </summary>
        [DataMember(Name = "dwItemKind3")]
        public DefineJob.SkillGroupDisciple Group { get; internal set; }

        /// <summary>
        /// Gets the skill link kind.
        /// </summary>
        [DataMember(Name = "dwLinkKind")]
        public ItemKind3? LinkKind { get; internal set; }

        /// <summary>
        /// Gets the skill bullet link kind.
        /// </summary>
        [DataMember(Name = "dwLinkKindBullet")]
        public ItemKind2? BulletLinkKind { get; internal set; }

        /// <summary>
        /// Gets the skill element (Fire, Water, Wind, etc...)
        /// </summary>
        [DataMember(Name = "eItemType")]
        public ElementType? Element { get; internal set; }
        
        /// <summary>
        /// Gets the skill continuous pain time.
        /// </summary>
        [DataMember(Name = "tmContinuousPain")]
        public int ContinuousPainTime { get; internal set; }

        /// <summary>
        /// Gets the skill required level to be used.
        /// </summary>
        [DataMember(Name = "dwReqDisLV")]
        public int RequiredLevel { get; internal set; }

        /// <summary>
        /// Gets the first required skill id before learning the current skill.
        /// </summary>
        [DataMember(Name = "dwReSkill1")]
        [DefaultValue(-1)]
        public int RequiredSkillId1 { get; internal set; }

        /// <summary>
        /// Gets the first required skill level before learning the current skill.
        /// </summary>
        [DataMember(Name = "dwReSkillLevel1")]
        public int RequiredSkillLevel1 { get; internal set; }

        /// <summary>
        /// Gets the second required skill id before learning the current skill.
        /// </summary>
        [DataMember(Name = "dwReSkill2")]
        [DefaultValue(-1)]
        public int RequiredSkillId2 { get; internal set; }

        /// <summary>
        /// Gets the second required skill level before learning the current skill.
        /// </summary>
        [DataMember(Name = "dwReSkillLevel2")]
        public int RequiredSkillLevel2 { get; internal set; }

        /// <summary>
        /// Gets the time for the skill to be ready to use.
        /// </summary>
        [DataMember(Name = "dwSkillReady")]
        public int SkillReadyTime { get; internal set; }

        /// <summary>
        /// Gets the skill special effect.
        /// </summary>
        [DataMember(Name = "dwSfxObj")]
        public DefineSpecialEffects SpecialEffect1 { get; internal set; }

        /// <summary>
        /// Gets the skill special effect.
        /// </summary>
        [DataMember(Name = "dwSfxObj2")]
        public DefineSpecialEffects SpecialEffect2 { get; internal set; }

        /// <summary>
        /// Gets the skill special effect.
        /// </summary>
        [DataMember(Name = "dwSfxObj3")]
        public DefineSpecialEffects SpecialEffect3 { get; internal set; }

        /// <summary>
        /// Gets the skill special effect.
        /// </summary>
        [DataMember(Name = "dwSfxObj4")]
        public DefineSpecialEffects SpecialEffect4 { get; internal set; }

        /// <summary>
        /// Gets the skill special effect.
        /// </summary>
        [DataMember(Name = "dwSfxObj5")]
        public DefineSpecialEffects SpecialEffect5 { get; internal set; }

        /// <summary>
        /// Gets the skill max boost level.
        /// </summary>
        [DataMember(Name = "ExpertMax")]
        public int MaxLevel { get; internal set; }

        /// <summary>
        /// Gets the skill type.
        /// </summary>
        [DataMember(Name = "dwSkillType")]
        public SkillType Type { get; internal set; }

        /// <summary>
        /// Gets the skill spell region.
        /// </summary>
        [DataMember(Name = "dwSpellRegion")]
        public SpellRegionType SpellRegionType { get; internal set; }

        /// <summary>
        /// Gets the skill spell type.
        /// </summary>
        [DataMember(Name = "dwSpellType")]
        public SpellType SpellType { get; internal set; }

        /// <summary>
        /// Gets the skill execution target.
        /// </summary>
        [DataMember(Name = "dwExeTarget")]
        public SkillExecuteTargetType ExecuteTarget { get; internal set; }

        /// <summary>
        /// Gets the skill first refer statistic type.
        /// </summary>
        [DataMember(Name = "dwReferStat1")]
        public DefineAttributes ReferStat1 { get; internal set; }

        /// <summary>
        /// Gets the skill second refer statistic type.
        /// </summary>
        [DataMember(Name = "dwReferStat2")]
        public DefineAttributes ReferStat2 { get; internal set; }

        /// <summary>
        /// Gets the skill first refer target.
        /// </summary>
        [DataMember(Name = "dwReferTarget1")]
        public SkillReferTargetType ReferTarget1 { get; internal set; }

        /// <summary>
        /// Gets the skill first refer target value.
        /// </summary>
        [DataMember(Name = "dwReferValue1")]
        public int ReferValue1 { get; internal set; }

        /// <summary>
        /// Gets the skill second refer target.
        /// </summary>
        [DataMember(Name = "dwReferTaget2")]
        public SkillReferTargetType ReferTarget2 { get; internal set; }

        /// <summary>
        /// Gets the skill second refer target value.
        /// </summary>
        [DataMember(Name = "dwReferValue2")]
        public int ReferValue2 { get; internal set; }

        /// <summary>
        /// Gets the skill weapon handed type.
        /// </summary>
        [DataMember(Name = "dwHanded")]
        public WeaponHandType? Handed { get; internal set; }

        /// <summary>
        /// Gets the skill levels data.
        /// </summary>
        public IReadOnlyDictionary<int, SkillLevelData> SkillLevels { get; internal set; }
    }
}
