using Graphs.Interfaces;
using ShortestPathAlgorithms.Interfaces;

namespace ShortestPathAlgorithms.CostCalculators.Interfaces
{
    public interface ICostCalculator<TCost>
    {
        (TCost Cost, IState EndState) Calculate(IEdgeDirected edgeDirected, IState startState);
    }
}
