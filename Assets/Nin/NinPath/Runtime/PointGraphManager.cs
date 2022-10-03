using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PointGraphManager : MonoBehaviour {

    public PointGraph graph;
    public bool showPointsName;
    public bool showWeights;
    public bool showVertices;
    public bool drawBidirectional = true;

    public Dictionary<Point, Queue<PointPathFollower>> pointQueues;

    private Action<PointPathFollower> move = f => { Debug.Log(f.name); f.isMoving = true; f.RemoveFromLastPointQueue(); };
    private Action<PointPathFollower> unMove = f => f.isMoving = false;
    private Action<PointPathFollower> nothing = f => { };

    private void Start() {
        pointQueues = new Dictionary<Point, Queue<PointPathFollower>>();
        foreach (Point point in graph.points) {
            Queue<PointPathFollower> queue = new Queue<PointPathFollower>();
            queue.OnFirst += move;
            queue.OnAdd += unMove;
            queue.OnRemove += nothing;
            pointQueues.Add(point, queue);
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = new Color32(0, 0, 150, 75);
        foreach (PointGraphVertex vertex in graph.vertices) {
            if (vertex.origin != null && vertex.destination != null) {
                if (showVertices) {
                    if (vertex.isBidirectional) {
                        Gizmos.DrawLine(vertex.origin.position, vertex.destination.position);
                    }
                    else {
                        DrawArrow.ForGizmo(vertex.origin.position, vertex.destination.position - vertex.origin.position);
                    }
                }
#if UNITY_EDITOR
                if (showWeights) Handles.Label(Vector3.Lerp(vertex.origin.position, vertex.destination.position, 0.5f), vertex.weight.ToString("F0"));
#endif
            }
        }
#if UNITY_EDITOR
            if (showPointsName) {
            foreach (Point point in graph.points) {
                Handles.Label(point.position, point.name);
            }
        }
#endif
    }

}