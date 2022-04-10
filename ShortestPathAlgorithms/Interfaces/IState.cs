using System;

namespace ShortestPathAlgorithms.Interfaces
{
    public interface IState : IEquatable<IState>
    {
        DateTime Time { get; set; }
    }
}
