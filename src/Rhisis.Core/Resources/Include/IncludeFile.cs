using System;
using Rhisis.Core.IO;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Rhisis.Core.Resources.Include
{
    public sealed class IncludeFile : IDisposable
    {
        private readonly FileTokenScanner _scanner;
        private readonly ICollection<IStatement> _statements;

        public IReadOnlyCollection<IStatement> Statements => _statements as IReadOnlyCollection<IStatement>;

        public IncludeFile(string filePath, string regexPattern = @"([(){}=,;\n\r\t])")
        {
            _scanner = new FileTokenScanner(filePath, regexPattern);
            _statements = new List<IStatement>();

            Read();
        }

        private void Read()
        {
            _scanner.Read();

            string token = null;
            while ((token = _scanner.GetToken()) != null)
            {
                switch (token)
                {
                    case "{":
                        _statements.Add(ParseBlock());
                        break;
                    case "(":
                        _statements.Add(ParseInstruction());
                        break;
                    case "=":
                        _statements.Add(ParseVariable());
                        break;
                }
            }
        }

        private Block ParseBlock()
        {
            string token = null;
            var block = new Block()
            {
                Name = _scanner.GetPreviousToken()
            };
            
            while ((token = _scanner.GetToken()) != "}")
            {
                if (token == null)
                    break;
                switch (token)
                {
                    case "{":
                        block.AddStatement(ParseBlock());
                        break;
                    case "(":
                        block.AddStatement(ParseInstruction());
                        break;
                    case "=":
                        block.AddStatement(ParseVariable());
                        break;
                    default:
                        block.AddUnknownStatement(token);
                        break;
                }
            }

            return block;
        }

        private Instruction ParseInstruction()
        {
            string parameter = null;
            var instruction = new Instruction
            {
                Name = _scanner.GetPreviousToken()
            };

            while ((parameter = _scanner.GetToken()) != ")")
                instruction.AddParameter(parameter);

            if (_scanner.CurrentTokenIs(";"))
                _scanner.GetToken();

            return instruction;
        }

        private Variable ParseVariable()
        {
            string variableName = _scanner.GetPreviousToken();
            string variableValue = _scanner.GetToken();
            string endDelimiter = _scanner.GetToken();

            if (endDelimiter != ";")
                throw new InvalidDataException("Invalid variable format. Missing ';' for variable " + variableName);

            return new Variable(variableName, variableValue);
        }

        public Block GetBlock(string blockName) => _statements.FirstOrDefault(x => x.Type == StatementType.Block && x.Name.Equals(blockName)) as Block;

        public void Dispose()
        {
            foreach (IStatement statement in _statements)
            {
                statement.Dispose();
            }

            _statements.Clear();
            _scanner.Dispose();
        }
    }
}
