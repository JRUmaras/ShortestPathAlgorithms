using Graphs.Interfaces;

namespace ShortestPathAlgorithms.CostCalculators.Interfaces
{
    public interface ICostCalculator<TCost, TState>
    {
        (TCost Cost, TState EndState) Calculate(IEdgeDirected edgeDirected, TState startState);
    }
}
