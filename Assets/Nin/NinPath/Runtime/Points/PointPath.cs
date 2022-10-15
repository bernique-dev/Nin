using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class PointPath {

    private List<Point> points {
        get {
            if (m_points == null) m_points = new List<Point>();
            return m_points;
        }
    }
    [SerializeField]
    private List<Point> m_points;

    /// <summary>
    /// Returns the total distance from origin to destination passing by all points
    /// </summary>
    public float distance {
        get {
            float result = 0;
            if (points.Count > 0) {
                for (int i = 0; i < points.Count - 1; i++) {
                    Point nextPoint = points[i + 1];
                    Point currentPoint = points[i];
                    result += Vector3.Distance(nextPoint.position, currentPoint.position);
                }
            }
            return result;
        }
    }

    /// <summary>
    /// Associates Points and their distance from the origin
    /// </summary>
    public Dictionary<Point, float> pointsAndDistance {
        get {
            if (m_pointsAndDistance == null) m_pointsAndDistance = new Dictionary<Point, float>();
            return m_pointsAndDistance;
        }
    }
    private Dictionary<Point, float> m_pointsAndDistance;

    /// <summary>
    /// Associates points and its following one in the path
    /// </summary>
    public Dictionary<Point, Point> pointsAndNext {
        get {
            if (m_pointsAndNext == null) m_pointsAndNext = new Dictionary<Point, Point>();
            return m_pointsAndNext;
        }
    }
    private Dictionary<Point, Point> m_pointsAndNext;

    /// <summary>
    /// Adds Point to the path
    /// </summary>
    /// <param name="point">Point to be added</param>
    public void Add(Point point) {
        points.Add(point);
        CalculateDistances(points.Count - 1);
        SetNextPoints(points.Count - 2);
    }

    /// <summary>
    /// Removes Point from the path
    /// </summary>
    /// <param name="point">Point to be removed</param>
    public void Remove(Point point) {
        int pointIdx = points.IndexOf(point);
        points.Remove(point);
        CalculateDistances(pointIdx);
        SetNextPoints(Mathf.Clamp(pointIdx - 1, 0, points.Count));
    }

    /// <summary>
    /// Inserts Point at specified index
    /// </summary>
    /// <param name="point">Point to be inserted</param>
    /// <param name="idx">Index to insert Point at</param>
    public void Insert(Point point, int idx) {
        points.Insert(idx, point);
        CalculateDistances(idx);
        SetNextPoints(idx);
    }

    /// <summary>
    /// Checks if pointsAndDistance is updated. If not, updates it.
    /// </summary>
    public void CheckDistancesCount() {
        if (pointsAndDistance.Count != points.Count) CalculateDistances();
    }

    /// <summary>
    /// Calculates distances from origin to each Points
    /// </summary>
    /// <param name="from"></param>
    public void CalculateDistances(int from = 0) {
        if (from >= 0 && points.Count > 0) {
            if (pointsAndDistance.ContainsKey(points[0])) {
                pointsAndDistance[points[0]] = 0;
            }
            else {
                pointsAndDistance.Add(points[0], 0);
            }
            for (int i = Mathf.Clamp(from, 1, points.Count - 1); i < points.Count; i++) {
                Point previousPoint = points[i - 1];
                Point currentPoint = points[i];
                float value = (pointsAndDistance.ContainsKey(previousPoint) ? pointsAndDistance[previousPoint] : 0) + Vector3.Distance(previousPoint.position, currentPoint.position);
                if (pointsAndDistance.ContainsKey(currentPoint)) {
                    pointsAndDistance[currentPoint] = value;
                } else {
                    pointsAndDistance.Add(currentPoint, value);
                }
            }
            
        }
    }

    /// <summary>
    /// Checks if pointsAndNext is updated. If not, updates it.
    /// </summary>
    public void CheckNextPointsCount() {
        if (pointsAndNext.Count != points.Count - 1) SetNextPoints();
    }

    /// <summary>
    /// Sets, for each Point, its following one in the path
    /// </summary>
    /// <param name="from"></param>
    public void SetNextPoints(int from = 0) {
        if (points.Count > from && from >= 0) {
            for (int i = from; i < points.Count - 1; i++) {
                Point nextPoint = points[i + 1];
                Point currentPoint = points[i];
                if (pointsAndNext.ContainsKey(currentPoint)) {
                    pointsAndNext[currentPoint] = nextPoint;
                }
                else {
                    pointsAndNext.Add(currentPoint, nextPoint);
                }

            }
            if (points.Count > 0) {
                Point lastPoint = points[points.Count - 1];
                if (pointsAndNext.ContainsKey(lastPoint)) {
                    pointsAndNext[lastPoint] = lastPoint;
                }
                else {
                    pointsAndNext.Add(lastPoint, lastPoint);
                }
            }
        }
        
    }

    /// <summary>
    /// Returns all Points
    /// </summary>
    public List<Point> GetPoints() {
        return points != null ? new List<Point>(points) : new List<Point>();
    }

    /// <summary>
    /// Returns the distance from the last Point at specified distance
    /// </summary>
    public float GetDistanceFromLastPoint(float _distance) {
        Point lastPoint = GetLastPointFromDistance(_distance);
        if (lastPoint) {
            return _distance - (pointsAndDistance.ContainsKey(lastPoint) ? pointsAndDistance[lastPoint] : 0);
        }
        return _distance;
    }
    /// <summary>
    /// Returns the distance from next Point to be crossed at specified distance
    /// </summary>
    public float GetDistanceFromNextPoint(float _distance) {
        Point nextPoint = GetNextPointFromDistance(_distance);
        if (nextPoint) {
            return _distance - (pointsAndDistance.ContainsKey(nextPoint) ? pointsAndDistance[nextPoint] : 0);
        }
        return _distance;
    }

    /// <summary>
    /// Returns next Point to be crossed at specified distance
    /// </summary>
    public Point GetNextPointFromDistance(float _distance) {
        CheckDistancesCount();
        return points.Count > 1 ? points.First(p => pointsAndDistance[p] >= Mathf.Clamp(_distance, 0, distance)) : (points.Count > 0 ? points[0] : null);

    }


    /// <summary>
    /// Returns last Point crossed at specified distance
    /// </summary>
    public Point GetLastPointFromDistance(float distance) {
        CheckDistancesCount();
        return points.Count > 1 ? points.Last(p => pointsAndDistance[p] <= distance) : (points.Count > 0 ? points[0] : null);
    }

    /// <summary>
    /// Returns position at a specified distance from the origin
    /// </summary>
    public Vector3 GetPositionFromDistance(float distance) {
        SetNextPoints();
        Point lastPoint = GetLastPointFromDistance(distance);
        if (!lastPoint) return Vector3.zero;
        return lastPoint.position + ((pointsAndNext[lastPoint].position - lastPoint.position).normalized * (distance - pointsAndDistance[lastPoint]));
        //return lastPoint.position;
    }

}
