using System.Linq;
using Graphs.Factories;
using ShortestPathAlgorithms.Algorithms;
using Xunit;

namespace ShortestPathAlgorithmsTests;

public class BruteForceAlgorithmTests
{
    [Fact]
    public void Find_BasicGraph_ShouldFindSolution()
    {
        // Arrange
        var graph = DirectedGraphFactory.CreateBasicGraphWeighted();

        // Act
        var startNode = graph.Nodes.First(n => n.Id == "7");
        var endNode = graph.Nodes.First(n => n.Id == "6");
        var path = DepthFirstBruteForce.Find(graph, startNode, endNode);
        
        // Assert
        Assert.NotNull(path);
        Assert.Equal(4, path!.Cost);
        Assert.True(path.Nodes.Select(n => n.Id).SequenceEqual(new [] {"7", "3", "2", "4", "6"}));
    }

    [Fact]
    public void Find_DisconnectedGraph_ShouldFailToFindPath()
    {
        // Arrange
        var graph = DirectedGraphFactory.CreateDisconnectedGraphWeighted();

        // Act
        var startNode = graph.Nodes.First(n => n.Id == "0");
        var endNode = graph.Nodes.First(n => n.Id == "5");
        var path = DepthFirstBruteForce.Find(graph, startNode, endNode);
        
        // Assert
        Assert.Null(path);
    }

    [Fact]
    public void Find_UnreachableNode_ShouldFailToFindPath()
    {
        // Arrange
        var graph = DirectedGraphFactory.CreateGraphWeightedWithUnreachableNode();

        // Act
        var startNode = graph.Nodes.First(n => n.Id == "1");
        var endNode = graph.Nodes.First(n => n.Id == "0");
        var path = DepthFirstBruteForce.Find(graph, startNode, endNode);
        
        // Assert
        Assert.Null(path);
    }
}