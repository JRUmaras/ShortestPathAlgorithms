using System;
using System.Collections.Generic;
using System.Linq;
using Graphs.Factories;
using Graphs.Interfaces;
using Graphs.Models;
using ShortestPathAlgorithms.CostCalculators;
using ShortestPathAlgorithms.CostCalculators.Interfaces;
using ShortestPathAlgorithms.Interfaces;
using ShortestPathAlgorithms.Models;

namespace ShortestPathAlgorithmsTests.Fixtures;

// Based on https://sboyles.github.io/teaching/ce392d/8-tdsp.pdf
public class SimpleDynamicGraphFixture
{
    public GraphDirected Graph { get; }

    public INode StartNode { get; }

    public INode EndNode { get; }

    public ICostCalculator<double> CostCalculator { get; }

    public IState StartState => new State
    {
        Time = DateTime.UtcNow.Date.AddHours(4)
    };

    public Path<double> ExpectedPath { get; }

    public SimpleDynamicGraphFixture()
    {
        Graph = DirectedGraphFactory.CreateGraphForTimeDependentDijkstraTestCase();
        StartNode = Graph.Nodes.First(node => node.Id == "1");
        EndNode = Graph.Nodes.First(node => node.Id == "4");
        
        var dict = new Dictionary<string, Func<DateTime, TimeSpan>>
        {
            ["1->2"] = time => TimeSpan.FromHours(2 + time.Hour),
            ["1->3"] = _ => TimeSpan.FromHours(10),
            ["2->3"] = time => TimeSpan.FromHours(Math.Max(8 - time.Hour / 2d, 1)),
            ["2->4"] = time => TimeSpan.FromHours(2 + time.Hour),
            ["3->4"] = _ => TimeSpan.FromHours(5)
        };
        CostCalculator = new TimeCostCalculator(dict);

        var nodes = new List<INode>
        {
            Graph.Nodes.First(node => node.Id == "1"),
            Graph.Nodes.First(node => node.Id == "2"),
            Graph.Nodes.First(node => node.Id == "3"),
            Graph.Nodes.First(node => node.Id == "4")
        }; 
        
        ExpectedPath = new Path<double>(nodes, 14);
    }
}