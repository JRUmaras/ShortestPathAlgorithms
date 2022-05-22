using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Graphs.Interfaces;
using Graphs.Models;
using ShortestPathAlgorithms.CostCalculators.Interfaces;
using ShortestPathAlgorithms.Helpers;
using ShortestPathAlgorithms.Models;

namespace ShortestPathAlgorithms.Algorithms;

/// <summary>
/// FIFO graph means that:
/// 1) Waiting at any state is not beneficial at any time
/// 2) Optimal paths are acyclic (i.e., they do not revisit nodes)
/// 3) Any subset of the optimal path is also a shortest path
/// </summary>
/// <typeparam name="TState">Object which defines state of graph traversal at a node (e.g. time at which node is reached).</typeparam>
// TODO Figure out a way to prevent cycles, i.e. path should not include the same node more than once
public class DijkstraDynamicNonFifo<TState>
{
    public static Path<double>? Find(GraphDirected graph, INode from, INode to, ICostCalculator<double, TState> costCalculator, TState startState)
    {
        var result = Search(from, to, graph, costCalculator, startState);

        if (!result.Keys.Select(k => k.Node).Contains(to)) return null;

        var path = Backtrack(to, result);

        return path;
    }

    private static IReadOnlyDictionary<(INode Node, TState State), ((INode Node, TState State)? Parent, double Cost)> Search(INode from, INode to, GraphDirected graph, ICostCalculator<double, TState> costCalculator, TState startState)
    {
        const int initialCost = 0;

        var childToParentMap = new Dictionary<(INode Node, TState State), ((INode Node, TState State)?, double Cost)>
        {
            { (from, startState), (null, initialCost) }
        };

        var initialPriorities = new List<((INode Node, TState State), double Priority)>
        {
            ((from, startState), initialCost)
        };

        var exploreQueue = new PriorityQueueNetWrapper<(INode Node, TState State), double>(initialPriorities);

        var visitedNodes = new Dictionary<(INode Node, TState State), ((INode Node, TState State)? Parent, double Cost)>();

        var queued = new HashSet<INode> { from };
        var dequeued = new HashSet<INode>();

        while (!exploreQueue.IsEmpty)
        {
            // In case of a disconnected graph, this helps us to break the loop
            // if all reachable nodes have been explored
            if (queued.Count == dequeued.Count) break;

            // Step to the node with shortest distance
            var (currentNodeStatePair, costAccumulated) = exploreQueue.Pop()  ?? throw new NullReferenceException("No nodes were found in the priority queue.");
            var currentNode = currentNodeStatePair.Node;
            var currentState = currentNodeStatePair.State;

            dequeued.Add(currentNode);

            // Early termination condition - we've arrived!
            if (currentNode == to)
            {
                visitedNodes.Add(currentNodeStatePair, childToParentMap[currentNodeStatePair]);
                break;
            }

            // Explore neighbours
            var edges = graph.FindOutgoingEdgesOfNode(currentNode);
            foreach (var edge in edges)
            {
                var (stepCost, newState) = costCalculator.Calculate(edge, currentState);
                
                if (visitedNodes.ContainsKey((edge.To, newState))) continue;
                
                var cost = costAccumulated + stepCost;
                Debug.Assert(cost >= costAccumulated, "Edge cost was negative.");

                (INode Node, TState State) nodeStatePair = (edge.To, newState);

                if (exploreQueue.TryGetPriority(nodeStatePair, out var costOld) && cost >= costOld) continue;

                queued.Add(nodeStatePair.Node);
                exploreQueue.PushOrUpdate(nodeStatePair, cost);
                childToParentMap[nodeStatePair] = (currentNodeStatePair, stepCost);
            }

            visitedNodes.Add(currentNodeStatePair, childToParentMap[currentNodeStatePair]);
        }

        return visitedNodes;
    }

    private static Path<double> Backtrack(INode endNode, IReadOnlyDictionary<(INode Node, TState State), ((INode Node, TState State)? Parent, double Cost)> pathTable)
    {
        var path = new Stack<INode>();

        // Find the node-state pair with lowest cost
        (INode Node, TState State)? currentNodeStatePair = null;
        var minCost = double.MaxValue;
        foreach (var (nodeStatePair, (_, cost)) in pathTable.Where(kv => kv.Key.Node == endNode))
        {
            if (cost > minCost) continue;
         
            currentNodeStatePair = nodeStatePair;
            minCost = cost;
        }

        var totalCost = 0d;
        do
        {
            path.Push(currentNodeStatePair!.Value.Node);
            var (parentNodeStatePair, stepCost) = pathTable[currentNodeStatePair.Value];
            currentNodeStatePair = parentNodeStatePair;
            totalCost += stepCost;
        } while (currentNodeStatePair is not null);

        return new Path<double>(path, totalCost);
    }
}