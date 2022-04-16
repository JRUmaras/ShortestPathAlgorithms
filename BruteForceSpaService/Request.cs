using Graphs.Models;

namespace BruteForceSpaService
{
    public class Request
    {
        public GraphDirectedWeighted Graph { get; }
        
        public Node Start { get; }
        
        public Node End { get; }
        
        public Request(GraphDirectedWeighted graph, Node start, Node end)
        {
            Graph = graph;
            Start = start;
            End = end;
        }
    }
}