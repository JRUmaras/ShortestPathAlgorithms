namespace Graphs.Interfaces
{
    public interface IEdgeDirectedWeighted : IEdgeDirected
    {
        int Weight { get; }
    }
}
