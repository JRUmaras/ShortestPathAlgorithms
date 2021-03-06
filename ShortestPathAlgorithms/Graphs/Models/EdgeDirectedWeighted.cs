using System;
using Graphs.Interfaces;

namespace Graphs.Models;

public class EdgeDirectedWeighted : IEdgeDirectedWeighted
{
    private readonly Lazy<string> _id;

    public string Id => _id.Value;
    
    public INode From { get; }
    
    public INode To { get; }
    
    public int Weight { get; }

    public EdgeDirectedWeighted(INode from, INode to, int weight)
    {
        From = from;
        To = to;
        Weight = weight;

        _id = new Lazy<string>(() => $"{From.Id}->{To.Id}");
    }

    public override string ToString() => Id;
}