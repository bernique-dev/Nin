using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Representation of a Point. Needs to be associated ot a GameObject
/// </summary>
public class Point : MonoBehaviour {

    public Vector3 position {
        get {
            return transform.position;
        }
    }

    public override string ToString() {
        return name;
    }

    /// <summary>
    /// Returns the closest Point inside a list of Points
    /// </summary>
    /// <param name="points">List of Points to look into</param>
    public Point GetClosestPointAmong(List<Point> points) {
        Point point = null;
        foreach (Point tmpPoint in points) {
            if (point == null) {
                point = tmpPoint;
            } else {
                if (Vector3.Distance(transform.position, tmpPoint.position) < Vector3.Distance(transform.position, point.position)) {
                    point = tmpPoint;
                }
            }
        }
        return point;
    }
}