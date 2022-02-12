using System.Collections.Generic;
using System.Linq;

namespace ShortestPathAlgorithms.Models;

public class Path
{
    public List<Node> Nodes { get; }

    public int Distance { get; }

    public Path(IEnumerable<Node> nodes, int distance)
    {
        Nodes = nodes as List<Node> ?? nodes.ToList();
        Distance = distance;
    }
}