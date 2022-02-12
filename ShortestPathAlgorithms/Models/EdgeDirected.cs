using System;

namespace ShortestPathAlgorithms.Models;

public class EdgeDirected
{
    private readonly Lazy<string> _id;

    public string Id => _id.Value;
    
    public Node From { get; }
    
    public Node To { get; }
    
    public int Weight { get; }

    public EdgeDirected(Node from, Node to, int weight)
    {
        From = from;
        To = to;
        Weight = weight;

        _id = new Lazy<string>(() => $"{From.Id}->{To.Id}");
    }
}