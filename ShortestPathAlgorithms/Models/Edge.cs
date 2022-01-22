namespace ShortestPathAlgorithms.Models;

public struct Edge
{
    private string _id;

    public string Id => _id ??= Node1.Id + '-' + Node2.Id;

    public Node Node1 { get; set; }
    
    public Node Node2 { get; set; }
    
    public int Weight { get; set; }

    public bool HasNode(Node node) => Node1.Id == node.Id || Node2.Id == node.Id;

    public Node GetTheOtherNode(Node node) => node.Id == Node1.Id ? Node2 : Node1;
}