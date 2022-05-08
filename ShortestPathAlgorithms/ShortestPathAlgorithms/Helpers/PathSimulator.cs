using System.Collections.Generic;
using System.Linq;
using Graphs.Interfaces;
using ShortestPathAlgorithms.CostCalculators.Interfaces;
using ShortestPathAlgorithms.Interfaces;

namespace ShortestPathAlgorithms.Helpers;

public class PathSimulator
{
    public static IReadOnlyList<double> Simulate(IReadOnlyList<INode> path, IGraphDirected graph, ICostCalculator<double, IState> costCalculator, IState startState)
    {
        var stepCosts = new List<double>(path.Count - 1);

        for (var j = 1; j < path.Count; j++)
        {
            var edge = graph.Edges.First(e => e.From.Id == path[j - 1].Id && e.To.Id == path[j].Id);
            var (stepCost, newState) = costCalculator.Calculate(edge, startState);
            startState = newState;
            stepCosts.Add(stepCost);
        }

        return stepCosts;
    }
}