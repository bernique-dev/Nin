using System;
using System.Collections.Generic;
using UnityEngine;

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

    public PointGraphVertex() : this(null, null) { }

    public PointGraphVertex(Point _origin, Point _destination) {
        origin = _origin;
        destination = _destination;
    }

}
