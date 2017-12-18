using System;

namespace Rhisis.Core.Resources.Include
{
    public interface IStatement : IDisposable
    {
        string Name { get; }

        StatementType Type { get; }
    }
}
