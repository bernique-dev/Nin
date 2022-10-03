using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PointPathManager : MonoBehaviour {

    public PointPath path;

    public bool showPathWhenNotSelected;

    private void Awake() {
        path.CalculateDistances();
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
        Gizmos.color = new Color32(255, 255, 255, 75);
        List<Point> points = path.GetPoints();
        for (int i = 1; i < points.Count; i++) {
            Point previousPoint = points[i - 1];
            Point currentPoint = points[i];
            Gizmos.DrawLine(previousPoint.position, currentPoint.position);
        }
    }
}