using System;
using ShortestPathAlgorithms.Interfaces;

namespace ShortestPathAlgorithms.Models
{
    public struct State : IState
    {
        public DateTime Time { get; set; }
    }
}
