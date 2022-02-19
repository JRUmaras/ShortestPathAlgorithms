using System.Linq;
using Graphs.Factories;
using ShortestPathAlgorithms.Algorithms;
using Xunit;

namespace ShortestPathAlgorithmsTests;

public class BruteForceAlgorithmTests
{
    [Fact]
    public void BasicTest()
    {
        // Arrange
        var graph = DirectedGraphFactory.CreateBasicGraph();

        // Act
        var startNode = graph.Nodes.First(n => n.Id == "7");
        var endNode = graph.Nodes.First(n => n.Id == "6");
        var path = DepthFirstBruteForce.FindShortestPath(graph, startNode, endNode);
        
        // Assert
        Assert.Equal(4, path.Distance);
        Assert.True(path.Nodes.Select(n => n.Id).SequenceEqual(new [] {"7", "3", "2", "4", "6"}));
    }
}