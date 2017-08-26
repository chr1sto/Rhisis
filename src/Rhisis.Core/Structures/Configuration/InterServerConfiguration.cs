using System.Runtime.Serialization;

namespace Rhisis.Core.Structures.Configuration
{
    /// <summary>
    /// Represents the Inter-Server configuration structure.
    /// </summary>
    [DataContract]
    public class InterServerConfiguration : BaseConfiguration
    {
        /// <summary>
        /// Get or sets the Inter-Server password.
        /// </summary>
        [DataMember(Name = "interPassword")]
        public string Password { get; set; }
    }
}
