using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// Manages PointGraph
/// </summary>
public class PointGraphManager : MonoBehaviour {

    public PointGraph graph;
    /// <summary>
    /// Shows points' names as Gizmos
    /// </summary>
    public bool showPointsName;
    /// <summary>
    /// Shows vertices' weights as Gizmos
    /// </summary>
    public bool showWeights;
    /// <summary>
    /// Shows vertices' as Gizmos
    /// </summary>
    public bool showVertices;
    public bool drawBidirectional = true;

    /// <summary>
    /// Associates Points and a Queue for PointPathFollowers expecting to go to it
    /// </summary>
    public Dictionary<Point, Queue<PointPathFollower>> pointQueues;

    private Action<PointPathFollower> move = f => { f.isMoving = true; f.RemoveFromLastPointQueue(); };
    private Action<PointPathFollower> unMove = f => f.isMoving = false;
    private Action<PointPathFollower> nothing = f => { };

    private void Start() {
        pointQueues = new Dictionary<Point, Queue<PointPathFollower>>();
        foreach (Point point in graph.points) {
            Queue<PointPathFollower> queue = new Queue<PointPathFollower>(new PointPathFollowerDistanceComparer());
            queue.OnFirst += move;
            queue.OnNotFirst += unMove;
            queue.OnAdd += unMove;
            queue.OnRemove += nothing;
            pointQueues.Add(point, queue);
        }
    }

    /// <summary>
    /// Links specified points with a specified weigh (0 by default)
    /// </summary>
    public void Link(Point origin, Point destination, float weight = 0) {
        PointGraphVertex vertex = new PointGraphVertex(origin, destination);
    }

    private void OnDrawGizmos() {
        Gizmos.color = new Color32(0, 0, 150, 75);
        if (graph.vertices != null) {
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

}
