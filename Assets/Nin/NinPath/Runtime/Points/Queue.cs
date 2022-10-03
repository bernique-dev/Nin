using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class Queue<T> {

    private List<T> elements;

    public Action<T> OnFirst;
    public Action<T> OnAdd;
    public Action<T> OnRemove;

    public Queue() {
        elements = new List<T>();
    }

    public bool Contains(T element) {
        return elements.Contains(element);
    }

    public void Add(T element) {
        elements.Add(element);
        OnAdd(element);
        if (element.Equals(elements[0])) {
            OnFirst(element);
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
