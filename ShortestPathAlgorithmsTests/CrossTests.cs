using System.Collections.Generic;
using System.Linq;
using Graphs.Factories;
using ShortestPathAlgorithms.Algorithms;
using Xunit;

namespace ShortestPathAlgorithmsTests;

public class CrossTests
{
    [Fact]
    public void BasicGraphTest()
    {
        // Arrange
        var graph = DirectedGraphFactory.CreateBasicGraph();
        var startEndNodePairs = GetAllPossiblePairs(graph.Nodes);
        var distancesBruteForce = new List<int>(startEndNodePairs.Count);
        var distancesDijkstra = new List<int>(startEndNodePairs.Count);

        // Act
        foreach (var (start, end) in startEndNodePairs)
        {
            distancesBruteForce.Add(DepthFirstBruteForce.FindShortestPath(graph, start, end).Distance);
            distancesDijkstra.Add(Dijkstra.Find(graph, start, end).Distance);
        }
        
        // Assert
        Assert.Equal(distancesDijkstra, distancesBruteForce);
    }

    private static IList<(T, T)> GetAllPossiblePairs<T>(IList<T> items)
    {
        var pairs = new List<(T, T)>();

        for (var i = 0; i < items.Count - 1; i++)
        {
            for (var j = i + 1; j < items.Count; j++)
            {
                pairs.Add((items[i], items[j]));
                pairs.Add((items[j], items[i]));
            }
        }
        
        return pairs;
    }
}