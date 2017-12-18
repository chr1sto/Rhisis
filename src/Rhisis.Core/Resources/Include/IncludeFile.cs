using System;
using Rhisis.Core.IO;
using System.Collections.Generic;
using System.IO;

namespace Rhisis.Core.Resources.Include
{
    public sealed class IncludeFile : IDisposable
    {
        private readonly TokenScanner _scanner;
        private readonly ICollection<IStatement> _statements;

        public IReadOnlyCollection<IStatement> Statements => this._statements as IReadOnlyCollection<IStatement>;

        public IncludeFile(string filePath)
        {
            this._scanner = new TokenScanner(filePath, @"([(){}=,;\n\r\t])");
            this._statements = new List<IStatement>();

            this.Read();
        }

        private void Read()
        {
            this._scanner.Read();

            string token = null;
            while ((token = this._scanner.GetToken()) != null)
            {
                if (token == "{")
                    this._statements.Add(this.ParseBlock());
                else if (token == "(")
                    this._statements.Add(this.ParseInstruction());
                else if (token == "=")
                    this._statements.Add(this.ParseVariable());
            }
        }

        private Block ParseBlock()
        {
            string token = null;
            var block = new Block()
            {
                Name = this._scanner.GetPreviousToken()
            };

            while ((token = this._scanner.GetToken()) != "}")
            {
                if (token == null)
                    break;
                if (token == "{")
                    block.AddStatement(this.ParseBlock());
                else if (token == "(")
                    block.AddStatement(this.ParseInstruction());
                else if (token == "=")
                    block.AddStatement(this.ParseVariable());
                else
                    block.AddUnknowStatement(token);
            }

            return block;
        }

        private Instruction ParseInstruction()
        {
            string parameter = null;
            var instruction = new Instruction()
            {
                Name = this._scanner.GetPreviousToken()
            };

            while ((parameter = this._scanner.GetToken()) != ")")
                instruction.AddParameter(parameter);

            this._scanner.GetToken();

            return instruction;
        }

        private Variable ParseVariable()
        {
            string variableName = this._scanner.GetPreviousToken();
            string variableValue = this._scanner.GetToken();
            string endDelimiter = this._scanner.GetToken();

            if (endDelimiter != ";")
                throw new InvalidDataException("Invalid variable format. Missing ';' for variable " + variableName);

            return new Variable(variableName, variableValue);
        }

        public void Dispose()
        {
            foreach (var statement in this._statements)
            {
                if (statement is IDisposable disposable)
                    disposable.Dispose();
            }

            this._statements.Clear();
            this._scanner.Dispose();
        }
    }
}
