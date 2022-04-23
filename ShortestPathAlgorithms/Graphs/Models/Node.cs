using Graphs.Interfaces;

namespace Graphs.Models;

public class Node : INode
{
    public string Id { get; }

    public Node(string id)
    {
        Id = id;
    }

    public bool Equals(INode? other) => Id == other?.Id;

    public override bool Equals(object? obj) => obj is INode other && Equals(other);

    public override int GetHashCode() => Id.GetHashCode();

    public static bool operator ==(Node node1, Node node2) => node1.Equals(node2);
    
    public static bool operator !=(Node node1, Node node2) => !(node1 == node2);

    public override string ToString() => Id;
}