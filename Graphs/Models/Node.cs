using System;

namespace Graphs.Models;

public class Node : IEquatable<Node>
{
    public string Id { get; }

    public Node(string id)
    {
        Id = id;
    }

    public bool Equals(Node? other) => Id == other?.Id;

    public override bool Equals(object? obj) => obj is Node other && Equals(other);

    public override int GetHashCode() => Id.GetHashCode();

    public static bool operator ==(Node node1, Node node2) => node1.Equals(node2);

    public static bool operator !=(Node node1, Node node2) => !(node1 == node2);
}