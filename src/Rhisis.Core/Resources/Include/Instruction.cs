using System;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.Core.Resources.Include
{
    public class Instruction : IStatement, IDisposable
    {
        private readonly char[] EscapeCharacters = new[] { '"' };

        private readonly ICollection<object> _parameters;

        /// <summary>
        /// Gets the instruction's name.
        /// </summary>
        public string Name { get; internal set; }

        /// <summary>
        /// Gets the instruction's parameters.
        /// </summary>
        public IReadOnlyCollection<object> Parameters => _parameters as IReadOnlyCollection<object>;

        /// <summary>
        /// Gets the statement type.
        /// </summary>
        public StatementType Type => StatementType.Instruction;

        /// <summary>
        /// Creates a new <see cref="Instruction"/> instance without any parameters.
        /// </summary>
        public Instruction()
            : this(string.Empty, new List<object>())
        {
        }

        /// <summary>
        /// Creates a new <see cref="Instruction"/> instance with a name and parameters.
        /// </summary>
        /// <param name="name">Instruction name</param>
        /// <param name="parameters">Parameters</param>
        public Instruction(string name, ICollection<object> parameters)
        {
            Name = name;
            _parameters = parameters;
        }

        /// <summary>
        /// Add a parameter to the instruction.
        /// </summary>
        /// <param name="parameter">Parameter</param>
        internal void AddParameter(object parameter)
        {
            if (parameter.ToString() != ",")
                _parameters.Add(parameter);
        }

        /// <summary>
        /// Gets the parameter at the given index and convert it into the given generic type.
        /// </summary>
        /// <typeparam name="T">Target type.</typeparam>
        /// <param name="parameterIndex">Instruction parameter index.</param>
        /// <returns></returns>
        public T GetParameter<T>(int parameterIndex)
        {
            if (parameterIndex < 0 || parameterIndex >= Parameters.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(parameterIndex), "The instruction parameter index is out of range.");
            }

            object parameter = Parameters.ElementAtOrDefault(parameterIndex);

            if (parameter is string)
                parameter = parameter.ToString().Trim(EscapeCharacters);

            if (typeof(T).IsEnum)
            {
                if (Enum.TryParse(typeof(T), parameter.ToString(), out object value))
                    return (T)value;
            }

            Type targetType = typeof(T);

            return (T)Convert.ChangeType(parameter, Nullable.GetUnderlyingType(targetType) ?? targetType);
        }
        
        /// <inheritdoc />
        public void Dispose()
        {
            if (_parameters.Any())
                _parameters.Clear();
        }

        /// <inheritdoc />
        public override string ToString() => $"{Name}({string.Join(", ", Parameters)})";
    }
}
