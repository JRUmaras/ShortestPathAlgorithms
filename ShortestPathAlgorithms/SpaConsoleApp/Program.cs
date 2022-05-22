using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Graphs.Factories;
using Graphs.Helpers;
using ShortestPathAlgorithms.Algorithms;
using ShortestPathAlgorithms.CostCalculators;
using ShortestPathAlgorithms.Interfaces;
using ShortestPathAlgorithms.Models;

const int gridSize = 1_000;

var (graph, nodeToCoordinatesMap) = DirectedGraphFactory.CreateRectangularGrid(gridSize, gridSize);

var costCalc = new TerrainCostCalculator(nodeToCoordinatesMap);

var fromNode = graph.Nodes.First(n => n.Id == "0");
var toNode = graph.Nodes.First(n => n.Id == ((gridSize + 1) * (gridSize + 1) - 1).ToString());

var startState = new State
{
    Time = DateTime.UtcNow.Date
};

var sw = new Stopwatch();
sw.Start();
var path = DijkstraDynamic<IState>.Find(graph, fromNode, toNode, costCalc, startState);
sw.Stop();
Console.WriteLine($"Processing took: {sw.Elapsed.TotalSeconds} s");

//var outputFilepath = Path.Combine(Path.GetTempPath(), $"terrain_path_{DateTime.UtcNow.Ticks}.txt");
//await NodesPrinter.PrintToFileWithCoordinatesAsync(path!.Nodes, outputFilepath, nodeToCoordinatesMap);