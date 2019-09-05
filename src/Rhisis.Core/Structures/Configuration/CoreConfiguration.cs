using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Rhisis.Core.Structures.Configuration
{
    /// <summary>
    /// Represents the Core server configuration structure.
    /// </summary>
    [DataContract]
    public class CoreConfiguration
    {
        /// <summary>
        /// Gets or sets the host.
        /// </summary>
        [DataMember(Name = "host")]
        [DefaultValue("127.0.0.1")]
        [Display(Name = "Core server host address", Order = 0)]
        public string Host { get; set; }

        /// <summary>
        /// Gets or sets the port.
        /// </summary>
        [DataMember(Name = "port")]
        [DefaultValue(23000)]
        [Display(Name = "Core server listening port", Order = 1)]
        public int Port { get; set; }

        /// <summary>
        /// Get or sets the Inter-Server password.
        /// </summary>
        [DataMember(Name = "password")]
        [Display(Name = "Core server password", Order = 3)]
        [PasswordPropertyText]
        public string Password { get; set; }
    }
}
