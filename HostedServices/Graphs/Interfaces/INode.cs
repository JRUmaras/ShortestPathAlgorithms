using System;

namespace Graphs.Interfaces
{
    public interface INode : IEquatable<INode>
    {
        string Id { get; }
    }
}
