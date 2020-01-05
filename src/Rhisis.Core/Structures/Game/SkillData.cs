using Rhisis.Core.Data;
using System.Collections.Generic;
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
        public ItemKind3 LinkKind { get; internal set; }

        /// <summary>
        /// Gets the skill element (Fire, Water, Wind, etc...)
        /// </summary>
        [DataMember(Name = "eItemType")]
        public ElementType Element { get; internal set; }

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
        public int RequiredSkillId2 { get; internal set; }

        /// <summary>
        /// Gets the second required skill level before learning the current skill.
        /// </summary>
        [DataMember(Name = "dwReSkillLevel2")]
        public int RequiredSkillLevel2 { get; internal set; }

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
        /// Gets the skill levels data.
        /// </summary>
        public IReadOnlyDictionary<int, SkillLevelData> SkillLevels { get; internal set; }
    }
}
