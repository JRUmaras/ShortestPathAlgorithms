using System.Collections.Generic;
using System.Linq;
using Graphs.Interfaces;

namespace ShortestPathAlgorithms.Models;

public class Path<TCost>
{
    public List<INode> Nodes { get; }

    public TCost Cost { get; }

    public Path(IEnumerable<INode> nodes, TCost distance)
    {
        Nodes = nodes as List<INode> ?? nodes.ToList();
        Cost = distance;
    }
}