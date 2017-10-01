using System.Runtime.Serialization;

namespace Rhisis.Core.Structures
{
    [DataContract]
    public class DefaultCharacter
    {
        [DataMember(Name = "level")]
        public int Level { get; set; }

        [DataMember(Name = "gold")]
        public int Gold { get; set; }

        [DataMember(Name = "strength")]
        public int Strength { get; set; }

        [DataMember(Name = "stamina")]
        public int Stamina { get; set; }

        [DataMember(Name = "dexterity")]
        public int Dexterity { get; set; }

        [DataMember(Name = "intelligence")]
        public int Intelligence { get; set; }

        [DataMember(Name = "mapId")]
        public int MapId { get; set; }

        [DataMember(Name = "posX")]
        public float PosX { get; set; }

        [DataMember(Name = "posY")]
        public float PosY { get; set; }

        [DataMember(Name = "posZ")]
        public float PosZ { get; set; }
        
        [DataMember(Name = "man")]
        public DefaultStartItem Man { get; set; }

        [DataMember(Name = "woman")]
        public DefaultStartItem Woman { get; set; }

        public DefaultCharacter()
        {
            this.Man = new DefaultStartItem();
            this.Woman = new DefaultStartItem();
        }
    }
}
