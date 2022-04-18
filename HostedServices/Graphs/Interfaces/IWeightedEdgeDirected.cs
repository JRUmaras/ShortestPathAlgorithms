namespace Graphs.Interfaces
{
    public interface IWeightedEdgeDirected : IEdgeDirected
    {
        int Weight { get; }
    }
}
