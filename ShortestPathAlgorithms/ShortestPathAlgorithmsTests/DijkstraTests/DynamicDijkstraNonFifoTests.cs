using Graphs.Factories;
using Graphs.Interfaces;
using Graphs.Models;
using Moq;
using ShortestPathAlgorithms.Algorithms;
using ShortestPathAlgorithms.CostCalculators.Interfaces;
using ShortestPathAlgorithms.Interfaces;
using ShortestPathAlgorithms.Models;
using ShortestPathAlgorithmsTests.Fixtures;
using System;
using System.Linq;
using ShortestPathAlgorithms.Helpers;
using Xunit;

namespace ShortestPathAlgorithmsTests.DijkstraTests;

public class DynamicDijkstraNonFifoTests : IClassFixture<SimpleDynamicGraphFixture>
{
    private readonly SimpleDynamicGraphFixture _fixture;

    public DynamicDijkstraNonFifoTests(SimpleDynamicGraphFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public void Find_BaseCase_ShouldFindSolution()
    {
        // Arrange
        var graph = _fixture.Graph;
        var start = _fixture.StartNode;
        var end = _fixture.EndNode;

        var costCalc = _fixture.CostCalculator;

        var state = _fixture.StartState;

        // Act
        var path = DijkstraDynamicNonFifo<IState>.Find(graph, start, end, costCalc, state);

        // Assert
        Assert.NotNull(path);
        Assert.Equal(_fixture.ExpectedPath.Cost, path!.Cost);
        Assert.Equal(_fixture.ExpectedPath.Nodes.Count, path.Nodes.Count);
        Assert.Equal(_fixture.ExpectedPath.Nodes.Select(n => n.Id), path.Nodes.Select(n => n.Id));
    }

    [Fact]
    public void Find_BasicGraphWithFixedWeights_ShouldFindSolution()
    {
        // Arrange
        var graph = DirectedGraphFactory.CreateBasicGraph();
        var costCalculator = UnitCostCalculatorMock().Object;

        // Act
        var startNode = graph.Nodes.First(n => n.Id == "7");
        var endNode = graph.Nodes.First(n => n.Id == "6");
        var path = DijkstraDynamicNonFifo<IState>.Find(graph, startNode, endNode, costCalculator, _fixture.StartState);
        
        // Assert
        Assert.NotNull(path);
        Assert.Equal(4, path!.Cost);
        Assert.True(path.Nodes.Select(n => n.Id).SequenceEqual(new [] {"7", "3", "2", "4", "6"}));
    }

    [Fact]
    public void Find_DisconnectedGraphWithFixedWeights_ShouldFailToFindPath()
    {
        // Arrange
        var graph = DirectedGraphFactory.CreateDisconnectedGraph();
        var costCalculator = UnitCostCalculatorMock().Object;

        // Act
        var startNode = graph.Nodes.First(n => n.Id == "0");
        var endNode = graph.Nodes.First(n => n.Id == "5");
        var path = DijkstraDynamicNonFifo<IState>.Find(graph, startNode, endNode, costCalculator, _fixture.StartState);
        
        // Assert
        Assert.Null(path);
    }

    [Fact]
    public void Find_UnreachableNodeWithFixedWeights_ShouldFailToFindPath()
    {
        // Arrange
        var graph = DirectedGraphFactory.CreateGraphWithUnreachableNode();
        var costCalculator = UnitCostCalculatorMock().Object;

        // Act
        var startNode = graph.Nodes.First(n => n.Id == "1");
        var endNode = graph.Nodes.First(n => n.Id == "0");
        var path = DijkstraDynamicNonFifo<IState>.Find(graph, startNode, endNode, costCalculator, _fixture.StartState);
        
        // Assert
        Assert.Null(path);
    }

    [Fact]
    // Non-FIFO links mean that sub-paths are not optimal themselves
    public void Find_NonFifoLinks_ShouldFindSolution()
    {
        // Arrange
        var nodes = new[]
        {
            new Node("0"),
            new Node("1"),
            new Node("2"),
            new Node("3")
        };
        var edges = new[]
        {
            new EdgeDirected(nodes[0], nodes[1]),
            new EdgeDirected(nodes[0], nodes[2]),
            new EdgeDirected(nodes[1], nodes[2]),
            new EdgeDirected(nodes[2], nodes[3]),
        };
        var graph = new GraphDirected(edges);

        var costCalculatorMock = new Mock<ICostCalculator<double, IState>>();
        costCalculatorMock
            .Setup(x => x.Calculate(It.IsAny<IEdgeDirected>(), It.IsAny<IState>()))
            .Returns<IEdgeDirected, IState>((edge, state) =>
            {
                var newState = new State { Time = state.Time.AddHours(1) };

                switch (edge.From.Id)
                {
                    case "0" when edge.To.Id == "1":
                    case "0" when edge.To.Id == "2":
                    case "1" when edge.To.Id == "2":
                    {
                        return (1, newState);
                    }

                    case "2" when edge.To.Id == "3":
                    {
                        var cost = state.Time.Hour == 1 ? 3 : 1;
                        return (cost, newState);
                    }

                    default:
                    {
                        throw new Exception("Unexpected edge in the test case.");
                    }
                }
            });

        var startNode = graph.Nodes.First(n => n.Id == "0");
        var endNode = graph.Nodes.First(n => n.Id == "3");
        var startState = new State { Time = DateTime.UtcNow.Date };

        // Act
        var path = DijkstraDynamicNonFifo<IState>.Find(graph, startNode, endNode, costCalculatorMock.Object, startState);

        // Assert
        Assert.NotNull(path);

        var calculatedPathCosts = PathSimulator.Simulate(path!.Nodes, graph, costCalculatorMock.Object, startState);
        var expectedPathCosts = PathSimulator.Simulate(nodes, graph, costCalculatorMock.Object, startState);
        Assert.Equal(calculatedPathCosts.Sum(), path.Cost);
        Assert.Equal(expectedPathCosts.Sum(), path.Cost);

        Assert.Equal(4, path.Nodes.Count);
        Assert.Equal(3, path.Cost);
        Assert.Equal(nodes, path.Nodes);
    }

    private static Mock<ICostCalculator<double, IState>> UnitCostCalculatorMock()
    {
        var costCalculatorMock = new Mock<ICostCalculator<double, IState>>();
        costCalculatorMock
            .Setup(x => x.Calculate(It.IsAny<IEdgeDirected>(), It.IsAny<IState>()))
            .Returns<IEdgeDirected, IState>((edge, state) => (1, new State { Time = state.Time }));
        return costCalculatorMock;
    }
}