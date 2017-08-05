using System.Runtime.Serialization;

namespace Rhisis.Core.Structures.Configuration
{
    [DataContract]
    public class InterServerConfiguration : BaseConfiguration
    {
        [DataMember(Name = "interPassword")]
        public string Password { get; set; }
    }
}
