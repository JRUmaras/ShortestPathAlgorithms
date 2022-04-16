using System;
using System.Collections.Generic;
using System.Linq;
using Graphs.Interfaces;
using Graphs.Models;
using ShortestPathAlgorithms.CostCalculators.Interfaces;
using ShortestPathAlgorithms.Interfaces;
using ShortestPathAlgorithms.Models;
using PriorityQueue = ShortestPathAlgorithms.Helpers.PriorityQueue<Graphs.Interfaces.INode, double>;

namespace ShortestPathAlgorithms.Algorithms;

public static class DijkstraDynamic
{
    // TODO
    // * What if graph is not connected? We start at source node, explore sub-graph, and then we dequeue the next node from another sub-graph, but that seems wrong because that part of graph can't be reached from the source
    // * Compare DijkstraDynamic and Dijkstra. DijkstraDynamic is more generic, should be possible to process test cases of Dijkstra. Apply Unit tests of Dijkstra to DijkstraDynamic

    public static Path<double> Find(GraphDirected graph, INode from, INode to, ICostCalculator<double> costCalculator, IState startState)
    {
        var initialPriorities = graph
            .Nodes
            .Select(node => node.Equals(from) ? (node, 0d) : (node, double.MaxValue));
        var nodesToExplore = new PriorityQueue(initialPriorities);

        var result = Search(nodesToExplore, graph, costCalculator, startState);

        var path = Backtrack(to, result);

        return path;
    }

    private static Dictionary<INode, (INode? parent, double distance)> Search(PriorityQueue exploreQueue, GraphDirected graph, ICostCalculator<double> costCalculator, IState startState)
    {
        var visitedNodes = new Dictionary<INode, (INode? Parent, double Cost)>();
        var childToParentMap = new Dictionary<INode, INode>();
        var stateCache = new Dictionary<INode, IState>();

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
                if (cost >= exploreQueue.GetPriority(edge.To)) continue;
                exploreQueue.PushOrUpdate(edge.To, cost);
                childToParentMap[edge.To] = currentNode;
                stateCache[edge.To] = newState;
            }
            
            // Mark as visited
            var parentAndCost = childToParentMap.TryGetValue(currentNode, out var parent)
                ? (parent, distanceFromSource: costAccumulated)
                : (null, distanceFromSource: costAccumulated);
            visitedNodes.Add(currentNode, parentAndCost);
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