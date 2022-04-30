﻿using System;
using System.IO;
using System.Linq;
using Graphs.Factories;
using Graphs.Helpers;
using ShortestPathAlgorithms.Algorithms;
using ShortestPathAlgorithms.CostCalculators;
using ShortestPathAlgorithms.Interfaces;
using ShortestPathAlgorithms.Models;

var (graph, nodeToCoordinatesMap) = DirectedGraphFactory.CreateRectangularGrid(100, 100);

var costCalc = new TerrainCostCalculator(nodeToCoordinatesMap);

var fromNode = graph.Nodes.First(n => n.Id == "0");
var toNode = graph.Nodes.First(n => n.Id == "10200");

var startState = new State
{
    Time = DateTime.UtcNow.Date
};

var path = DijkstraDynamic<IState>.Find(graph, fromNode, toNode, costCalc, startState);

//var outputFilepath = Path.Combine(Path.GetTempPath(), $"terrain_path_{DateTime.UtcNow.Ticks}.txt");
//await NodesPrinter.PrintToFileWithCoordinatesAsync(path!.Nodes, outputFilepath, nodeToCoordinatesMap);