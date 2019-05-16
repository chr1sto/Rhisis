using System.Runtime.Serialization;

namespace Rhisis.Core.Structures.Game
{
    [DataContract]
    public class PenalityData
    {
        [DataMember]
        public int Level { get; set; }

        [DataMember]
        public decimal Value { get; set; }
    }
}
