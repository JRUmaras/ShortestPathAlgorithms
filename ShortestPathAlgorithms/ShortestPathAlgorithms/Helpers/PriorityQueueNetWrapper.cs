using System;
using System.Collections.Generic;

namespace ShortestPathAlgorithms.Helpers;

/// <summary>
/// <see cref="PriorityQueue&lt;TElement, TPriority&gt;"/> does not support updating the priorities,
/// so this wrapper class ensures that only one instance of an element would effectively exist in the queue
/// even if the underlying instance of <see cref="PriorityQueue&lt;TElement, TPriority&gt;"/> actually has
/// multiple priorities enqueued for the said element.
/// </summary>
/// <typeparam name="TElement"></typeparam>
/// <typeparam name="TPriority"></typeparam>
public class PriorityQueueNetWrapper<TElement, TPriority> : IPriorityQueue<TElement, TPriority> 
    where TElement : IEquatable<TElement> 
    where TPriority : IComparable<TPriority>
{
    private readonly PriorityQueue<TElement, TPriority> _priorityQueue;
    private readonly Dictionary<TElement, TPriority> _priorityDictionary;

    public bool IsEmpty => _priorityDictionary.Count == 0;

    public PriorityQueueNetWrapper(IEnumerable<(TElement element, TPriority priority)>? elementsAndPriorities = null)
    {
        _priorityQueue = new PriorityQueue<TElement, TPriority>();
        _priorityDictionary = new Dictionary<TElement, TPriority>();

        if (elementsAndPriorities is null) return;

        foreach (var (element, priority) in elementsAndPriorities)
        {
            _priorityQueue.Enqueue(element, priority);
            _priorityDictionary[element] = priority;
        }
    }

    public void PushOrUpdate(TElement element, TPriority priority)
    {
        _priorityQueue.Enqueue(element, priority);
        _priorityDictionary[element] = priority;
    }

    public (TElement element, TPriority priority)? Pop()
    {
        bool dequeueSucceeded;
        TElement element;
        TPriority priority;
        do
        {
            if (!_priorityQueue.TryDequeue(out element!, out priority!)) return null;
            dequeueSucceeded = _priorityDictionary.Remove(element);
        } while (dequeueSucceeded is false);
            
        return (element, priority);

    }

    public bool TryGetPriority(TElement element, out TPriority? priority) => _priorityDictionary.TryGetValue(element, out priority);
}