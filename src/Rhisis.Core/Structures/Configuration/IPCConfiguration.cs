using System.Runtime.Serialization;

namespace Rhisis.Core.Structures.Configuration
{
    /// <summary>
    /// Represents the Inter-Server configuration structure.
    /// </summary>
    [DataContract]
    public class IPCConfiguration : BaseConfiguration
    {
        /// <summary>
        /// Get or sets the Inter-Server password.
        /// </summary>
        [DataMember(Name = "password")]
        public string Password { get; set; }
    }
}
