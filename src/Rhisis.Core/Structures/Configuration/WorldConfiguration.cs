using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Rhisis.Core.Structures.Configuration
{
    /// <summary>
    /// Represents the World Server configuration structure.
    /// </summary>
    [DataContract]
    public class WorldConfiguration : BaseConfiguration
    {
        /// <summary>
        /// Gets or sets the world's id.
        /// </summary>
        [DataMember(Name = "id")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the parent cluster server id.
        /// </summary>
        [DataMember(Name = "clusterId")]
        public int ClusterId { get; set; }

        /// <summary>
        /// Gets or sets the world's name.
        /// </summary>
        [DataMember(Name = "name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the enabled or disabled systems of the world server.
        /// </summary>
        [DataMember(Name = "systems")]
        public IDictionary<string, bool> Systems { get; set; }

        /// <summary>
        /// Gets or sets the IPC configuration.
        /// </summary>
        [DataMember(Name = "ipc")]
        public IPCConfiguration IPC { get; set; }

        /// <summary>
        /// Creates a new <see cref="WorldConfiguration"/> instance.
        /// </summary>
        public WorldConfiguration()
        {
            this.Systems = new Dictionary<string, bool>();
            this.IPC = new IPCConfiguration();
        }
    }
}
