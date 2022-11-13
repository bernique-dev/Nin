using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// Follows a PointPath
/// </summary>
[ExecuteInEditMode]
public class PointPathFollower : MonoBehaviour, ILoadable, ILoadingWaiter {

    public static List<PointPathFollower> instances;
    public PointPath path;

    public PointGraphManager graphManager;

    /// <summary>
    /// Graph to calculate PointPath from
    /// </summary>
    public PointGraph graph {
        get {
            return graphManager? graphManager.graph : null;
        }
    }

    public bool hasPointsToGo { get { return graph.points.Count > 0; } }

    /// <summary>
    /// Progress in the path (0 <= d <= 1)
    /// </summary>
    public float distanceProgress {
        get {
            return path != null ? distanceFromStart / path.distance : 0;
        }
        set {
            distanceFromStart = path != null || double.IsNaN(value) ? path.distance * Mathf.Clamp(value, 0, 1) : 0;
        }
    }

    /// <summary>
    /// Distance from origin
    /// </summary>
    public float distanceFromStart {
        get {
            return m_distanceFromStart;
        }
        set {
            m_distanceFromStart = Mathf.Clamp(value, 0, path != null ? path.distance : 0);
            if (isGoing && path != null) transform.position = path.GetPositionFromDistance(distanceFromStart);
        }
    }

    public bool isArrived {
        get {
            return distanceProgress == 1;
        }
    }
    
    [SerializeField]
    private float m_distanceFromStart = 0;



    public bool showPathWhenNotSelected;

    public Point origin;
    public Point destination;

    public bool isGoing = false;
    public bool isMoving = false;
    public float speed = 1;
    public float rotationSpeed = 15;


    public Point previousNextPoint;
    public Point nextPoint;

    private void Awake() {
        if (instances == null) instances = new List<PointPathFollower>();
        instances.Add(this);
    }

    public void Load() {
        //todo Use to load save
    }

    public void Begin() {
        //todo Called by upper class (will call every class below like PPFollower, WorkerBehaviour, ...)

        //CalculateShortestPathFromHereToRandomPoint();
        CalculateShortestPath(origin, destination);
        //Debug.Log("Began");
    }

    private void Update() {
        if (Application.isPlaying) {
            if (isGoing && path != null && path.distance > 0) {

                // Checks current next Point and if different from saved one, adds to next Point's queue
                nextPoint = path.GetNextPointFromDistance(distanceFromStart);
                if (previousNextPoint != nextPoint) {
                    //if (!graphManager.pointQueues[nextPoint].Contains(this)) {
                        graphManager.pointQueues[nextPoint].Add(this);
                    //}
                    previousNextPoint = nextPoint;
                }
                // Finds the vector pointing from our position to the target
                Vector3 direction = (nextPoint.position - transform.position).normalized;
                if (isMoving) {
                    distanceFromStart += Time.deltaTime * speed;
                    if (direction != Vector3.zero) {
                        // Creates the rotation we need to be in to look at the target
                        Quaternion rotationToLookAt = Quaternion.LookRotation(direction);
                        // Rotates GO over time according to speed until we are in the required rotation
                        transform.rotation = Quaternion.Slerp(transform.rotation, rotationToLookAt, Time.deltaTime * rotationSpeed);
                    }
                }
                else {
                    isMoving = false;
                }

            }
        } else {
            // In Editor
            if (path != null) {
                path.SetNextPoints();
                path.CalculateDistances();
            }
        }
    }

    /// <summary>
    /// Removes froom last Point's queue
    /// </summary>
    public void RemoveFromLastPointQueue() {
        Point lastPoint = path.GetLastPointFromDistance(distanceFromStart);
        if (lastPoint != nextPoint) {
            if (graphManager.pointQueues[lastPoint].Contains(this)) {
                graphManager.pointQueues[lastPoint].Remove(this);
            }
        }
    }


    public void CalculateRandomPath() {
        distanceFromStart = 0;
        Point pathOrigin = graph.points[Random.Range(0, graph.points.Count - 1)];
        Point pathDestination = graph.points.Where(p => p != origin).ToList()[Random.Range(0, graph.points.Count - 1)];
        CalculateShortestPath(pathOrigin, pathDestination);
    }

    /// <summary>
    /// Calculates ShortestPath from current position to random Point taken from graph
    /// </summary>
    public void CalculateShortestPathFromHereToRandomPoint() {
        List<Point> possiblePoints = graph.points.Where(p => p != origin && (graphManager.pointQueues != null ? graphManager.pointQueues[p].Count <= 0 : true)).ToList();
        Point pathDestination = possiblePoints[Random.Range(0, possiblePoints.Count - 1)];
        CalculateShortestPathFromHere(pathDestination);
    }


    /// <summary>
    /// Calculates ShortestPath from current position to specified Point
    /// </summary>
    public void CalculateShortestPathFromHere(Point toPoint) {
        isGoing = false;

        destination = toPoint;

        if (path != null && path.GetPoints().Count > 0) {
            //Debug.Log(path.GetPoints().Count);
            Point previousLastPoint = path.GetLastPointFromDistance(distanceFromStart);
            Point previousNextPoint = path.GetNextPointFromDistance(distanceFromStart);

            distanceFromStart = path.GetDistanceFromLastPoint(distanceFromStart);
            if (previousLastPoint) origin = previousLastPoint;
            CalculateShortestPath(origin, destination, false);

            // Checks if path backtracks. If so, adds previous path's next point as path's origin
            Point nextPoint = path != null ? path.GetNextPointFromDistance(distanceFromStart) : previousNextPoint;
            if (previousNextPoint != nextPoint) {
                Point newOrigin = origin;
                if (previousNextPoint) newOrigin = previousNextPoint;
                distanceFromStart = Vector3.Distance(transform.position, newOrigin.position);
                origin = newOrigin;
                path.Insert(origin, 0);
            }
        } else {
            CalculateShortestPath(origin, destination, false);
        }

        isGoing = true;
    }

    /// <summary>
    /// Calculates shortest path from specified Points
    /// </summary>
    /// <param name="isMovingAfter">Can the player move after arriving ?</param>
    public void CalculateShortestPath(Point fromPoint, Point toPoint, bool isMovingAfter = true) {
        isGoing = false;
        origin = fromPoint;
        destination = toPoint;
        path = graph.FindShortestPath(fromPoint, toPoint);
        isGoing = isMovingAfter;
    }


    private void OnDrawGizmos() {
        if (showPathWhenNotSelected) {
            DrawGizmosPath();
        }
    }

    private void OnDrawGizmosSelected() {
        DrawGizmosPath();
    }

    /// <summary>
    /// Draw path as Gizmos
    /// </summary>
    private void DrawGizmosPath() {
        if (path != null) {

            List<Point> points = path.GetPoints();
            if (origin && destination && points.Count > 0) {
                Vector3 positionOnPath = path.GetPositionFromDistance(distanceFromStart);
                Vector3 headPoint = positionOnPath + Vector3.up;
                Gizmos.color = Color.red;
                Gizmos.DrawLine(positionOnPath, headPoint);
                Gizmos.color = Color.white;

                Point lastPoint = path.GetLastPointFromDistance(distanceFromStart);

                for (int i = 1; i < points.Count; i++) {
                    Point previousPoint = points[i - 1];
                    Point currentPoint = points[i];
                    Gizmos.color = new Color32(255, 255, 255, (byte)(previousPoint == lastPoint ? 255 : 75));
                    Gizmos.DrawLine(previousPoint.position, currentPoint.position);
                }

                Gizmos.color = Color.green;
                Gizmos.DrawLine(origin.position, origin.position + Vector3.up);
                Gizmos.color = Color.blue;
                Gizmos.DrawLine(destination.position, destination.position + Vector3.up);
            }
        }
    }
    public override string ToString() {
        return name;
    }
}
