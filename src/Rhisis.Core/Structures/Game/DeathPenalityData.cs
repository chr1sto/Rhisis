using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Rhisis.Core.Structures.Game
{
    [DataContract]
    public class DeathPenalityData
    {
        [DataMember]
        public IEnumerable<PenalityData> RevivalPenality { get; set; }

        [DataMember]
        public IEnumerable<PenalityData> DecExpPenality { get; set; }

        [DataMember]
        public IEnumerable<PenalityData> LevelDownPenality { get; set; }
    }
}
