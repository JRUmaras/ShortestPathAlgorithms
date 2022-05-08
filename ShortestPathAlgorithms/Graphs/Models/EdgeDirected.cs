using Graphs.Interfaces;

namespace Graphs.Models;

public class EdgeDirected : IEdgeDirected
{
    private string? _id;

    public string Id => _id ??= $"{From.Id}->{To.Id}";
    
    public INode From { get; }
    
    public INode To { get; }
    
    public EdgeDirected(INode from, INode to)
    {
        From = from;
        To = to;
    }

    public override string ToString() => Id;
}