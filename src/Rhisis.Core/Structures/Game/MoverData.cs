using Rhisis.Core.Data;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Rhisis.Core.Structures.Game
{
    /// <summary>
    /// Represents a Mover data structure from the propMover.txt resource file from the data.res.
    /// </summary>
    [DataContract]
    public class MoverData
    {
        /// <summary>
        /// Gets the mover id.
        /// </summary>
        [DataMember(Name = "dwID")]
        public int Id { get; private set; }

        /// <summary>
        /// Gets or sets the mover name.
        /// </summary>
        [DataMember(Name = "szName")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the mover AI id.
        /// </summary>
        [DataMember(Name = "dwAI")]
        public int AI { get; set; }

        /// <summary>
        /// Gets or sets the mover belligerence.
        /// </summary>
        [DataMember(Name = "dwBelligerence")]
        public int Belligerence { get; set; }

        /// <summary>
        /// Gets or sets the mover speed.
        /// </summary>
        [DataMember(Name = "fSpeed")]
        public float Speed { get; set; }

        /// <summary>
        /// Gets or sets the mover Hit Points (HP).
        /// </summary>
        [DataMember(Name = "dwAddHp")]
        public int AddHp { get; set; }

        /// <summary>
        /// Gets or sets the mover Magic Points (MP).
        /// </summary>
        [DataMember(Name = "dwAddMp")]
        public int AddMp { get; set; }

        /// <summary>
        /// Gets or sets the mover level.
        /// </summary>
        [DataMember(Name = "dwLevel")]
        public int Level { get; set; }

        /// <summary>
        /// Gets or sets the mover flight level.
        /// </summary>
        [DataMember(Name = "dwFilghtLevel")]
        public int FlightLevel { get; set; }

        /// <summary>
        /// Gets or sets the mover attack min.
        /// </summary>
        [DataMember(Name = "dwAtkMin")]
        public int AttackMin { get; set; }

        /// <summary>
        /// Gets or sets the mover attack max.
        /// </summary>
        [DataMember(Name = "dwAtkMax")]
        public int AttackMax { get; set; }

        /// <summary>
        /// Gets or sets the mover strength.
        /// </summary>
        [DataMember(Name = "dwStr")]
        public int Strength { get; set; }

        /// <summary>
        /// Gets or sets the mover stamina.
        /// </summary>
        [DataMember(Name = "dwSta")]
        public int Stamina { get; set; }

        /// <summary>
        /// Gets or sets the mover dexterity.
        /// </summary>
        [DataMember(Name = "dwDex")]
        public int Dexterity { get; set; }

        /// <summary>
        /// Gets or sets the mover intelligence.
        /// </summary>
        [DataMember(Name = "dwInt")]
        public int Intelligence { get; set; }

        /// <summary>
        /// Gets or sets the mover hit rate.
        /// </summary>
        [DataMember(Name = "dwHR")]
        public int HitRating { get; set; }

        /// <summary>
        /// Gets or sets the mover escape rate.
        /// </summary>
        [DataMember(Name = "dwER")]
        public int EscapeRating { get; set; }

        /// <summary>
        /// Gets or sets the mover class.
        /// </summary>
        [DataMember(Name = "dwClass")]
        public MoverClassType Class { get; set; }

        /// <summary>
        /// Gets or sets the mover natural armor.
        /// </summary>
        [DataMember(Name = "dwNaturealArmor")]
        public int NaturalArmor { get; set; }

        /// <summary>
        /// Gets or sets the mover magic resistance.
        /// </summary>
        [DataMember(Name = "dwResisMagic")]
        public int MagicResitance { get; set; }

        /// <summary>
        /// Gets or sets the mover attack delay.
        /// </summary>
        [DataMember(Name = "dwReAttackDelay")]
        public int ReAttackDelay { get; set; }
        
        /// <summary>
        /// Gets or sets the mover attack speed.
        /// </summary>
        [DataMember(Name = "dwAttackSpeed")]
        public int AttackSpeed { get; set; }

        /// <summary>
        /// Gets or sets the monster correction value.
        /// </summary>
        [DataMember(Name = "dwCorrectionValue")]
        public int CorrectionValue { get; set; }

        /// <summary>
        /// Gets or sets the amount of experience given when the mover dies.
        /// </summary>
        [DataMember(Name = "dwExpValue")]
        public long Experience { get; set; }

        /// <summary>
        /// Gets or sets the monster element type.
        /// </summary>
        [DataMember(Name = "eElementType")]
        public ElementType Element { get; set; }

        /// <summary>
        /// Gets or sets the mover's resitance to electricity.
        /// </summary>
        [DataMember(Name = "fResistElecricity")]
        public float ElectricityResistance { get; set; }

        /// <summary>
        /// Gets or sets the mover's resistance to fire.
        /// </summary>
        [DataMember(Name = "fResistFire")]
        public float FireResitance { get; set; }

        /// <summary>
        /// Gets or sets the mover's resitance to wind.
        /// </summary>
        [DataMember(Name = "fResistWind")]
        public float WindResistance { get; set; }

        /// <summary>
        /// Gets or sets the mover's resitance to water.
        /// </summary>
        [DataMember(Name = "fResistWater")]
        public float WaterResistance { get; set; }

        /// <summary>
        /// Gets or sets the mover's resistance to earth.
        /// </summary>
        [DataMember(Name = "fResistEarth")]
        public float EarthResistance { get; set; }

        /// <summary>
        /// Gets or sets the minimal amount of gold dropped when the mover dies.
        /// </summary>
        [IgnoreDataMember]
        public int DropGoldMin { get; set; }

        /// <summary>
        /// Gets or sets the maximal amount of gold dropped when the mover dies.
        /// </summary>
        [IgnoreDataMember]
        public int DropGoldMax { get; set; }

        /// <summary>
        /// Gets or sets the maximal amount of items dropped when the mover dies.
        /// </summary>
        [IgnoreDataMember]
        public int MaxDropItem { get; set; }

        /// <summary>
        /// Gets or sets the collection of items the mover can drop.
        /// </summary>
        [IgnoreDataMember]
        public ICollection<DropItemData> DropItems { get; }

        /// <summary>
        /// Gets or sets the collection of item kinds the mover can drop.
        /// </summary>
        [IgnoreDataMember]
        public ICollection<DropItemKindData> DropItemsKind { get; }
        
        /// <summary>
        /// Creates a new <see cref="MoverData"/> instance.
        /// </summary>
        public MoverData()
        {
            DropItems = new List<DropItemData>();
            DropItemsKind = new List<DropItemKindData>();
        }

        /// <inheritdoc />
        public override string ToString() => Name;
    }
}
