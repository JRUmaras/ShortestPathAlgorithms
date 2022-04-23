using Graphs.Interfaces;

namespace Graphs.Models;

public readonly struct PointCartesian : IPointCartesian
{
    public double X { get; }
    
    public double Y { get; }

    public PointCartesian(double x, double y)
    {
        X = x;
        Y = y;
    }
}