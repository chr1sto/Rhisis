namespace Rhisis.Core.Resources.Include
{
    public interface IStatement
    {
        string Name { get; }

        StatementType Type { get; }
    }
}
