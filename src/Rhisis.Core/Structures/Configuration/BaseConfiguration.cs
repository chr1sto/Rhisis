using System.Runtime.Serialization;

namespace Rhisis.Core.Structures.Configuration
{
    /// <summary>
    /// Reprensts the basic server configuration structure.
    /// </summary>
    [DataContract]
    public class BaseConfiguration
    {
        /// <summary>
        /// Gets or sets the host.
        /// </summary>
        [DataMember(Name = "host")]
        public string Host { get; set; }

        /// <summary>
        /// Gets or sets the port.
        /// </summary>
        [DataMember(Name = "port")]
        public int Port { get; set; }
    }
}
