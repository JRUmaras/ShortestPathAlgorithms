namespace Graphs.Interfaces;

public interface IGraphDirected
{
    IEdgeDirected[] Edges { get; }

    INode[] Nodes { get; }
}