using System;
using System.Collections.Generic;
using Graphs.Interfaces;
using ShortestPathAlgorithms.CostCalculators.Interfaces;
using ShortestPathAlgorithms.Interfaces;

namespace ShortestPathAlgorithms.CostCalculators
{
    internal class TimeCostCalculator : ICostCalculator<TimeSpan>
    {
        private readonly Dictionary<string, Func<DateTime, TimeSpan>> _edgeCostDictionary;

        public (TimeSpan Cost, IState EndState) Calculate(IEdgeDirected edgeDirected, IState startState)
        {
            var cost = _edgeCostDictionary[edgeDirected.Id].Invoke(startState.Time);
            startState.Time = startState.Time.Add(cost);
            return (cost, startState);
        }
    }
}
