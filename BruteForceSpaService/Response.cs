using ShortestPathAlgorithms.Models;

namespace BruteForceSpaService
{
    public class Response
    {
        public Path<int> Path { get; }

        public Response(Path<int> path)
        {
            Path = path;
        }
    }
}