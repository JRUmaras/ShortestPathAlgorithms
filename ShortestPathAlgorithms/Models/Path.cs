using System.Collections.Generic;

namespace ShortestPathAlgorithms.Models;

public class Path
{
    public List<Node> Nodes { get; }

    public int Distance { get; }

    public Path(IEnumerable<Node> nodes, int distance)
    {
        Nodes = new List<Node>(nodes);
        Distance = distance;
    }
}