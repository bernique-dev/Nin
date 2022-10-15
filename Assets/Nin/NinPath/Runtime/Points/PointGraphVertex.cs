using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Representation of a vertex inside a PointGraph
/// </summary>
[Serializable]
public class PointGraphVertex {

    public Point origin;
    public Point destination;

    public bool isBidirectional = true;

    /// <summary>
    /// Relative to the type of path it represents
    /// (i.e a road, a walkway, a sea-taxi ride, ...
    /// </summary>
    public int weight;

    public float distance {
        get {
            return (origin != null && destination != null) ? Vector3.Distance(origin.position, destination.position) : 0;
        }
    }

    public PointGraphVertex() : this(null, null, 0) { }

    public PointGraphVertex(Point _origin, Point _destination) : this(_origin, _destination, 0) { }

    public PointGraphVertex(Point _origin, Point _destination, int _weight) {
        origin = _origin;
        destination = _destination;
        weight = _weight;
    }

    public override bool Equals(object obj) {
        PointGraphVertex objVertex = obj as PointGraphVertex;
        if (objVertex != null) {
            return objVertex.origin == origin && objVertex.destination == destination || (isBidirectional ? (objVertex.origin == destination && objVertex.destination == origin) : false);
        }
        return base.Equals(obj);
    }

    public override int GetHashCode() {
        return origin.GetHashCode() + destination.GetHashCode() + isBidirectional.GetHashCode();
    }

}
