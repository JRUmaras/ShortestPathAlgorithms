using System.Linq;
using ShortestPathAlgorithms.Algorithms;
using ShortestPathAlgorithmsTests.Fixtures;
using Xunit;

namespace ShortestPathAlgorithmsTests;

public class DepthFirstBruteForceDynamicTests : IClassFixture<SimpleDynamicGraphFixture>
{
    private readonly SimpleDynamicGraphFixture _fixture;

    public DepthFirstBruteForceDynamicTests(SimpleDynamicGraphFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public void DepthFirstBruteForceDynamic_Find_ShouldFindSolution()
    {
        // Arrange
        var graph = _fixture.Graph;
        var start = _fixture.StartNode;
        var end = _fixture.EndNode;

        var costCalc = _fixture.CostCalculator;

        var state = _fixture.StartState;

        // Act
        var path = DepthFirstBruteForceDynamic.Find(graph, start, end, costCalc, state);

        // Assert
        Assert.Equal(_fixture.ExpectedPath.Cost, path.Cost);
        Assert.Equal(_fixture.ExpectedPath.Nodes.Count, path.Nodes.Count);
        Assert.Equal(_fixture.ExpectedPath.Nodes.Select(n => n.Id), path.Nodes.Select(n => n.Id));
    }
}