using System;
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
            new EdgeDirected(nodes[0], nodes[1], 1),
            new EdgeDirected(nodes[0], nodes[2], 1),
            new EdgeDirected(nodes[1], nodes[4], 1),
            new EdgeDirected(nodes[1], nodes[5], 1),
            new EdgeDirected(nodes[2], nodes[3], 1),
            new EdgeDirected(nodes[2], nodes[4], 1),
            new EdgeDirected(nodes[3], nodes[7], 1),
            new EdgeDirected(nodes[4], nodes[5], 1),
            new EdgeDirected(nodes[4], nodes[6], 1),
            // reverse edges
            new EdgeDirected(nodes[1], nodes[0], 1),
            new EdgeDirected(nodes[2], nodes[0], 1),
            new EdgeDirected(nodes[3], nodes[2], 1),
            new EdgeDirected(nodes[4], nodes[1], 1),
            new EdgeDirected(nodes[4], nodes[2], 1),
            new EdgeDirected(nodes[5], nodes[1], 1),
            new EdgeDirected(nodes[5], nodes[4], 1),
            new EdgeDirected(nodes[6], nodes[4], 1),
            new EdgeDirected(nodes[7], nodes[3], 1)
        };

        var graph = new GraphDirectedWeighted(edges);
        
        return graph;
    }

    /// <summary>
    /// Creates a graph consisting of two disconnected sub-graphs.
    /// Each of the sub-graphs are themselves connected and any node reachable
    /// from any other node. 
    /// </summary>
    /// <returns>Directed graph <see cref="GraphDirectedWeighted"/> with said properties.</returns>
    public static GraphDirectedWeighted CreateDisconnectedGraph()
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
            new EdgeDirected(nodes[0], nodes[1], 1),
            new EdgeDirected(nodes[1], nodes[2], 1),
            new EdgeDirected(nodes[2], nodes[1], 1),
            new EdgeDirected(nodes[3], nodes[4], 1),
            new EdgeDirected(nodes[4], nodes[5], 1),
            new EdgeDirected(nodes[5], nodes[3], 1),
        };

        var graph = new GraphDirectedWeighted(edges);
        
        return graph;
    }

    /// <summary>
    /// Creates a graph which has a node with only outgoing edges.
    /// All other nodes can be reached from any other node.
    /// </summary>
    /// <returns>Directed graph <see cref="GraphDirectedWeighted"/> with said properties.</returns>
    public static GraphDirectedWeighted CreateGraphWithUnreachableNode()
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
            new EdgeDirected(nodes[0], nodes[1], 1),
            new EdgeDirected(nodes[1], nodes[2], 1),
            new EdgeDirected(nodes[2], nodes[3], 1),
            new EdgeDirected(nodes[3], nodes[1], 1)
        };

        var graph = new GraphDirectedWeighted(edges);
        
        return graph;
    }

    
    /// <summary>
    /// Based on https://sboyles.github.io/teaching/ce392d/8-tdsp.pdf.
    /// </summary>
    /// <returns>Directed graph <see cref="GraphDirected"/>.</returns>
    public static GraphDirected CreateGraphForTimeDependentDijkstraTestCase()
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
            new EdgeDirected(nodes[0], nodes[1], 0),
            new EdgeDirected(nodes[0], nodes[2], 0),
            new EdgeDirected(nodes[1], nodes[2], 0),
            new EdgeDirected(nodes[1], nodes[3], 0),
            new EdgeDirected(nodes[2], nodes[3], 0)
        };

        var graph = new GraphDirected(edges);
        
        return graph;
    }

    /// <summary>
    /// Creates a connected graph with bidirectional edges.
    /// </summary>
    /// <returns>Directed graph <see cref="GraphDirected"/>.</returns>
    public static GraphDirected CreateBasicGraph()
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
            new EdgeDirected(nodes[0], nodes[1], 0),
            new EdgeDirected(nodes[0], nodes[2], 0),
            new EdgeDirected(nodes[1], nodes[4], 0),
            new EdgeDirected(nodes[1], nodes[5], 0),
            new EdgeDirected(nodes[2], nodes[3], 0),
            new EdgeDirected(nodes[2], nodes[4], 0),
            new EdgeDirected(nodes[3], nodes[7], 0),
            new EdgeDirected(nodes[4], nodes[5], 0),
            new EdgeDirected(nodes[4], nodes[6], 0),
            // reverse edges
            new EdgeDirected(nodes[1], nodes[0], 0),
            new EdgeDirected(nodes[2], nodes[0], 0),
            new EdgeDirected(nodes[3], nodes[2], 0),
            new EdgeDirected(nodes[4], nodes[1], 0),
            new EdgeDirected(nodes[4], nodes[2], 0),
            new EdgeDirected(nodes[5], nodes[1], 0),
            new EdgeDirected(nodes[5], nodes[4], 0),
            new EdgeDirected(nodes[6], nodes[4], 0),
            new EdgeDirected(nodes[7], nodes[3], 0)
        };

        var graph = new GraphDirected(edges);
        
        return graph;
    }
}