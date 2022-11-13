using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Compares PointPathFollower's distance from next Point
/// </summary>
public class PointPathFollowerDistanceComparer : Comparer<PointPathFollower> {
    public override int Compare(PointPathFollower x, PointPathFollower y) {
        if (x.isMoving == y.isMoving) {
            return x.path.GetDistanceFromNextPoint(x.distanceFromStart).CompareTo(y.path.GetDistanceFromNextPoint(y.distanceFromStart));
        } else {
            if (x.isMoving) {
                return -1;
            }
            if (y.isMoving) {
                return 1;
            }
            return 0;
        }
    }
}
