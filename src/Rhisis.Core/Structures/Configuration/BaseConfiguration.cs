using System.Runtime.Serialization;

namespace Rhisis.Core.Structures.Configuration
{
    [DataContract]
    public class BaseConfiguration
    {
        [DataMember(Name = "host")]
        public string Host { get; set; }

        [DataMember(Name = "port")]
        public int Port { get; set; }
    }
}
