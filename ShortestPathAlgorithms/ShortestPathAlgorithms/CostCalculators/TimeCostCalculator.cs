using System;
using System.Collections.Generic;
using Graphs.Interfaces;
using ShortestPathAlgorithms.CostCalculators.Interfaces;
using ShortestPathAlgorithms.Interfaces;
using ShortestPathAlgorithms.Models;

namespace ShortestPathAlgorithms.CostCalculators
{
    public class TimeCostCalculator : ICostCalculator<double>
    {
        private readonly Dictionary<string, Func<DateTime, TimeSpan>> _edgeCostDictionary;

        public TimeCostCalculator(Dictionary<string, Func<DateTime, TimeSpan>> edgeCostDictionary)
        {
            _edgeCostDictionary = edgeCostDictionary;
        }

        public (double Cost, IState EndState) Calculate(IEdgeDirected edgeDirected, IState startState)
        {
            var cost = _edgeCostDictionary[edgeDirected.Id].Invoke(startState.Time);
            var newState = new State
            {
                Time = startState.Time.Add(cost)
            };
            return (cost.TotalHours, newState);
        }
    }
}
