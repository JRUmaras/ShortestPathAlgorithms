using ShortestPathAlgorithms.Models;

namespace BruteForceSpaService
{
    public class Response<TCost>
    {
        public Path<TCost>? Path { get; }

        public Response(Path<TCost>? path)
        {
            Path = path;
        }
    }
}