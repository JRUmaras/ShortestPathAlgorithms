using System;
using System.IO;
using System.Linq;
using Graphs.Factories;
using ShortestPathAlgorithms.Algorithms;
using ShortestPathAlgorithms.CostCalculators;
using ShortestPathAlgorithms.Interfaces;
using ShortestPathAlgorithms.Models;
using Xunit;
using Graphs.Helpers;

namespace ShortestPathAlgorithmsTests.UseCaseTests
{
    public class TerrainWalkerTests
    {
        [Fact]
        public async void ConstantSpeedWalker_ShouldFindPath()
        {
            // Arrange
            var (graph, nodeToCoordinatesMap) = DirectedGraphFactory.CreateRectangularGrid(100, 100);
            
            var costCalc = new TerrainCostCalculator(nodeToCoordinatesMap);

            var fromNode = graph.Nodes.First(n => n.Id == "0");
            var toNode = graph.Nodes.First(n => n.Id == "10200");

            var startState = new State
            {
                Time = DateTime.UtcNow.Date
            };

            var outputFilepath = Path.Combine(Path.GetTempPath(), $"terrain_path_{DateTime.UtcNow.Ticks}.txt");

            // Act
            var path = DijkstraDynamic<IState>.Find(graph, fromNode, toNode, costCalc, startState);
            await NodesPrinter.PrintToFileWithCoordinatesAsync(path!.Nodes, outputFilepath, nodeToCoordinatesMap);

            // Assert
            Assert.NotNull(path);
            Assert.True(path!.Cost >= 0);
            Assert.True(File.Exists(outputFilepath));
        }
    }
}
