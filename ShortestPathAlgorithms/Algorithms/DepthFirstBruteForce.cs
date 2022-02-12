using System.Collections.Generic;
using System.Linq;
using ShortestPathAlgorithms.Models;

namespace ShortestPathAlgorithms.Algorithms;

public static class DepthFirstBruteForce
{
    public static Path FindShortestPath(GraphUndirected graph, Node start, Node end)
    {
        var currentPath = new Stack<Node>();
        currentPath.Push(start);

        var paths = new List<Path>();

        var edgeStack = new Stack<(Edge Edge, Node FromNode)>(graph.FindEdgesOfNode(start).Select(e => (e, start)));

        while (edgeStack.Count > 0)
        {
            var (edge, fromNode) = edgeStack.Pop();
            
            // Unwind the path if necessary
            while (currentPath.Peek().Id != fromNode.Id)
            {
                currentPath.Pop();
            }
                
            // Add node to path
            var newNode = edge.GetTheOtherNode(currentPath.Peek());
            currentPath.Push(newNode);
            
            // Handle the case if we reached the destination
            if (newNode.Id == end.Id)
            {
                var trail = currentPath.Reverse().ToList();
                var weight = graph.FindWeightOfTrail(trail);
                paths.Add(new Path(trail, weight));
                currentPath.Pop();
                continue;
            }
            
            // Queue all edges going from the newly added node, filtering out those involving nodes already in the path
            graph.FindEdgesOfNode(newNode)
                .Where(e => !currentPath.Select(n => n.Id).Contains(e.GetTheOtherNode(newNode).Id))
                .ToList()
                .ForEach(e => edgeStack.Push((e, newNode)));
        }

        return paths.OrderBy(p => p.Distance).First();
    }
}