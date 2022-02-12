using System.Linq;
using ShortestPathAlgorithms.Algorithms;
using ShortestPathAlgorithms.Models;
using Xunit;

namespace ShortestPathAlgorithmsTests;

public class DijkstraTests
{
    [Fact]
    public void BasicTest()
    {
        // Arrange
        var graph = CreateBasicGraph();

        // Act
        var startNode = graph.Nodes.First(n => n.Id == "7");
        var endNode = graph.Nodes.First(n => n.Id == "6");
        var path = Dijkstra.Find(graph, startNode, endNode);
        
        // Assert
        Assert.Equal(4, path.Distance);
        Assert.True(path.Nodes.Select(n => n.Id).SequenceEqual(new [] {"7", "3", "2", "4", "6"}));
    }

    private static GraphDirected CreateBasicGraph()
    {
        var nodes = new[]
        {
            new Node("0"),
            new Node("1"),
            new Node("2"),
            new Node("3"),
            new Node("4"),
            new Node("5"),
            new Node("6"),
            new Node("7")
        };
            
        var edges = new[]
        {
            new EdgeDirected(nodes[0], nodes[1], 1),
            new EdgeDirected(nodes[0], nodes[2], 1),
            new EdgeDirected(nodes[1], nodes[4], 1),
            new EdgeDirected(nodes[1], nodes[5], 1),
            new EdgeDirected(nodes[2], nodes[3], 1),
            new EdgeDirected(nodes[2], nodes[4], 1),
            new EdgeDirected(nodes[3], nodes[7], 1),
            new EdgeDirected(nodes[4], nodes[5], 1),
            new EdgeDirected(nodes[4], nodes[6], 1),
            // reverse edges
            new EdgeDirected(nodes[1], nodes[0], 1),
            new EdgeDirected(nodes[2], nodes[0], 1),
            new EdgeDirected(nodes[4], nodes[1], 1),
            new EdgeDirected(nodes[5], nodes[1], 1),
            new EdgeDirected(nodes[3], nodes[2], 1),
            new EdgeDirected(nodes[4], nodes[2], 1),
            new EdgeDirected(nodes[7], nodes[3], 1),
            new EdgeDirected(nodes[5], nodes[4], 1),
            new EdgeDirected(nodes[6], nodes[4], 1)
        };

        var graph = new GraphDirected(edges);
        
        return graph;
    }
}