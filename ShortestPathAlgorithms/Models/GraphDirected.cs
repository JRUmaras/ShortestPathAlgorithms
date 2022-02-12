using System;
using System.Collections.Generic;
using System.Linq;

namespace ShortestPathAlgorithms.Models;

public class GraphDirected
{
    private readonly Lazy<Node[]> _nodes;

    public EdgeDirected[] Edges { get; }

    public Node[] Nodes => _nodes.Value;

    public GraphDirected(IEnumerable<EdgeDirected> edges)
    {
        if (edges is null) throw new ArgumentNullException($"Argument {nameof(edges)} cannot be null.");
        
        Edges = edges as EdgeDirected[] ?? edges.ToArray();
        _nodes = new Lazy<Node[]>(GetNodes);
    }

    public IEnumerable<EdgeDirected> FindOutgoingEdgesOfNode(Node node) 
        => Edges.Where(e => e.From.Id == node.Id);

    private Node[] GetNodes()
    {
        var hashset = new HashSet<Node>();
        foreach (var edge in Edges)
        {
            hashset.Add(edge.From);
            hashset.Add(edge.To);
        }

        return hashset.ToArray();
    }
}