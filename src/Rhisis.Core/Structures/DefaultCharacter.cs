using Newtonsoft.Json;
using Rhisis.Core.Common;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace Rhisis.Core.Structures
{
    [DataContract]
    public class DefaultCharacter
    {
        public const int DefaultLevel = 1;
        public const int DefaultStrength = 15;
        public const int DefaultStamina = 15;
        public const int DefaultDexterity = 15;
        public const int DefaultIntelligence = 15;
        public const int DefaultMapId = 1;
        public const float DefaultPosX = 6968.0f;
        public const float DefaultPosY = 100.0f;
        public const float DefaultPosZ = 3328.0f;

        [DefaultValue(DefaultLevel)]
        [DataMember]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public int Level { get; set; } = DefaultLevel;

        [DefaultValue(0)]
        [DataMember]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public int Gold { get; set; }

        [DefaultValue(DefaultStrength)]
        [DataMember]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public int Strength { get; set; } = DefaultStrength;

        [DefaultValue(DefaultStamina)]
        [DataMember]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public int Stamina { get; set; } = DefaultStamina;

        [DefaultValue(DefaultDexterity)]
        [DataMember]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public int Dexterity { get; set; } = DefaultDexterity;

        [DefaultValue(DefaultIntelligence)]
        [DataMember]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public int Intelligence { get; set; } = DefaultIntelligence;

        [DefaultValue(DefaultMapId)]
        [DataMember]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public int MapId { get; set; } = DefaultMapId;

        [DefaultValue(DefaultPosX)]
        [DataMember]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public float PosX { get; set; } = DefaultPosX;

        [DefaultValue(DefaultPosY)]
        [DataMember]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public float PosY { get; set; } = DefaultPosY;

        [DefaultValue(DefaultPosZ)]
        [DataMember]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public float PosZ { get; set; } = DefaultPosZ;

        [DataMember]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public DefaultStartItems Man { get; set; } = new DefaultStartItems(GenderType.Male);

        [DataMember]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public DefaultStartItems Woman { get; set; } = new DefaultStartItems(GenderType.Female);
    }
}
