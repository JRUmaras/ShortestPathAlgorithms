using System;
using System.Collections.Generic;
using System.Linq;
using Graphs.Interfaces;

namespace Graphs.Models;

public class GraphDirectedWeighted
{
    private readonly Lazy<INode[]> _nodes;

    public IEdgeDirectedWeighted[] Edges { get; }

    public INode[] Nodes => _nodes.Value;

    public GraphDirectedWeighted(IEnumerable<IEdgeDirectedWeighted> edges)
    {
        if (edges is null) throw new ArgumentNullException($"Argument {nameof(edges)} cannot be null.");
        
        Edges = edges as IEdgeDirectedWeighted[] ?? edges.ToArray();
        _nodes = new Lazy<INode[]>(GetNodes);
    }

    public IEnumerable<IEdgeDirectedWeighted> FindOutgoingEdgesOfNode(INode node) 
        => Edges.Where(e => e.From.Id == node.Id);

    private INode[] GetNodes()
    {
        var hashset = new HashSet<INode>();
        foreach (var edge in Edges)
        {
            hashset.Add(edge.From);
            hashset.Add(edge.To);
        }

        return hashset.ToArray();
    }
}