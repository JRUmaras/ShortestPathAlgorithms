using System;
using System.Collections.Generic;
using System.Linq;
using Graphs.Interfaces;
using Graphs.Models;
using ShortestPathAlgorithms.CostCalculators.Interfaces;
using ShortestPathAlgorithms.Models;
using PriorityQueue = ShortestPathAlgorithms.Helpers.PriorityQueue<Graphs.Interfaces.INode, double>;

namespace ShortestPathAlgorithms.Algorithms;

public static class DijkstraDynamic<TState>
{
    public static Path<double>? Find(GraphDirected graph, INode from, INode to, ICostCalculator<double, TState> costCalculator, TState startState)
    {
        var initialPriorities = new List<(INode element, double priority)>
        {
            (from, 0)
        };
        var nodesToExplore = new PriorityQueue(initialPriorities);

        var result = Search(nodesToExplore, graph, costCalculator, startState, to);

        if (!result.ContainsKey(to)) return null;

        var path = Backtrack(to, result);

        return path;
    }

    private static Dictionary<INode, (INode? parent, double distance)> Search(PriorityQueue exploreQueue, GraphDirected graph, ICostCalculator<double, TState> costCalculator, TState startState, INode? to = null)
    {
        var visitedNodes = new Dictionary<INode, (INode? Parent, double Cost)>();
        var childToParentMap = new Dictionary<INode, INode>();
        var stateCache = new Dictionary<INode, TState>();

        while (!exploreQueue.IsEmpty)
        {
            // Step to the node with shortest distance
            var (currentNode, costAccumulated) = exploreQueue.Pop() ?? throw new NullReferenceException("No nodes were found in the priority queue.");
            if (!stateCache.TryGetValue(currentNode, out var state))
            {
                state = startState;
            }
            
            // Explore neighbours
            var edges = graph.FindOutgoingEdgesOfNode(currentNode);
            foreach (var edge in edges)
            {
                if (visitedNodes.Keys.Contains(edge.To)) continue;

                var (stepCost, newState) = costCalculator.Calculate(edge, state);
                var cost = costAccumulated + stepCost;
                if (exploreQueue.TryGetPriority(edge.To, out var costOld) && cost >= costOld) continue;
                exploreQueue.PushOrUpdate(edge.To, cost);
                childToParentMap[edge.To] = currentNode;
                stateCache[edge.To] = newState;
            }
            
            // Mark as visited
            var parentAndCost = childToParentMap.TryGetValue(currentNode, out var parent)
                ? (parent, distanceFromSource: costAccumulated)
                : (null, distanceFromSource: costAccumulated);
            visitedNodes.Add(currentNode, parentAndCost);

            if (to is not null && currentNode == to) break;
        }

        return visitedNodes;
    }
    
    private static Path<double> Backtrack(INode endNode, IReadOnlyDictionary<INode, (INode? Parent, double Cost)> pathTable)
    {
        var path = new Stack<INode>();
        var currentNode = endNode;
        do
        {
            path.Push(currentNode);
            currentNode = pathTable[currentNode].Parent;
        } while (currentNode is not null);

        return new Path<double>(path, pathTable[endNode].Cost);
    }
}