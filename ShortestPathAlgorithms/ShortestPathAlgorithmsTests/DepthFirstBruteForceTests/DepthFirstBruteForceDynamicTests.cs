using System.Linq;
using Graphs.Factories;
using Graphs.Interfaces;
using Moq;
using ShortestPathAlgorithms.Algorithms;
using ShortestPathAlgorithms.CostCalculators.Interfaces;
using ShortestPathAlgorithms.Interfaces;
using ShortestPathAlgorithms.Models;
using ShortestPathAlgorithmsTests.Fixtures;
using Xunit;

namespace ShortestPathAlgorithmsTests.DepthFirstBruteForceTests;

public class DepthFirstBruteForceDynamicTests : IClassFixture<SimpleDynamicGraphFixture>
{
    private readonly SimpleDynamicGraphFixture _fixture;

    public DepthFirstBruteForceDynamicTests(SimpleDynamicGraphFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public void Find_BasicGraph_ShouldFindSolution()
    {
        // Arrange
        var graph = _fixture.Graph;
        var start = _fixture.StartNode;
        var end = _fixture.EndNode;

        var costCalc = _fixture.CostCalculator;

        var state = _fixture.StartState;

        // Act
        var path = DepthFirstBruteForceDynamic<IState>.Find(graph, start, end, costCalc, state);

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
        var path = DepthFirstBruteForceDynamic<IState>.Find(graph, startNode, endNode, costCalculator, _fixture.StartState);
        
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
        var path = DepthFirstBruteForceDynamic<IState>.Find(graph, startNode, endNode, costCalculator, _fixture.StartState);
        
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
        var path = DepthFirstBruteForceDynamic<IState>.Find(graph, startNode, endNode, costCalculator, _fixture.StartState);
        
        // Assert
        Assert.Null(path);
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