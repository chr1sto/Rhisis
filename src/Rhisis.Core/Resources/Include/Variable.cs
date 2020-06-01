using System;

namespace Rhisis.Core.Resources.Include
{
    public class Variable : IStatement, IDisposable
    {
        public string Name { get; private set; }

        public object Value { get; private set; }

        public StatementType Type => StatementType.Variable;

        public Variable()
            : this(string.Empty, null)
        {
        }

        public Variable(string name, object value)
        {
            Name = name;
            Value = value;
        }

        public void Dispose()
        {
            Name = string.Empty;
            Value = null;
        }
    }
}
