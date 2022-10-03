using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class Queue<T> {

    private List<T> elements;

    public Action<T> OnFirst;
    public Action<T> OnNotFirst;
    public Action<T> OnAdd;
    public Action<T> OnRemove;

    public Comparer<T> comparer;

    public Queue(Comparer<T> _comparer) {
        elements = new List<T>();
        comparer = _comparer;
    }

    public void Sort() {
        if (comparer != null) {
            elements.Sort(comparer);
        } else {
            elements.Sort();
        }
    }

    public bool Contains(T element) {
        return elements.Contains(element);
    }

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
