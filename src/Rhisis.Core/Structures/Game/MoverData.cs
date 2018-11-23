using System.Runtime.Serialization;

namespace Rhisis.Core.Structures.Game
{
    /// <summary>
    /// Represents a Mover data structure from the propMover.txt resource file from the data.res.
    /// </summary>
    [DataContract]
    public class MoverData
    {
        [DataMember(Name = "dwID")]
        public int Id { get; private set; }

        [DataMember(Name = "szName")]
        public string Name { get; set; }

        [DataMember(Name = "dwAI")]
        public int AI { get; set; }

        [DataMember(Name = "dwBelligerence")]
        public int Belligerence { get; set; }

        [DataMember(Name = "fSpeed")]
        public float Speed { get; set; }

        [DataMember(Name = "dwAddHp")]
        public int AddHp { get; set; }

        [DataMember(Name = "dwAddMp")]
        public int AddMp { get; set; }

        [DataMember(Name = "dwLevel")]
        public int Level { get; set; }

        [DataMember(Name = "dwFilghtLevel")]
        public int FlightLevel { get; set; }

        public override string ToString() => this.Name;
    }
}
