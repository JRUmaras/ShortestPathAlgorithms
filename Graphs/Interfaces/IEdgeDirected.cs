namespace Graphs.Interfaces
{
    public interface IEdgeDirected
    {
        string Id { get; }

        INode From { get; }

        INode To { get; }
    }
}
