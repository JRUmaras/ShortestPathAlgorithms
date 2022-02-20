using ShortestPathAlgorithms.Models;

namespace BruteForceSpaService
{
    public class Response
    {
        public Path Path { get; }

        public Response(Path path)
        {
            Path = path;
        }
    }
}