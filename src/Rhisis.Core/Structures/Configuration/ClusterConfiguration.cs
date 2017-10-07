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
        [DataMember(Name = "id")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the cluster server name.
        /// </summary>
        [DataMember(Name = "name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the login protect state.
        /// </summary>
        [DataMember(Name = "enableLoginProtect")]
        public bool EnableLoginProtect { get; set; }

        /// <summary>
        /// Gets or sets the Inter-Server configuration.
        /// </summary>
        [DataMember(Name = "isc")]
        public ISCConfiguration ISC { get; set; }

        /// <summary>
        /// Gets or sets the default character configuration.
        /// </summary>
        [DataMember(Name = "defaultCharacter")]
        public DefaultCharacter DefaultCharacter { get; set; }

        /// <summary>
        /// Creates a new <see cref="ClusterConfiguration"/> instance.
        /// </summary>
        public ClusterConfiguration()
        {
            this.ISC = new ISCConfiguration();
        }
    }
}
