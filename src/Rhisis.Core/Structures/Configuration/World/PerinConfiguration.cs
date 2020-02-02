using System.Runtime.Serialization;

namespace Rhisis.Core.Structures.Configuration.World
{
    /// <summary>
    /// Represents the perin configuration section.
    /// </summary>
    public class PerinConfiguration
    {
        public const int DefaultPerinValue = 100000000;

        /// <summary>
        /// Gets or sets perin value.
        /// </summary>
        [DataMember(Name = "perinValue")]
        public int PerinValue { get; set; }

        /// <summary>
        /// Creates a new <see cref="PerinConfiguration"/> instance.
        /// </summary>
        public PerinConfiguration()
        {
            PerinValue = DefaultPerinValue;
        }
    }
}
