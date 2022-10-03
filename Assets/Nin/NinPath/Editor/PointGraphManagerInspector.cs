using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PointGraphManager))]
public class PointGraphManagerInspector : Editor {

    private Point selectedPoint;
    private PointGraphVertex vertex;

    private bool drawVertices;
    private bool previousDrawVertices;

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        EditorGUI.BeginChangeCheck();

        PointGraphManager pointGraphManager = (PointGraphManager)target;

        //if (GUILayout.Button(drawVertices ? "Draw Vertices" : "STOP", drawVertices ? toggleButtonStyleToggled : toggleButtonStyleNormal)) {
        //    drawVertices = !drawVertices;
        //}

        GUILayout.BeginHorizontal();
        drawVertices = GUILayout.Toggle(drawVertices, drawVertices ? "Stop drawing" : "Draw Vertices", "Button");
        pointGraphManager.drawBidirectional = EditorGUILayout.Toggle(pointGraphManager.drawBidirectional, GUILayout.Width(15));
        GUILayout.EndHorizontal();

        if (previousDrawVertices != drawVertices) {
            ActiveEditorTracker.sharedTracker.isLocked = drawVertices;
            //ActiveEditorTracker.sharedTracker.ForceRebuild();
            if (drawVertices) {
                Selection.activeGameObject = null;
            } else {
                Selection.activeGameObject = pointGraphManager.gameObject;
            }
        }
        if (drawVertices) {
            if (vertex == null) vertex = new PointGraphVertex();
            GameObject go = Selection.activeGameObject;
            if (go) {
                Point tmpPoint = go.GetComponent<Point>();
                if (tmpPoint) {
                    selectedPoint = tmpPoint;
                    if (vertex.origin == null) {
                        vertex.origin = selectedPoint;
                        Selection.activeGameObject = null;
                    } else if (vertex.origin != selectedPoint) {
                        vertex.destination = selectedPoint;
                        vertex.isBidirectional = pointGraphManager.drawBidirectional;
                        Selection.activeGameObject = null;
                        pointGraphManager.graph.vertices.Add(vertex);
                        vertex = null;
                    }
                    if (!pointGraphManager.graph.points.Contains(selectedPoint)) {
                        pointGraphManager.graph.points.Add(selectedPoint);
                    }
                } else {
                    Selection.activeGameObject = selectedPoint.gameObject;
                }
            }
            GUIStyle style = new GUIStyle(EditorStyles.label);
            style.alignment = TextAnchor.MiddleCenter;
            GUILayout.Label(vertex != null ? (vertex.origin + "->" + vertex.destination) : "no vertex", style);
        } else {
            vertex = null;
        }
        previousDrawVertices = drawVertices;

        if (pointGraphManager.pointQueues != null) {
            foreach (KeyValuePair<Point, Queue<PointPathFollower>> kvp in pointGraphManager.pointQueues) {
                GUILayout.Label(kvp.Key + " - " + kvp.Value);
            }
        }

        if (EditorGUI.EndChangeCheck()) {
            EditorUtility.SetDirty(pointGraphManager);
        }
    }


}
