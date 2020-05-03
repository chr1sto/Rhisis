using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Rhisis.Core.Structures.Configuration
{
    /// <summary>
    /// Represents the world cluster server configuration structure.
    /// </summary>
    [DataContract]
    public class WorldClusterConfiguration
    {
        /// <summary>
        /// Gets or sets the host.
        /// </summary>
        [DataMember(Name = "host")]
        [DefaultValue("127.0.0.1")]
        [Display(Name = "World cluster server host address", Order = 0)]
        public string Host { get; set; }

        /// <summary>
        /// Gets or sets the port.
        /// </summary>
        [DataMember(Name = "port")]
        [DefaultValue(28001)]
        [Display(Name = "World cluster listening port", Order = 1)]
        public int Port { get; set; }

        /// <summary>
        /// Get or sets the Inter-Server password.
        /// </summary>
        [DataMember(Name = "password")]
        [Display(Name = "World cluster server password", Order = 3)]
        [PasswordPropertyText]
        public string Password { get; set; }
    }
}