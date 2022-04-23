using Graphs.Interfaces;
using ShortestPathAlgorithms.CostCalculators.Interfaces;
using ShortestPathAlgorithms.Interfaces;
using ShortestPathAlgorithms.Models;

namespace ShortestPathAlgorithms.CostCalculators;

public class UnitCostCalculator : ICostCalculator<double>
{
    public (double Cost, IState EndState) Calculate(IEdgeDirected edgeDirected, IState startState)
    {
        var cost = System.TimeSpan.FromHours(1);
        var newState = new State
        {
            Time = startState.Time.Add(cost)
        };
        return (cost.TotalHours, newState);
    }
}