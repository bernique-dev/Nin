using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point : MonoBehaviour {

    public Vector3 position {
        get {
            return transform.position;
        }
    }

    public override string ToString() {
        return name;
    }

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