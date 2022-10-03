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

    public Dictionary<Point, float> pointsAndDistance {
        get {
            if (m_pointsAndDistance == null) m_pointsAndDistance = new Dictionary<Point, float>();
            return m_pointsAndDistance;
        }
    }
    private Dictionary<Point, float> m_pointsAndDistance;


    public Dictionary<Point, Point> pointsAndNext {
        get {
            if (m_pointsAndNext == null) m_pointsAndNext = new Dictionary<Point, Point>();
            return m_pointsAndNext;
        }
    }
    private Dictionary<Point, Point> m_pointsAndNext;

    public void Add(Point point) {
        points.Add(point);
        CalculateDistances(points.Count - 1);
        SetNextPoints(points.Count - 2);
    }

    public void Remove(Point point) {
        int pointIdx = points.IndexOf(point);
        points.Remove(point);
        CalculateDistances(pointIdx);
        SetNextPoints(Mathf.Clamp(pointIdx - 1, 0, points.Count));
    }

    public void Insert(Point point, int idx) {
        points.Insert(idx, point);
        CalculateDistances(idx);
        SetNextPoints(idx);
    }

    public void CheckDistancesCount() {
        if (pointsAndDistance.Count != points.Count) CalculateDistances();
    }

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

    public void CheckNextPointsCount() {
        if (pointsAndNext.Count != points.Count - 1) SetNextPoints();
    }

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

    public List<Point> GetPoints() {
        return points != null ? new List<Point>(points) : new List<Point>();
    }

    public float GetDistanceFromLastPoint(float _distance) {
        Point lastPoint = GetLastPointFromDistance(_distance);
        if (lastPoint) {
            return _distance - (pointsAndDistance.ContainsKey(lastPoint) ? pointsAndDistance[lastPoint] : 0);
        }
        return _distance;
    }

    public float GetDistanceFromNextPoint(float _distance) {
        Point nextPoint = GetNextPointFromDistance(_distance);
        if (nextPoint) {
            return _distance - (pointsAndDistance.ContainsKey(nextPoint) ? pointsAndDistance[nextPoint] : 0);
        }
        return _distance;
    }

    public Point GetNextPointFromDistance(float _distance) {
        CheckDistancesCount();
        return points.Count > 1 ? points.First(p => pointsAndDistance[p] >= Mathf.Clamp(_distance, 0, distance)) : (points.Count > 0 ? points[0] : null);

    }

    public Point GetLastPointFromDistance(float distance) {
        CheckDistancesCount();
        return points.Count > 1 ? points.Last(p => pointsAndDistance[p] <= distance) : (points.Count > 0 ? points[0] : null);
    }

    public Vector3 GetPositionFromDistance(float distance) {
        SetNextPoints();
        Point lastPoint = GetLastPointFromDistance(distance);
        if (!lastPoint) return Vector3.zero;
        return lastPoint.position + ((pointsAndNext[lastPoint].position - lastPoint.position).normalized * (distance - pointsAndDistance[lastPoint]));
        //return lastPoint.position;
    }

}
