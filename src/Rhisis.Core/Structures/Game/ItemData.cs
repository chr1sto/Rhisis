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
    }
}
