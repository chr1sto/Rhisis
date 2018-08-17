using System;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.Core.Resources.Include
{
    public class Block : IStatement, IDisposable
    {
        private readonly ICollection<IStatement> _statements;
        private readonly ICollection<string> _unknownStatements;

        public string Name { get; set; }

        public StatementType Type => StatementType.Block;

        public IReadOnlyCollection<IStatement> Statements => this._statements as IReadOnlyCollection<IStatement>;

        public IReadOnlyCollection<string> UnknownStatements => this._unknownStatements as IReadOnlyCollection<string>;

        public Block this[string blockName] => this.GetBlockByName(blockName);

        public Block()
            : this(string.Empty)
        {
        }

        public Block(string name)
        {
            this.Name = name;
            this._statements = new List<IStatement>();
            this._unknownStatements = new List<string>();
        }

        public Block GetBlockByName(string name) => this.GetStatement(name, StatementType.Block) as Block;

        public Instruction GetInstruction(string name) => this.GetStatement(name, StatementType.Instruction) as Instruction;

        public Variable GetVariable(string name) => this.GetStatement(name, StatementType.Variable) as Variable;

        private IStatement GetStatement(string name, StatementType type) => this._statements.FirstOrDefault(x => x.Name == name && x.Type == type);

        internal void AddStatement(IStatement statement) => this._statements.Add(statement);

        internal void AddUnknownStatement(string statement) => this._unknownStatements.Add(statement);

        public void Dispose()
        {
            if (this._statements.Any())
                this._statements.Clear();
        }
    }
}
