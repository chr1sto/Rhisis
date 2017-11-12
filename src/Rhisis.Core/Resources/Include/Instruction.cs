using System;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.Core.Resources.Include
{
    public class Instruction : IStatement, IDisposable
    {
        private readonly ICollection<object> _parameters;

        public string Name { get; internal set; }

        public IReadOnlyCollection<object> Parameters => this._parameters as IReadOnlyCollection<object>;

        public StatementType Type => StatementType.Instruction;

        public Instruction()
            : this(string.Empty, new List<object>())
        {
        }

        public Instruction(string name, ICollection<object> parameters)
        {
            this.Name = name;
            this._parameters = parameters;
        }

        internal void AddParameter(object parameter)
        {
            if (parameter.ToString() != ",")
                this._parameters.Add(parameter);
        }

        public void Dispose()
        {
            if (this._parameters.Any())
                this._parameters.Clear();
        }
    }
}
