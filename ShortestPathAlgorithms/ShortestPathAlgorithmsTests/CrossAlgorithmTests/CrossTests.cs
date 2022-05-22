using System;
using System.Collections.Generic;
using System.Linq;
using Graphs.Factories;
using Graphs.Interfaces;
using Moq;
using ShortestPathAlgorithms.Algorithms;
using ShortestPathAlgorithms.CostCalculators.Interfaces;
using ShortestPathAlgorithms.Interfaces;
using ShortestPathAlgorithms.Models;
using Xunit;

namespace ShortestPathAlgorithmsTests.CrossAlgorithmTests;

public class CrossTests
{
    [Fact]
    public void DijkstraVsDepthFirstBruteForce_BasicGraphTest()
    {
        // Arrange
        var graph = DirectedGraphFactory.CreateBasicGraphWeighted();
        var startEndNodePairs = GetAllPossiblePairs(graph.Nodes);
        var distancesBruteForce = new List<int>(startEndNodePairs.Count);
        var distancesDijkstra = new List<int>(startEndNodePairs.Count);

        // Act
        foreach (var (start, end) in startEndNodePairs)
        {
            var bruteForceSolution = DepthFirstBruteForce.Find(graph, start, end) ?? throw new NullReferenceException("Brute force algorithm failed to find a solution.");
            var dijkstraSolution = Dijkstra.Find(graph, start, end) ?? throw new NullReferenceException("Dijkstra algorithm failed to find a solution.");
            distancesBruteForce.Add(bruteForceSolution.Cost);
            distancesDijkstra.Add(dijkstraSolution.Cost);
        }
        
        // Assert
        Assert.Equal(distancesDijkstra, distancesBruteForce);
    }

    [Fact]
    public void DijkstraDynamicVsDepthFirstBruteForceDynamic_BasicGraphTest()
    {
        // Arrange
        var graph = DirectedGraphFactory.CreateBasicGraph();
        var startEndNodePairs = GetAllPossiblePairs(graph.Nodes);

        var distancesBruteForce = new List<double>(startEndNodePairs.Count);
        var distancesDijkstra = new List<double>(startEndNodePairs.Count);
        var diffs = new List<double>(startEndNodePairs.Count);

        var costCalculatorMock = new Mock<ICostCalculator<double, IState>>();
        costCalculatorMock
            .Setup(x => x.Calculate(It.IsAny<IEdgeDirected>(), It.IsAny<IState>()))
            .Returns<IEdgeDirected, IState>((edge, state) =>
            {
                // Make sure that the cost is non-decreasing with time, else the condition of FIFO graph will be violated
                var cost = Math.Abs(edge.From.Id.GetHashCode() / (double)int.MaxValue) * state.Time.Hour + Math.Abs(edge.To.Id.GetHashCode() / (double)int.MaxValue);
                cost = Math.Abs(cost);
                var newState = new State { Time = state.Time.AddHours(cost) };
                return (cost, newState);
            });
        
        static IState InitStartState() => new State { Time = DateTime.UtcNow.Date };

        // Act
        foreach (var (start, end) in startEndNodePairs)
        {
            var bruteForcePath = DepthFirstBruteForceDynamic<IState>.Find(graph, start, end, costCalculatorMock.Object, InitStartState());
            var dijkstraPath = DijkstraDynamic<IState>.Find(graph, start, end, costCalculatorMock.Object, InitStartState());

            if (bruteForcePath is not null) distancesBruteForce.Add(bruteForcePath.Cost);
            if (dijkstraPath is not null) distancesDijkstra.Add(dijkstraPath.Cost);
        }
        
        // Assert
        Assert.Equal(startEndNodePairs.Count, distancesDijkstra.Count);
        Assert.Equal(startEndNodePairs.Count, distancesBruteForce.Count);

        Assert.True(distancesDijkstra.All(d => d >= 0));
        Assert.True(distancesBruteForce.All(d => d >= 0));

        for (var idx = 0; idx < startEndNodePairs.Count; idx++)
        {
            var diff = distancesBruteForce[idx] == distancesDijkstra[idx]
                ? 0
                : Math.Abs((distancesDijkstra[idx] - distancesBruteForce[idx]) / distancesBruteForce[idx]);
            diffs.Add(diff);
        }

        Assert.True(diffs.All(delta => delta < 1e-15));
    }

    [Fact]
    public void DijkstraDynamicVsDijkstraDynamicNonFifo_BasicFifoGraphTest()
    {
        // Arrange
        var graph = DirectedGraphFactory.CreateBasicGraph();
        var startEndNodePairs = GetAllPossiblePairs(graph.Nodes);

        var distancesBruteForce = new List<double>(startEndNodePairs.Count);
        var distancesDijkstra = new List<double>(startEndNodePairs.Count);
        var diffs = new List<double>(startEndNodePairs.Count);

        var costsSet = new HashSet<(IEdgeDirected Edge, IState StartState, IState EndState, double Cost, double k)>();
        
        var costCalculatorMock = new Mock<ICostCalculator<double, IState>>();
        costCalculatorMock
            .Setup(x => x.Calculate(It.IsAny<IEdgeDirected>(), It.IsAny<IState>()))
            .Returns<IEdgeDirected, IState>((edge, state) =>
            {
                // Make sure that the cost is non-decreasing with time, else the condition of FIFO graph will be violated
                var cost = Math.Abs(edge.From.Id.GetHashCode() / (double)int.MaxValue) * state.Time.Hour + Math.Abs(edge.To.Id.GetHashCode() / (double)int.MaxValue);
                cost = Math.Abs(cost);
                var newState = new State { Time = state.Time.AddHours(cost) };

                costsSet.Add((edge, state, newState, cost, Math.Abs(edge.From.Id.GetHashCode() / (double)int.MaxValue)));

                return (cost, newState);
            });
        
        static IState InitStartState() => new State { Time = DateTime.UtcNow.Date };

        // Act
        foreach (var (start, end) in startEndNodePairs)
        {
            var dijkstraNonFifo = DijkstraDynamicNonFifo<IState>.Find(graph, start, end, costCalculatorMock.Object, InitStartState());
            var dijkstraPath = DijkstraDynamic<IState>.Find(graph, start, end, costCalculatorMock.Object, InitStartState());

            if (dijkstraNonFifo is not null) distancesBruteForce.Add(dijkstraNonFifo.Cost);
            if (dijkstraPath is not null) distancesDijkstra.Add(dijkstraPath.Cost);
        }
        
        // Assert
        Assert.Equal(startEndNodePairs.Count, distancesDijkstra.Count);
        Assert.Equal(startEndNodePairs.Count, distancesBruteForce.Count);

        Assert.True(distancesDijkstra.All(d => d >= 0));
        Assert.True(distancesBruteForce.All(d => d >= 0));

        for (var idx = 0; idx < startEndNodePairs.Count; idx++)
        {
            var diff = distancesBruteForce[idx] == distancesDijkstra[idx]
                ? 0
                : Math.Abs((distancesDijkstra[idx] - distancesBruteForce[idx]) / distancesBruteForce[idx]);
            diffs.Add(diff);
        }

        Assert.True(diffs.All(delta => delta < 1e-15));
    }

    [Fact(Skip = "DijkstraDynamicNonFifo allows cycles for now, so this test MIGHT fail.")]
    public void DijkstraDynamicNonFifoVsDepthFirstBruteForceDynamic_BasicGraphTest()
    {
        // Arrange
        var graph = DirectedGraphFactory.CreateBasicGraph();
        var startEndNodePairs = GetAllPossiblePairs(graph.Nodes);

        var distancesBruteForce = new List<double>(startEndNodePairs.Count);
        var distancesDijkstra = new List<double>(startEndNodePairs.Count);
        var diffs = new List<double>(startEndNodePairs.Count);

        var costCalculatorMock = new Mock<ICostCalculator<double, IState>>();
        costCalculatorMock
            .Setup(x => x.Calculate(It.IsAny<IEdgeDirected>(), It.IsAny<IState>()))
            .Returns<IEdgeDirected, IState>((edge, state) =>
            {
                // Make sure that the cost is non-decreasing with time, else the condition of FIFO graph will be violated
                var cost = edge.From.Id.GetHashCode() / (double)int.MaxValue * state.Time.Hour + edge.To.Id.GetHashCode() / (double)int.MaxValue;
                cost = Math.Abs(cost);
                var newState = new State { Time = state.Time.AddHours(cost) };
                return (cost, newState);
            });
        
        static IState InitStartState() => new State { Time = DateTime.UtcNow.Date };

        // Act
        foreach (var (start, end) in startEndNodePairs)
        {
            var dijkstraNonFifo = DepthFirstBruteForceDynamic<IState>.Find(graph, start, end, costCalculatorMock.Object, InitStartState());
            var dijkstraPath = DijkstraDynamicNonFifo<IState>.Find(graph, start, end, costCalculatorMock.Object, InitStartState());

            if (dijkstraNonFifo is not null) distancesBruteForce.Add(dijkstraNonFifo.Cost);
            if (dijkstraPath is not null) distancesDijkstra.Add(dijkstraPath.Cost);
        }
        
        // Assert
        Assert.Equal(startEndNodePairs.Count, distancesDijkstra.Count);
        Assert.Equal(startEndNodePairs.Count, distancesBruteForce.Count);

        Assert.True(distancesDijkstra.All(d => d >= 0));
        Assert.True(distancesBruteForce.All(d => d >= 0));

        for (var idx = 0; idx < startEndNodePairs.Count; idx++)
        {
            var diff = distancesBruteForce[idx] == distancesDijkstra[idx]
                ? 0
                : Math.Abs((distancesDijkstra[idx] - distancesBruteForce[idx]) / distancesBruteForce[idx]);
            diffs.Add(diff);
        }

        // For debugging
        if (!diffs.All(delta => delta < 1e-15))
        {
            var errorCases = new List<int>();
            for (var idx = 0; idx < diffs.Count; idx++)
            {
                if (diffs[idx] > 1e-15) errorCases.Add(idx);
            }

            foreach (var errorCase in errorCases)
            {
                var (start, end) = startEndNodePairs[errorCase];

                var bruteForcePath = DepthFirstBruteForceDynamic<IState>.Find(graph, start, end, costCalculatorMock.Object, InitStartState());
                var dijkstraPath = DijkstraDynamicNonFifo<IState>.Find(graph, start, end, costCalculatorMock.Object, InitStartState());
            }
        }

        Assert.True(diffs.All(delta => delta < 1e-15));
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