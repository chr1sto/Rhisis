using System.Runtime.Serialization;

namespace Rhisis.Core.Structures.Configuration.World
{
    /// <summary>
    /// Represents the death configuration section.
    /// </summary>
    [DataContract]
    public sealed class DeathConfiguration
    {
        /// <summary>
        /// Gets or sets a value that indicates if the death penality option is enabled.
        /// </summary>
        /// <remarks>
        /// When the death penality systme is activated, experience is removed when a player dies.
        /// </remarks>
        [DataMember]
        public bool DeathPenalityEnabled { get; set; } = true;
    }
}
