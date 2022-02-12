using System.Collections.Generic;
using System.Linq;

namespace ShortestPathAlgorithms.Models;

public class GraphUndirected
{
    public Node[] Nodes { get; set; }
    
    public Edge[] Edges { get; set; }

    public List<Edge> FindEdgesOfNode(Node node) 
        => Edges
            .Where(edge => edge.Node1.Id == node.Id || edge.Node2.Id == node.Id)
            .ToList();

    public List<Node> FindNeighbourNodes(Node node) 
        => Edges
            .Where(edge => edge.HasNode(node))
            .Select(edge => edge.Node1.Id == node.Id ? edge.Node2 : edge.Node1)
            .ToList();

    public int FindWeightOfTrail(List<Node> nodes)
    {
        var weight = 0;
        for (var idx = 0; idx < nodes.Count - 1; idx++)
        {
            weight += Edges
                .First(e =>
                    (e.Node1.Id == nodes[idx].Id && e.Node2.Id == nodes[idx + 1].Id)
                    || (e.Node1.Id == nodes[idx + 1].Id && e.Node2.Id == nodes[idx].Id))
                .Weight;
            
        }

        return weight;
    }
}