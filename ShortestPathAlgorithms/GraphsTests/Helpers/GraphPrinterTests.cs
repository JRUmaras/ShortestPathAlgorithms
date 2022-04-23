using System;
using System.IO;
using Graphs.Factories;
using Graphs.Helpers;
using Xunit;

namespace GraphsTests.Helpers
{
    public class GraphPrinterTests
    {
        [Fact]
        public async void PrintToFileAsync_RectangularGridGraph_ShouldPrintToFile()
        {
            // Arrange
            var outputFilepath = Path.Combine(Path.GetTempPath(), $"rectangular_grid_graph_{DateTime.UtcNow.Ticks}.txt");
            var (rectangularGridGraph, _) = DirectedGraphFactory.CreateRectangularGrid(5, 5);

            // Act
            await GraphPrinter.PrintToFileAsync(rectangularGridGraph, outputFilepath);

            // Assert
            Assert.True(File.Exists(outputFilepath));
        }

        [Fact]
        public async void PrintToFileWithCoordinatesAsync_RectangularGridGraph_ShouldPrintToFile()
        {
            // Arrange
            var outputFilepath = Path.Combine(Path.GetTempPath(), $"rectangular_grid_graph_{DateTime.UtcNow.Ticks}.txt");
            var (rectangularGridGraph, nodeToCoordinatesMap) = DirectedGraphFactory.CreateRectangularGrid(5, 5);

            // Act
            await GraphPrinter.PrintToFileWithCoordinatesAsync(rectangularGridGraph, outputFilepath, nodeToCoordinatesMap);

            // Assert
            Assert.True(File.Exists(outputFilepath));
        }
    }
}
