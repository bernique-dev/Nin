using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PointGraph {

    public List<Point> points;

    public List<PointGraphVertex> vertices;

    public PointPath FindShortestPath(Point fromPoint, Point toPoint) {
        PointPath path = null;

        Dictionary<Point, List<Point>> neighbours = CalculateNeighbours(points, vertices);

        List<Point> expandablePoints = new List<Point>() { fromPoint };
        Dictionary<Point, Point> cameFrom = new Dictionary<Point, Point>();

        Dictionary<Point, float> gScores = new Dictionary<Point, float>();
        Dictionary<Point, float> fScores = new Dictionary<Point, float>();
        foreach (Point point in points) {
            gScores.Add(point, float.PositiveInfinity);
            fScores.Add(point, float.PositiveInfinity);
        }
        gScores[fromPoint] = 0;
        fScores[fromPoint] = GetHScore(fromPoint, fromPoint);

        while (expandablePoints.Count > 0) {
            Point currentPoint = GetLowestFScorePoint(expandablePoints, fScores);
            if (currentPoint == toPoint) {
                return ReconstructPath(cameFrom, currentPoint);
            }
            expandablePoints.Remove(currentPoint);

            foreach (Point neighbourPoint in neighbours[currentPoint]) {
                float tmpGScore = gScores[currentPoint] + Vector3.Distance(currentPoint.position, neighbourPoint.position);
                if (tmpGScore < gScores[neighbourPoint]) {
                    cameFrom[neighbourPoint] = currentPoint;
                    gScores[neighbourPoint] = tmpGScore;
                    fScores[neighbourPoint] = tmpGScore + GetHScore(fromPoint, neighbourPoint);
                    if (!expandablePoints.Contains(neighbourPoint)) expandablePoints.Add(neighbourPoint);
                }
            }
        }

        return path;
    }

    private float GetHScore(Point fromPoint, Point toPoint) {
        return Vector3.Distance(fromPoint.position, toPoint.position);
    }

    private Point GetLowestFScorePoint(List<Point> _points, Dictionary<Point, float> _fScores) {
        Point lowestFScorePoint = _points[0];
        for (int i = 1; i < _points.Count; i++) {
            Point currentPoint = _points[i];
            if (_fScores[lowestFScorePoint] > _fScores[currentPoint]) {
                lowestFScorePoint = currentPoint;
            }
        }
        return lowestFScorePoint;
    }

    private Dictionary<Point, List<Point>> CalculateNeighbours(List<Point> _points, List<PointGraphVertex> _vertices) {
        Dictionary<Point, List<Point>> pointsAndneighbours = new Dictionary<Point, List<Point>>();

        foreach (Point point in _points) {
            pointsAndneighbours.Add(point, new List<Point>());
            foreach (PointGraphVertex vertex in _vertices.Where(v => v.origin == point || (v.destination == point && v.isBidirectional))) {
                pointsAndneighbours[point].Add(vertex.origin == point ? vertex.destination : vertex.origin);
            }
        }

        return pointsAndneighbours;
    }

    private PointPath ReconstructPath(Dictionary<Point, Point> cameFrom, Point current) {
        PointPath path = new PointPath();
        path.Add(current);
        while (cameFrom.ContainsKey(current)) {
            current = cameFrom[current];
            path.Insert(current, 0);
        }
        return path;
    }

}
