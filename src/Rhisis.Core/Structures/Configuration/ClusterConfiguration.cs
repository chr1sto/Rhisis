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
        [DefaultValue(23000)]
        [Display(Name = "Cluster server listening port", Order = 1)]
        public int Port { get; set; }

        /// <summary>
        /// Gets or sets the cluster server id.
        /// </summary>
        [DataMember]
        [DefaultValue(1)]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the cluster server name.
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the login protect state.
        /// </summary>
        [DataMember]
        public bool EnableLoginProtect { get; set; }

        /// <summary>
        /// Gets or sets the Inter-Server configuration.
        /// </summary>
        [DataMember]
        public CoreConfiguration ISC { get; set; } = new CoreConfiguration();

        /// <summary>
        /// Gets or sets the default character configuration.
        /// </summary>
        [DataMember]
        public DefaultCharacter DefaultCharacter { get; set; } = new DefaultCharacter();
    }
}
