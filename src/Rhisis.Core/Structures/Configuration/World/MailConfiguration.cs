using System.Runtime.Serialization;

namespace Rhisis.Core.Structures.Configuration.World
{
    /// <summary>
    /// Represents the mail configuration section.
    /// </summary>
    public class MailConfiguration
    {
        public const int DefaultMailShippingCost = 500;

        /// <summary>
        /// Gets or sets mail shipping costs.
        /// </summary>
        [DataMember(Name = "mailShippingCost")]
        public uint MailShippingCost { get; set; }

        /// <summary>
        /// Creates a new <see cref="MailConfiguration"/> instance.
        /// </summary>
        public MailConfiguration()
        {
            this.MailShippingCost = DefaultMailShippingCost;
        }
    }
}
