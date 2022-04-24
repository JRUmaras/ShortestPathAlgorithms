using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Graphs.Interfaces;

namespace Graphs.Helpers
{
    public class NodesPrinter
    {
        public static async Task PrintToFileWithCoordinatesAsync(IList<INode> nodes, string outputFilepath, IReadOnlyDictionary<INode, IPointCartesian> nodeToCoordinatesMap)
        {
            var areIdsIntegers = nodes.All(n => int.TryParse(n.Id, out _));

            var orderedNodes = areIdsIntegers 
                ? nodes.OrderBy(n => int.Parse(n.Id)) 
                : nodes.OrderBy(n => n.Id);

            var fileStream = File.Open(outputFilepath, FileMode.Create);
            await using var file = new StreamWriter(fileStream);

            foreach (var node in orderedNodes)
            {
                var point = nodeToCoordinatesMap[node];
                await file.WriteLineAsync($"{node.Id} {point.X} {point.Y}");
            }
        }
    }
}
