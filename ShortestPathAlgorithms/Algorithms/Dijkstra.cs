using System;
using System.Collections.Generic;
using System.Linq;
using Graphs.Models;
using Path = ShortestPathAlgorithms.Models.Path;
using PriorityQueue = ShortestPathAlgorithms.Helpers.PriorityQueue<Graphs.Models.Node, int>;

namespace ShortestPathAlgorithms.Algorithms;

public static class Dijkstra
{
    public static Path Find(GraphDirected graph, Node from, Node to)
    {
        var initialPriorities = graph
            .Nodes
            .Select(node => node == from ? (node, 0) : (node, int.MaxValue));
        var nodesToExplore = new PriorityQueue(initialPriorities);

        var result = Search(nodesToExplore, graph.FindOutgoingEdgesOfNode);

        var path = Backtrack(to, result);

        return path;
    }

    private static Dictionary<Node, (Node? parent, int distance)> Search(
        PriorityQueue exploreQueue, 
        Func<Node, IEnumerable<EdgeDirected>> getNodeEdges)
    {
        var visitedNodes = new Dictionary<Node, (Node? parent, int distance)>();
        var childToParentMap = new Dictionary<Node, Node>();

        while (!exploreQueue.IsEmpty)
        {
            // Step to the node with shortest distance
            var (currentNode, distanceFromSource) = exploreQueue.Pop() ?? throw new NullReferenceException("No nodes were found in the priority queue.");
            
            // Explore neighbours
            var edges = getNodeEdges(currentNode);
            foreach (var edge in edges)
            {
                if (visitedNodes.Keys.Contains(edge.To)) continue;

                var distance = distanceFromSource + edge.Weight;
                if (distance >= exploreQueue.GetPriority(edge.To)) continue;
                exploreQueue.PushOrUpdate(edge.To, distance);
                childToParentMap[edge.To] = currentNode;
            }
            
            // Mark as visited
            var parentAndDistance = childToParentMap.TryGetValue(currentNode, out var parent)
                ? (parent, distanceFromSource)
                : (null, distanceFromSource);
            visitedNodes.Add(currentNode, parentAndDistance);
        }

        return visitedNodes;
    }
    
    private static Path Backtrack(Node endNode, IReadOnlyDictionary<Node, (Node? Parent, int Distance)> pathTable)
    {
        var path = new Stack<Node>();
        var currentNode = endNode;
        do
        {
            path.Push(currentNode);
            currentNode = pathTable[currentNode].Parent;
        } while (currentNode is not null);

        return new Path(path, pathTable[endNode].Distance);
    }
}