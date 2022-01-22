using System.Linq;
using ShortestPathAlgorithms.Algorithms;
using ShortestPathAlgorithms.Models;
using Xunit;

namespace ShortestPathAlgorithmsTests;

public class BruteForceAlgorithmTests
{
    [Fact]
    public void BasicTest()
    {
        // Arrange
        var graph = CreateBasicGraph();

        // Act
        var startNode = graph.Nodes.First(n => n.Id == "7");
        var endNode = graph.Nodes.First(n => n.Id == "6");
        var path = DepthFirstBruteForce.FindShortestPath(graph, startNode, endNode);
        
        // Assert
        Assert.Equal(4, path.Distance);
        Assert.True(path.Nodes.Select(n => n.Id).SequenceEqual(new [] {"7", "3", "2", "4", "6"}));
    }

    private static UndirectedGraph CreateBasicGraph()
    {
        var graph = new UndirectedGraph
        {
            Nodes = new[]
            {
                new Node { Id = "0" },
                new Node { Id = "1" },
                new Node { Id = "2" },
                new Node { Id = "3" },
                new Node { Id = "4" },
                new Node { Id = "5" },
                new Node { Id = "6" },
                new Node { Id = "7" },
            }
        };

        graph.Edges = new[]
        {
            new Edge
            {
                Node1 = graph.Nodes[0],
                Node2 = graph.Nodes[1],
                Weight = 1
            },
            new Edge
            {
                Node1 = graph.Nodes[0],
                Node2 = graph.Nodes[2],
                Weight = 1
            },
            new Edge
            {
                Node1 = graph.Nodes[1],
                Node2 = graph.Nodes[4],
                Weight = 1
            },
            new Edge
            {
                Node1 = graph.Nodes[1],
                Node2 = graph.Nodes[5],
                Weight = 1
            },
            new Edge
            {
                Node1 = graph.Nodes[2],
                Node2 = graph.Nodes[3],
                Weight = 1
            },
            new Edge
            {
                Node1 = graph.Nodes[2],
                Node2 = graph.Nodes[4],
                Weight = 1
            },
            new Edge
            {
                Node1 = graph.Nodes[3],
                Node2 = graph.Nodes[7],
                Weight = 1
            },
            new Edge
            {
                Node1 = graph.Nodes[4],
                Node2 = graph.Nodes[5],
                Weight = 1
            },
            new Edge
            {
                Node1 = graph.Nodes[4],
                Node2 = graph.Nodes[6],
                Weight = 1
            }
        };

        return graph;
    }
}