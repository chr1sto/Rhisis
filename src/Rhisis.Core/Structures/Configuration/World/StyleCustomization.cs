using System.Runtime.Serialization;

namespace Rhisis.Core.Structures.Configuration.World
{
    /// <summary>
    /// Reprensents the Customization structure.
    /// </summary>
    [DataContract]
    public class StyleCustomization
    {
        public const int DefaultChangeFaceCost = 1000000;
        public const int DefaultChangeHairColorCost = 4000000;
        public const int DefaultChangeHairCost = 2000000;

        /// <summary>
        /// Gets or sets the costs for changing characters face.
        /// </summary>
        [DataMember(Name = "changeFaceCost")]
        public uint ChangeFaceCost { get; set; }

        /// <summary>
        /// Gets or sets the costs for changing characters hair.
        /// </summary>
        [DataMember(Name = "changeHairCost")]
        public uint ChangeHairCost { get; set; }

        /// <summary>
        /// Gets or sets the costs for changing characters hair color.
        /// </summary>
        [DataMember(Name = "changeHairColorCost")]
        public uint ChangeHairColorCost { get; set; }

        /// <summary>
        /// Creates a new <see cref="StyleCustomization"/> instance.
        /// </summary>
        public StyleCustomization()
        {
            this.ChangeFaceCost = DefaultChangeFaceCost;
            this.ChangeHairCost = DefaultChangeHairCost;
            this.ChangeHairColorCost = DefaultChangeHairColorCost;
        }
    }
}
