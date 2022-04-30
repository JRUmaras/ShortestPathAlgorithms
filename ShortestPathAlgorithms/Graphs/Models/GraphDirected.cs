using System;
using System.Collections.Generic;
using System.Linq;
using Graphs.Interfaces;

namespace Graphs.Models;

public class GraphDirected : IGraphDirected
{
    private readonly Lazy<INode[]> _nodes;

    public IEdgeDirected[] Edges { get; }

    public INode[] Nodes => _nodes.Value;

    public IReadOnlyDictionary<INode, IReadOnlyList<IEdgeDirected>> OutgoingEdgesTable { get; }

    public GraphDirected(IEnumerable<IEdgeDirected> edges)
    {
        if (edges is null) throw new ArgumentNullException($"Argument '{nameof(edges)}' cannot be null.");
        
        Edges = edges as EdgeDirectedWeighted[] ?? edges.ToArray();
        _nodes = new Lazy<INode[]>(GetNodes);

        OutgoingEdgesTable = Edges
            .GroupBy(e => e.From)
            .ToDictionary(group => group.Key, group => (IReadOnlyList<IEdgeDirected>)group.ToList());
    }

    public IEnumerable<IEdgeDirected> FindOutgoingEdgesOfNode(INode node) 
        => OutgoingEdgesTable[node];

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