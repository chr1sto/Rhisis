using Rhisis.Core.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace Rhisis.Core.Structures.Game
{
    /// <summary>
    /// Represents an Item data from the propItem.txt file.
    /// </summary>
    [DataContract]
    public class ItemData
    {
        [DataMember(Name = "ver6")]
        public int Version { get; set; }

        [DataMember(Name = "dwID")]
        public int Id { get; set; }

        [DataMember(Name = "szName")]
        public string Name { get; set; }

        [DataMember(Name = "dwPackMax")]
        public int PackMax { get; set; }

        [DataMember(Name = "dwItemKind1")]
        public ItemKind1 ItemKind1 { get; set; }

        [DataMember(Name = "dwItemKind2")]
        public ItemKind2 ItemKind2 { get; set; }

        [DataMember(Name = "dwItemKind3")]
        public ItemKind3 ItemKind3 { get; set; }

        [DataMember(Name = "dwItemSex")]
        [DefaultValue(int.MaxValue)]
        public int ItemSex { get; set; }

        [DataMember(Name = "dwCost")]
        public int Cost { get; set; }

        [DataMember(Name = "dwLimitLevel1")]
        public int LimitLevel { get; set; }

        [DataMember(Name = "dwParts")]
        public ItemPartType Parts { get; set; }

        [DataMember(Name = "dwAbilityMin")]
        public int AbilityMin { get; set; }

        [DataMember(Name = "dwAbilityMax")]
        public int AbilityMax { get; set; }

        [DataMember(Name = "eItemType")]
        public ElementType Element { get; set; }

        [DataMember(Name = "dwItemLV")]
        public int Level { get; set; }

        [DataMember(Name = "dwItemRare")]
        public int Rare { get; set; }

        [DataMember(Name = "dwAttackSpeed")]
        public float AttackSpeed { get; set; }

        [DataMember(Name = "dwDestParam1")]
        public string DestParam1 { get; set; }

        [DataMember(Name = "dwDestParam2")]
        public string DestParam2 { get; set; }

        [DataMember(Name = "dwDestParam3")]
        public string DestParam3 { get; set; }

        [DataMember(Name = "nAdjParamVal1")]
        public int AdjustParam1 { get; set; }

        [DataMember(Name = "nAdjParamVal2")]
        public int AdjustParam2 { get; set; }

        [DataMember(Name = "nAdjParamVal3")]
        public int AdjustParam3 { get; set; }

        [DataMember(Name = "dwCircleTime")]
        public int CircleTime { get; set; }

        [DataMember(Name = "dwUseable")]
        public bool IsUseable { get; set; }

        [DataMember(Name = "dwSfxObj")]
        public int SfxObject { get; set; }

        [DataMember(Name = "dwSfxObj2")]
        public int SfxObject2 { get; set; }

        [DataMember(Name = "dwSfxObj3")]
        public int SfxObject3 { get; set; }

        [DataMember(Name = "dwSfxObj4")]
        public int SfxObject4 { get; set; }

        [DataMember(Name = "dwSfxObj5")]
        public int SfxObject5 { get; set; }

        [DataMember(Name = "bPermanence")]
        public bool IsPermanant { get; set; }

        [DataMember(Name = "dwSkillReady")]
        public int CoolTime { get; set; }

        [DataMember(Name = "dwWeaponType")]
        public int WeaponTypeId { get; set; }

        [DataMember(Name = "dwItemAtkOrder1")]
        public int ItemAtkOrder1 { get; set; }

        [DataMember(Name = "dwItemAtkOrder2")]
        public int ItemAtkOrder2 { get; set; }

        [DataMember(Name = "dwItemAtkOrder3")]
        public int ItemAtkOrder3 { get; set; }

        [DataMember(Name = "dwItemAtkOrder4")]
        public int ItemAtkOrder4 { get; set; }

        [DataMember(Name = "dwSkillReadyType")]
        public int SkillReadyType { get; set; }

        [DataMember(Name = "dwReferStat1")]
        public WeaponKindType WeaponKind { get; set; }

        [IgnoreDataMember]
        public WeaponType WeaponType => (WeaponType)Enum.ToObject(typeof(WeaponType), WeaponTypeId);

        [DataMember(Name = "dwAddSkillMin")]
        public int AttackSkillMin { get; set; }

        [DataMember(Name = "dwAddSkillMax")]
        public int AttackSkillMax { get; set; }

        [IgnoreDataMember]
        public bool IsStackable => PackMax > 1;

        [IgnoreDataMember]
        public IReadOnlyDictionary<DefineAttributes, int> Params { get; internal set; }
    }
}
