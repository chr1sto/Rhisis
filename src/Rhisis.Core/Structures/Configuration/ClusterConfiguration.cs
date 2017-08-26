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
        /// Gets or sets the Inter-Server configuration.
        /// </summary>
        [DataMember(Name = "interServer")]
        public InterServerConfiguration InterServer { get; set; }

        /// <summary>
        /// Creates a new <see cref="ClusterConfiguration"/> instance.
        /// </summary>
        public ClusterConfiguration()
        {
            this.InterServer = new InterServerConfiguration();
        }
    }
}
