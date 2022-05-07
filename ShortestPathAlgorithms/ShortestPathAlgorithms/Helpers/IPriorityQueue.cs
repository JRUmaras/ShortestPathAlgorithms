using System;

namespace ShortestPathAlgorithms.Helpers;

public interface IPriorityQueue<TElement, TPriority> where TElement : IEquatable<TElement> where TPriority : IComparable<TPriority>
{
    bool IsEmpty { get; }

    void PushOrUpdate(TElement element, TPriority priority);

    (TElement element, TPriority priority)? Pop();

    bool TryGetPriority(TElement element, out TPriority? priority);
}