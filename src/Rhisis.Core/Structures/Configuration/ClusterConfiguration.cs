using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Rhisis.Core.Structures.Configuration
{
    /// <summary>
    /// Represents the Cluster Server configuration structure.
    /// </summary>
    [DataContract]
    public class ClusterConfiguration
    {
        /// <summary>
        /// Gets or sets the host.
        /// </summary>
        [DataMember(Name = "host")]
        [DefaultValue("127.0.0.1")]
        [Display(Name = "Cluster server host address", Order = 0)]
        public string Host { get; set; }

        /// <summary>
        /// Gets or sets the port.
        /// </summary>
        [DataMember(Name = "port")]
        [DefaultValue(28000)]
        [Display(Name = "Cluster server listening port", Order = 1)]
        public int Port { get; set; }

        /// <summary>
        /// Gets or sets the cluster server id.
        /// </summary>
        [DataMember]
        [DefaultValue(1)]
        [Display(Name = "Cluster server unique id", Order = 2)]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the cluster server name.
        /// </summary>
        [DataMember]
        [DefaultValue("Rhisis")]
        [Display(Name = "Cluster server name", Order = 3)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the login protect state.
        /// </summary>
        [DataMember]
        [Display(Name = "Enable second password verification", Order = 4)]
        public bool EnableLoginProtect { get; set; }

        /// <summary>
        /// Gets or sets the default character configuration.
        /// </summary>
        [DataMember]
        public DefaultCharacter DefaultCharacter { get; set; } = new DefaultCharacter();
    }
}
