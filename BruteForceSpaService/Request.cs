using ShortestPathAlgorithms.Models;

namespace BruteForceSpaService
{
    public class Request
    {
        public UndirectedGraph Graph { get; set; }
        
        public Node Start { get; set; }
        
        public Node End { get; set; }
    }
}