using System.ComponentModel;
using System.Runtime.Serialization;

namespace Rhisis.Core.Structures.Configuration
{
    /// <summary>
    /// Represents the Cluster Server configuration structure.
    /// </summary>
    [DataContract]
    public class ClusterConfiguration : BaseConfiguration
    {
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
        public ISCConfiguration ISC { get; set; } = new ISCConfiguration();

        /// <summary>
        /// Gets or sets the default character configuration.
        /// </summary>
        [DataMember]
        public DefaultCharacter DefaultCharacter { get; set; } = new DefaultCharacter();
    }
}
