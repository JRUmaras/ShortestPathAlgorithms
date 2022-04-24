using System;
using System.Collections.Generic;
using Graphs.Interfaces;
using ShortestPathAlgorithms.CostCalculators.Interfaces;
using ShortestPathAlgorithms.Interfaces;
using ShortestPathAlgorithms.Models;

namespace ShortestPathAlgorithms.CostCalculators;

public class TerrainCostCalculator : ICostCalculator<double, IState>
{
    private readonly IReadOnlyDictionary<INode, IPointCartesian> _nodeToCoordinatesMap;
    private const double _speedPerHour = 1;

    public TerrainCostCalculator(IReadOnlyDictionary<INode, IPointCartesian> nodeToCoordinatesMap)
    {
        _nodeToCoordinatesMap = nodeToCoordinatesMap;
    }

    public (double Cost, IState EndState) Calculate(IEdgeDirected edgeDirected, IState startState)
    {
        var coordinatesFrom = _nodeToCoordinatesMap[edgeDirected.From];
        var coordinatesTo = _nodeToCoordinatesMap[edgeDirected.To];

        var heightFrom = Height(coordinatesFrom);
        var heightTo = Height(coordinatesTo);

        var distance = Distance(coordinatesFrom, coordinatesTo);
        var dTime = TimeSpan.FromHours(distance / _speedPerHour);
        var newState = new State
        {
            Time = startState.Time.Add(dTime)
        };

        var cost = Math.Max(0, heightTo - heightFrom) + distance * 0.015;

        return (cost, newState);
    }

    public double Height(IPointCartesian point) => (Math.Cos(point.X / 10 + Math.PI / 2) * Math.Sin(point.Y) / 10 + 1) / 2;

    public double Distance(IPointCartesian from, IPointCartesian to)
    {
        var dX = to.X - from.X;
        var dY = to.Y - from.Y;

        return Math.Sqrt(dX * dX + dY * dY);
    }
}