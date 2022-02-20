using Graphs.Models;

namespace BruteForceSpaService
{
    public class Request
    {
        public GraphDirected Graph { get; }
        
        public Node Start { get; }
        
        public Node End { get; }
        
        public Request(GraphDirected graph, Node start, Node end)
        {
            Graph = graph;
            Start = start;
            End = end;
        }
    }
}