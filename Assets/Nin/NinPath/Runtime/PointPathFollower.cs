using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class PointPathFollower : MonoBehaviour {


    public PointPath path;

    public PointGraphManager graphManager;

    public PointGraph graph {
        get {
            return graphManager? graphManager.graph : null;
        }
    }

    public float distanceProgress {
        get {
            return path != null ? distanceFromStart / path.distance : 0;
        }
        set {
            distanceFromStart = path != null || double.IsNaN(value) ? path.distance * Mathf.Clamp(value, 0, 1) : 0;
        }
    }

    public float distanceFromStart {
        get {
            return m_distanceFromStart;
        }
        set {
            m_distanceFromStart = Mathf.Clamp(value, 0, path != null ? path.distance : 0);
            if (isGoing) transform.position = path.GetPositionFromDistance(distanceFromStart);
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

    public bool isGoing;
    public bool isMoving = false;
    public float speed = 1;
    public float rotationSpeed = 15;

    public float distanceToleranceFromPoint = 1;

    private Point previousNextPoint;
    private Point previousPreviousNextPoint;
    private Point nextPoint;

    private void Start() {
        //CalculateShortestPathFromHereToRandomPoint();
        CalculateShortestPath(origin, destination);
    }

    private void Update() {
        if (Application.isPlaying) {
            if (isGoing) {

                nextPoint = path.GetNextPointFromDistance(distanceFromStart);
                if (previousNextPoint != nextPoint) {
                    //Debug.Log(name + " " + previousNextPoint + " -> " + nextPoint);
                    if (!graphManager.pointQueues[nextPoint].Contains(this)) {
                        graphManager.pointQueues[nextPoint].Add(this);
                    }
                    previousNextPoint = nextPoint;
                }
                //find the vector pointing from our position to the target
                Vector3 direction = (nextPoint.position - transform.position).normalized;
                if (isMoving) {
                    distanceFromStart += Time.deltaTime * speed;
                    if (direction != Vector3.zero) {
                        //create the rotation we need to be in to look at the target
                        Quaternion rotationToLookAt = Quaternion.LookRotation(direction);
                        //rotate us over time according to speed until we are in the required rotation
                        transform.rotation = Quaternion.Slerp(transform.rotation, rotationToLookAt, Time.deltaTime * rotationSpeed);
                    }
                }
                else {
                    isMoving = false;
                }

            }
        } else {
            if (path != null) {
                path.SetNextPoints();
                path.CalculateDistances();
            }
        }
    }

    public void RemoveFromLastPointQueue() {
        //Debug.Log(this);
        Point lastPoint = path.GetLastPointFromDistance(distanceFromStart);
        if (lastPoint != nextPoint) {
            if (graphManager.pointQueues[lastPoint].Contains(this)) {
                graphManager.pointQueues[lastPoint].Remove(this);
            }
        }
    }


    public void CalculateShortestPathFromHereToRandomPoint() {
        Point pathDestination = graph.points.Where(p => p != origin).ToList()[Random.Range(0, graph.points.Count - 1)];
        CalculateShortestPathFromHere(pathDestination);
    }

    public void CalculateShortestPathFromHere(Point toPoint) {
        isGoing = false;

        destination = toPoint;
        Point previousLastPoint = path.GetLastPointFromDistance(distanceFromStart);
        Point previousNextPoint = path.GetNextPointFromDistance(distanceFromStart);

        distanceFromStart = path.GetDistanceFromLastPoint(distanceFromStart);
        if(previousLastPoint) origin = previousLastPoint;
        CalculateShortestPath(origin, destination, false);

        PointPath newPath = path;
        Point nextPoint = newPath.GetNextPointFromDistance(distanceFromStart);
        if (previousNextPoint != nextPoint) {
            if (previousNextPoint) origin = previousNextPoint;
            distanceFromStart = Vector3.Distance(transform.position, origin.position);
            path.Insert(origin, 0);
        }

        isGoing = true;
    }

    public void CalculateShortestPath(Point fromPoint, Point toPoint, bool isMovingAfter = true) {
        isGoing = false;
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

    private void DrawGizmosPath() {
        if (origin && destination) {
            Vector3 positionOnPath = path.GetPositionFromDistance(distanceFromStart);
            Vector3 headPoint = positionOnPath + Vector3.up;
            Gizmos.color = Color.red;
            Gizmos.DrawLine(positionOnPath, headPoint);
            Gizmos.color = Color.white;

            Point lastPoint = path.GetLastPointFromDistance(distanceFromStart);

            List<Point> points = path.GetPoints();
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
        if (nextPoint && previousNextPoint) {
            Handles.Label(transform.position + Vector3.up * 3, previousNextPoint + " - " + nextPoint + "\n(" + previousPreviousNextPoint + ")");
        }
    }
    public override string ToString() {
        return name;
    }
}
