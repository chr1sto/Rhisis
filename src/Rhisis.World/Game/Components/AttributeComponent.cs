using Rhisis.Core.Data;
using System.Collections.Generic;

namespace Rhisis.World.Game.Components
{
    public class AttributeComponent
    {
        private readonly IDictionary<DefineAttributes, int> _attributes;

        /// <summary>
        /// Gets or sets the attribute value matching the given key.
        /// </summary>
        /// <param name="attribute">Attribute key.</param>
        /// <returns>Attribute value.</returns>
        public int this[DefineAttributes attribute]
        {
            get => GetAttribute(attribute);
            set => _attributes[attribute] = value;
        }

        /// <summary>
        /// Creates a new <see cref="AttributeComponent"/> instance.
        /// </summary>
        public AttributeComponent()
        {
            _attributes = new Dictionary<DefineAttributes, int>();
        }

        /// <summary>
        /// Resets an attribute to a given value.
        /// </summary>
        /// <param name="attribute">Attribute to reset.</param>
        /// <param name="value">Attribute value.</param>
        public void ResetAttribute(DefineAttributes attribute, int value)
        {
            _attributes[attribute] = value;
        }

        /// <summary>
        /// Increases an attribute value.
        /// </summary>
        /// <param name="attribute">Attribute to increase.</param>
        /// <param name="increaseValue">Increase value.</param>
        public void IncreaseAttribute(DefineAttributes attribute, int increaseValue)
        {
            _attributes[attribute] += increaseValue;
        }

        /// <summary>
        /// Decrease an attribute value.
        /// </summary>
        /// <param name="attribute">Attribute to decrease.</param>
        /// <param name="decreaseValue">Decrease value.</param>
        public void DecreaseAttribute(DefineAttributes attribute, int decreaseValue)
        {
            _attributes[attribute] -= decreaseValue;
        }

        /// <summary>
        /// Gets the attribute value matching the attribute key.
        /// </summary>
        /// <param name="attribute">Attribute.</param>
        /// <returns>Attribute value.</returns>
        public int GetAttribute(DefineAttributes attribute)
        {
            return _attributes.TryGetValue(attribute, out int value) ? value : default;
        }
    }
}
