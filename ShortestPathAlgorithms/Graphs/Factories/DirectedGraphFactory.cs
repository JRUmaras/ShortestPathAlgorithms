using System.Collections.Generic;
using System.Linq;
using Graphs.Interfaces;
using Graphs.Models;

namespace Graphs.Factories;

/// <summary>
/// Hard-coded source of directed graphs with desired properties.
/// </summary>
public static class DirectedGraphFactory
{
    /// <summary>
    /// Creates a connected graph with bidirectional edges.
    /// </summary>
    /// <returns>Directed graph <see cref="GraphDirectedWeighted"/> with said properties.</returns>
    public static GraphDirectedWeighted CreateBasicGraphWeighted()
    {
        var edges = CreateEdgesOfBasicGraph();

        var graph = new GraphDirectedWeighted(edges);
        
        return graph;
    }

    /// <summary>
    /// Creates a graph consisting of two disconnected sub-graphs.
    /// Each of the sub-graphs are themselves connected and any node reachable
    /// from any other node. 
    /// </summary>
    /// <returns>Directed graph <see cref="GraphDirectedWeighted"/> with said properties.</returns>
    public static GraphDirectedWeighted CreateDisconnectedGraphWeighted()
    {
        var edges = CreateDisconnectedGraphEdges();

        var graph = new GraphDirectedWeighted(edges);
        
        return graph;
    }

    /// <summary>
    /// Creates a graph which has a node with only outgoing edges.
    /// All other nodes can be reached from any other node.
    /// </summary>
    /// <returns>Directed graph <see cref="GraphDirectedWeighted"/> with said properties.</returns>
    public static GraphDirectedWeighted CreateGraphWeightedWithUnreachableNode()
    {
        var edges = CreateEdgesGraphWithUnreachableNode();

        var graph = new GraphDirectedWeighted(edges);
        
        return graph;
    }

    /// <summary>
    /// Based on https://sboyles.github.io/teaching/ce392d/8-tdsp.pdf.
    /// </summary>
    /// <returns>Directed graph <see cref="GraphDirected"/>.</returns>
    public static GraphDirected CreateGraphForTimeDependentDijkstraTestCase()
    {
        var edges = CreateEdgesBasicGraph2();

        var graph = new GraphDirected(edges);
        
        return graph;
    }

    /// <summary>
    /// Creates a connected graph with bidirectional edges.
    /// </summary>
    /// <returns>Directed graph <see cref="GraphDirected"/>.</returns>
    public static GraphDirected CreateBasicGraph()
    {
        var edges = CreateEdgesOfBasicGraph();

        var graph = new GraphDirected(edges);
        
        return graph;
    }

    /// <summary>
    /// Creates a graph consisting of two disconnected sub-graphs.
    /// Each of the sub-graphs are themselves connected and any node reachable
    /// from any other node. 
    /// </summary>
    /// <returns>Directed graph <see cref="GraphDirected"/> with said properties.</returns>
    public static GraphDirected CreateDisconnectedGraph()
    {
        var edges = CreateDisconnectedGraphEdges();

        var graph = new GraphDirected(edges);
        
        return graph;
    }

    /// <summary>
    /// Creates a graph which has a node with only outgoing edges.
    /// All other nodes can be reached from any other node.
    /// </summary>
    /// <returns>Directed graph <see cref="GraphDirected"/> with said properties.</returns>
    public static GraphDirected CreateGraphWithUnreachableNode()
    {
        var edges = CreateEdgesGraphWithUnreachableNode();

        var graph = new GraphDirected(edges);
        
        return graph;
    }

    /// <summary>
    /// Creates a graph from a rectangular grid.
    /// Nodes are spaced in unit intervals in both x and y directions.
    /// Nodes are connected to nearest neighbors in x, y, and diagonal directions.
    /// Grid can be pictured as a rectangle placed in the first quadrant of coordinate plane,
    /// nodes are placed from left to right, bottom to top from the origin. So 0th node is
    /// at the origin, the last node is the furthest grid point from the origin.
    /// </summary>
    /// <param name="width">Width of the grid.</param>
    /// <param name="height">Height of the grid.</param>
    /// <returns></returns>
    public static (GraphDirected Graph, IReadOnlyDictionary<INode, IPointCartesian>) CreateRectangularGrid(int width, int height)
    {
        var nodes = Enumerable
            .Range(0, (width + 1) * (height + 1))
            .Select(id => (INode) new Node(id.ToString()))
            .ToArray();

        // Create edges to nearest nodes for each node, including diagonal edges
        var steps = new List<(int XStep, int YStep)>
        {
            (-1, 1),
            (0, 1),
            (1, 1),
            (1, 0)
        };

        int CoordinatesToIndex(int x, int y) => x + y * (width + 1);

        var edges = new List<IEdgeDirected>();
        var nodeToCoordinatesMap = new Dictionary<INode, IPointCartesian>();

        for (var y = 0; y <= height; y++)
        for (var x = 0; x <= width; x++)
        {
            var currentNodeIndex = CoordinatesToIndex(x, y);
            nodeToCoordinatesMap.Add(nodes[currentNodeIndex], new PointCartesian(x, y));

            foreach (var (xStep, yStep) in steps)
            {
                var x2 = x + xStep;
                var y2 = y + yStep;

                if (x2 < 0 || x2 > width || y2 > height) continue;

                var targetNodeIndex = CoordinatesToIndex(x2, y2);
                var edge = new EdgeDirected(nodes[currentNodeIndex], nodes[targetNodeIndex]);
                var edgeReverse = new EdgeDirected(nodes[targetNodeIndex], nodes[currentNodeIndex]);
                edges.Add(edge);
                edges.Add(edgeReverse);
            }
        }

        return (new GraphDirected(edges), nodeToCoordinatesMap);
    }

    private static EdgeDirectedWeighted[] CreateEdgesOfBasicGraph()
    {
        var nodes = new[]
        {
            new Node("0"),
            new Node("1"),
            new Node("2"),
            new Node("3"),
            new Node("4"),
            new Node("5"),
            new Node("6"),
            new Node("7")
        };

        var edges = new[]
        {
            new EdgeDirectedWeighted(nodes[0], nodes[1], 1),
            new EdgeDirectedWeighted(nodes[0], nodes[2], 1),
            new EdgeDirectedWeighted(nodes[1], nodes[4], 1),
            new EdgeDirectedWeighted(nodes[1], nodes[5], 1),
            new EdgeDirectedWeighted(nodes[2], nodes[3], 1),
            new EdgeDirectedWeighted(nodes[2], nodes[4], 1),
            new EdgeDirectedWeighted(nodes[3], nodes[7], 1),
            new EdgeDirectedWeighted(nodes[4], nodes[5], 1),
            new EdgeDirectedWeighted(nodes[4], nodes[6], 1),
            // reverse edges
            new EdgeDirectedWeighted(nodes[1], nodes[0], 1),
            new EdgeDirectedWeighted(nodes[2], nodes[0], 1),
            new EdgeDirectedWeighted(nodes[3], nodes[2], 1),
            new EdgeDirectedWeighted(nodes[4], nodes[1], 1),
            new EdgeDirectedWeighted(nodes[4], nodes[2], 1),
            new EdgeDirectedWeighted(nodes[5], nodes[1], 1),
            new EdgeDirectedWeighted(nodes[5], nodes[4], 1),
            new EdgeDirectedWeighted(nodes[6], nodes[4], 1),
            new EdgeDirectedWeighted(nodes[7], nodes[3], 1)
        };
        return edges;
    }

    private static EdgeDirectedWeighted[] CreateDisconnectedGraphEdges()
    {
        var nodes = new[]
        {
            new Node("0"),
            new Node("1"),
            new Node("2"),
            new Node("3"),
            new Node("4"),
            new Node("5")
        };

        var edges = new[]
        {
            // Sub-graph 1
            new EdgeDirectedWeighted(nodes[0], nodes[1], 1),
            new EdgeDirectedWeighted(nodes[1], nodes[2], 1),
            new EdgeDirectedWeighted(nodes[2], nodes[1], 1),
            // Sub-graph 2
            new EdgeDirectedWeighted(nodes[3], nodes[4], 1),
            new EdgeDirectedWeighted(nodes[4], nodes[5], 1),
            new EdgeDirectedWeighted(nodes[5], nodes[3], 1),
        };
        return edges;
    }

    private static EdgeDirectedWeighted[] CreateEdgesGraphWithUnreachableNode()
    {
        var nodes = new[]
        {
            new Node("0"),
            new Node("1"),
            new Node("2"),
            new Node("3")
        };

        var edges = new[]
        {
            // Unreachable node
            new EdgeDirectedWeighted(nodes[0], nodes[1], 1),
            // Reachable nodes
            new EdgeDirectedWeighted(nodes[1], nodes[2], 1),
            new EdgeDirectedWeighted(nodes[2], nodes[3], 1),
            new EdgeDirectedWeighted(nodes[3], nodes[1], 1)
        };
        return edges;
    }

    /// <summary>
    /// Based on https://sboyles.github.io/teaching/ce392d/8-tdsp.pdf.
    /// </summary>
    /// <returns>Array of directed edges <see cref="EdgeDirectedWeighted"/>.</returns>
    private static EdgeDirectedWeighted[] CreateEdgesBasicGraph2()
    {
        var nodes = new[]
        {
            new Node("1"),
            new Node("2"),
            new Node("3"),
            new Node("4")
        };

        var edges = new[]
        {
            new EdgeDirectedWeighted(nodes[0], nodes[1], 0),
            new EdgeDirectedWeighted(nodes[0], nodes[2], 0),
            new EdgeDirectedWeighted(nodes[1], nodes[2], 0),
            new EdgeDirectedWeighted(nodes[1], nodes[3], 0),
            new EdgeDirectedWeighted(nodes[2], nodes[3], 0)
        };
        return edges;
    }
}