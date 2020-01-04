using System;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.Core.Resources.Include
{
    public class Block : IStatement, IDisposable
    {
        private readonly ICollection<IStatement> _statements;
        private readonly ICollection<string> _unknownStatements;

        /// <summary>
        /// Gets or sets the block's name.
        /// </summary>
        public string Name { get; set; }

        /// <inheritdoc />
        public StatementType Type => StatementType.Block;

        /// <summary>
        /// Gets a collection of statements of the current block.
        /// </summary>
        public IReadOnlyCollection<IStatement> Statements => this._statements as IReadOnlyCollection<IStatement>;

        /// <summary>
        /// Gets a collection of unhandled statements.
        /// </summary>
        public IReadOnlyCollection<string> UnknownStatements => this._unknownStatements as IReadOnlyCollection<string>;

        /// <summary>
        /// Access an instruction block by its name.
        /// </summary>
        /// <param name="blockName">Block name.</param>
        /// <returns></returns>
        public Block this[string blockName] => this.GetBlockByName(blockName);

        /// <summary>
        /// Creates a new empty and unnamed <see cref="Block"/> instance.
        /// </summary>
        public Block()
            : this(string.Empty)
        {
        }

        /// <summary>
        /// Creates a new empty named <see cref="Block"/> instance.
        /// </summary>
        /// <param name="name">Block name.</param>
        public Block(string name)
        {
            this.Name = name;
            this._statements = new List<IStatement>();
            this._unknownStatements = new List<string>();
        }

        /// <summary>
        /// Gets an inner block instruction of the current block by its name.
        /// </summary>
        /// <param name="name">Block name.</param>
        /// <returns>Block.</returns>
        public Block GetBlockByName(string name) => this.GetStatement<Block>(name, StatementType.Block);

        /// <summary>
        /// Gets an instruction of the current block by its name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Instruction GetInstruction(string name) => this.GetStatement<Instruction>(name, StatementType.Instruction);

        /// <summary>
        /// Gets a collection of instructions by their names.
        /// </summary>
        /// <param name="name">Instruction names.</param>
        /// <returns></returns>
        public IEnumerable<Instruction> GetInstructions(string name) => this.GetStatements<Instruction>(name, StatementType.Instruction);

        /// <summary>
        /// Gets the instruction by its name and converts the parameter at a given index into a given type.
        /// </summary>
        /// <typeparam name="T">Target type.</typeparam>
        /// <param name="instructionName">Instruction name.</param>
        /// <param name="parameterIndex">Parameter index.</param>
        /// <returns></returns>
        public T GetInstructionParameter<T>(string instructionName, int parameterIndex)
        {
            Instruction instruction = this.GetInstruction(instructionName);

            if (instruction == null)
            {
                return default;
            }

            return instruction.GetParameter<T>(parameterIndex);
        }

        /// <summary>
        /// Gets a variable of the current block by its name.
        /// </summary>
        /// <param name="name">Variable name.</param>
        /// <returns></returns>
        public Variable GetVariable(string name) => this.GetStatement<Variable>(name, StatementType.Variable);

        /// <summary>
        /// Gets a statement by its name and type.
        /// </summary>
        /// <typeparam name="T">Statement type.</typeparam>
        /// <param name="name">Statement name.</param>
        /// <param name="type">Statement type identitifcation.</param>
        /// <returns></returns>
        private T GetStatement<T>(string name, StatementType type) where T : IStatement
            => (T)this._statements.FirstOrDefault(x => x.Name == name && x.Type == type);

        /// <summary>
        /// Gets a collection of statements by their name and type.
        /// </summary>
        /// <typeparam name="T">Statement type.</typeparam>
        /// <param name="name">Statement name.</param>
        /// <param name="type">Statement type identitifcation.</param>
        /// <returns></returns>
        private IEnumerable<T> GetStatements<T>(string name, StatementType type) where T : IStatement
            => this._statements.Where(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase) && x.Type == type).Cast<T>();

        /// <summary>
        /// Add a new statement to the current block.
        /// </summary>
        /// <param name="statement">Statement to add.</param>
        internal void AddStatement(IStatement statement) => this._statements.Add(statement);

        /// <summary>
        /// Adds a new unknown statement to the current block.
        /// </summary>
        /// <param name="statement">Unknown statement to add.</param>
        internal void AddUnknownStatement(string statement) => this._unknownStatements.Add(statement);

        /// <summary>
        /// Disposes the current resources.
        /// </summary>
        public void Dispose()
        {
            if (this._statements.Any())
                this._statements.Clear();
        }

        /// <inheritidoc />
        public override string ToString() => $"{this.Name}";
    }
}
