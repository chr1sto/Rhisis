using Rhisis.Core.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rhisis.World.Game.Components
{
    public class AttributeComponent
    {
        private readonly IDictionary<DefineAttributes, int> _attributes;

        public int this[DefineAttributes attribute] => this.GetAttribute(attribute);

        public AttributeComponent()
        {
            this._attributes = new Dictionary<DefineAttributes, int>();
        }

        public void ResetAttribute(DefineAttributes attribute, int value)
        {
            this._attributes[attribute] = value;
        }

        public void IncreaseAttribute(DefineAttributes attribute, int increaseValue)
        {
            this._attributes[attribute] += increaseValue;
        }

        public void DecreaseAttribute(DefineAttributes attribute, int decreaseValue)
        {
            this._attributes[attribute] -= decreaseValue;
        }

        public int GetAttribute(DefineAttributes attribute)
        {
            return this._attributes.TryGetValue(attribute, out int value) ? value : throw new KeyNotFoundException();
        }
    }
}
