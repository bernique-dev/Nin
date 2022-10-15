using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Queue FIFO
/// </summary>
/// <typeparam name="T">Type of elements in the Queue</typeparam>
public class Queue<T> {

    private List<T> elements;

    /// <summary>
    /// Action triggered on element when it gets first of the Queue
    /// </summary>
    public Action<T> OnFirst;
    /// <summary>
    /// Action triggered on element when it was first of the Queue but is not anymore
    /// </summary>
    public Action<T> OnNotFirst;
    /// <summary>
    /// Action triggered on element when it gets added to the Queue
    /// </summary>
    public Action<T> OnAdd;
    /// <summary>
    /// Action triggered on element when it gets removed from the Queue
    /// </summary>
    public Action<T> OnRemove;

    /// <summary>
    /// Comparer used if Queue needs to be sorted (not efficient for Pathfinding)
    /// </summary>
    public Comparer<T> comparer;

    public Queue(Comparer<T> _comparer) {
        elements = new List<T>();
        comparer = _comparer;
    }

    /// <summary>
    /// Sorts the list (not efficient for Pathfinding)
    /// </summary>
    public void Sort() {
        if (comparer != null) {
            elements.Sort(comparer);
        } else {
            elements.Sort();
        }
    }

    /// <summary>
    /// Returns if the queue contains a specified element
    /// </summary>
    public bool Contains(T element) {
        return elements.Contains(element);
    }

    /// <summary>
    /// Adds specified element to the Queue (can sort Queue after adding)
    /// </summary>
    /// <param name="element">Element to add</param>
    /// <param name="sort">Sorts queue or not</param>
    public void Add(T element, bool sort = true) {
        elements.Add(element);
        OnAdd(element);
        T previousFirstElement = elements[0];
        if (sort) Sort();
        if (element.Equals(elements[0])) {
            OnFirst(element);
        }
        if (!previousFirstElement.Equals(elements[0])) {
            OnNotFirst(previousFirstElement);
        }
    }

    /// <summary>
    /// Removes specified element from the Queue
    /// </summary>
    /// <param name="element"></param>
    public void Remove(T element) {
        if (elements.Count > 1) {
            T previousSecondElement = elements[1];
            elements.Remove(element);
            OnRemove(element);
            if (previousSecondElement.Equals(elements[0])) {
                OnFirst(previousSecondElement);
            }
        } else {
            elements.Remove(element);
            OnRemove(element);
        }
    }

    public override string ToString() {
        string elementsString = string.Join(',', elements.Select(f => f.ToString()).ToList());
        return elementsString;
    }

}
