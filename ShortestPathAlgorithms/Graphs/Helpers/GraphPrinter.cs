using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Graphs.Interfaces;

namespace Graphs.Helpers;

public class GraphPrinter
{
    public static async Task PrintToFileAsync(IGraphDirected graph, string outputFilepath)
    {
        var orderedNodes = graph.Nodes.OrderBy(n => n.Id);
        var orderedEdges = graph.Edges
            .OrderBy(e => e.From.Id)
            .ThenBy(e => e.To.Id);

        var fileStream = File.Open(outputFilepath, FileMode.Create);
        await using var file = new StreamWriter(fileStream);

        foreach (var node in orderedNodes)
        {
            await file.WriteLineAsync(node.Id);
        }

        foreach (var edge in orderedEdges)
        {
            await file.WriteLineAsync($"{edge.From.Id} {edge.To.Id}");
        }
    }

    public static async Task PrintToFileWithCoordinatesAsync(IGraphDirected graph, string outputFilepath, IReadOnlyDictionary<INode, IPointCartesian> nodeToCoordinatesMap)
    {
        var orderedNodes = graph.Nodes.OrderBy(n => n.Id);
        var orderedEdges = graph.Edges
            .OrderBy(e => e.From.Id)
            .ThenBy(e => e.To.Id);

        var fileStream = File.Open(outputFilepath, FileMode.Create);
        await using var file = new StreamWriter(fileStream);

        foreach (var node in orderedNodes)
        {
            var point = nodeToCoordinatesMap[node];
            await file.WriteLineAsync($"{node.Id} {point.X} {point.Y}");
        }

        foreach (var edge in orderedEdges)
        {
            await file.WriteLineAsync($"{edge.From.Id} {edge.To.Id}");
        }
    }
}