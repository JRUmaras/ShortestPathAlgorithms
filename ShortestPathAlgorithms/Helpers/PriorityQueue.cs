using System;
using System.Collections.Generic;
using System.Linq;

namespace ShortestPathAlgorithms.Helpers;

public class PriorityQueue<TElement, TPriority> 
    where TElement :  IEquatable<TElement>
    where TPriority :  IComparable<TPriority>
{
    private readonly Dictionary<TElement, TPriority> _priorityDict;
    private KeyValuePair<TElement, TPriority>? _nextToDequeue;

    public bool IsEmpty => _priorityDict.Count == 0;

    public PriorityQueue(IEnumerable<(TElement element, TPriority priority)>? elementsAndPriorities = null)
    {
        if (elementsAndPriorities is null)
        {
            _priorityDict = new Dictionary<TElement, TPriority>();
            _nextToDequeue = null;
            return;
        }

        _priorityDict = elementsAndPriorities.ToDictionary(kv => kv.element, kv => kv.priority);
        ResetNextToDequeue();
    }

    public void PushOrUpdate(TElement element, TPriority priority)
    {
        _priorityDict[element] = priority;

        if (_nextToDequeue is null)
        {
            ResetNextToDequeue();
            return;
        }
        
        if (_nextToDequeue!.Value.Value.CompareTo(priority) > 0)
        {
            _nextToDequeue = new KeyValuePair<TElement, TPriority>(element, priority);
        }
    }

    public (TElement element, TPriority priority)? Pop()
    {
        if (_nextToDequeue is null)
        {
            ResetNextToDequeue();
            if (_nextToDequeue is null) return null;
        }
        
        _priorityDict.Remove(_nextToDequeue.Value.Key);
        
        var dequeued = _nextToDequeue.Value;
        _nextToDequeue = null;
        
        return (dequeued.Key, dequeued.Value);
    }

    public TPriority GetPriority(TElement element) => _priorityDict[element];

    public bool TryGetPriority(TElement element, out TPriority? priority) => _priorityDict.TryGetValue(element, out priority);

    private void ResetNextToDequeue()
    {
        _nextToDequeue = null;

        if (_priorityDict.Count == 0) return;
        
        _nextToDequeue = _priorityDict.First();

        if (_priorityDict.Count == 1) return;
        
        foreach (var (element, priority) in _priorityDict)
        {
            if (priority.CompareTo(_nextToDequeue!.Value.Value) < 0)
            {
                _nextToDequeue = new KeyValuePair<TElement, TPriority>(element, priority);
            }
        }
    }
}