using System.Collections.Generic;
using System.Linq;
using Graphs.Interfaces;
using Graphs.Models;
using ShortestPathAlgorithms.Models;

namespace ShortestPathAlgorithms.Algorithms;

public static class DepthFirstBruteForce
{
    public static Path<int> FindShortestPath(GraphDirected graph, INode start, INode end)
    {
        var currentPath = new Stack<INode>();
        currentPath.Push(start);

        var paths = new List<Path<int>>();

        var edgeStack = new Stack<IWeightedEdgeDirected>(graph.FindOutgoingEdgesOfNode(start));

        while (edgeStack.Count > 0)
        {
            var edge  = edgeStack.Pop();
            
            // Unwind the path if necessary
            while (currentPath.Peek() != edge.From) currentPath.Pop();
                
            // Add node to path
            var newNode = edge.To;
            currentPath.Push(newNode);
            
            // Handle the case if we reached the destination
            if (newNode.Id == end.Id)
            {
                var trail = currentPath.Reverse().ToList();

                var weight = 0;
                for (var index = 0; index < trail.Count - 1; index++)
                {
                    weight += graph.Edges
                        .First(e => e.From == trail[index] && e.To == trail[index + 1])
                        .Weight;
                }
                paths.Add(new Path<int>(trail, weight));
                currentPath.Pop();
                continue;
            }
            
            // Queue all edges going from the newly added node, filtering out those involving nodes already in the path
            graph.FindOutgoingEdgesOfNode(newNode)
                .Where(e => !currentPath.Contains(e.To))
                .ToList()
                .ForEach(e => edgeStack.Push(e));
        }

        return paths.OrderBy(p => p.Cost).First();
    }
}