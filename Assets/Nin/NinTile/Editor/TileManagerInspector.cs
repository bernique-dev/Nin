using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TileManager))]
public class TileManagerInspector : Editor {

    private Point selectedPoint;
    private PointGraphVertex vertex;

    private bool drawVertices;
    private bool selectNonLinkedPoints;
    private bool previousDrawVertices;
    private bool previousSelectNonLinkedPoints;

    // Start is called before the first frame update
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        EditorGUI.BeginChangeCheck();

        TileManager tileManager = (TileManager)target;

        //if (GUILayout.Button(drawVertices ? "Draw Vertices" : "STOP", drawVertices ? toggleButtonStyleToggled : toggleButtonStyleNormal)) {
        //    drawVertices = !drawVertices;
        //}


        GUILayout.BeginHorizontal();
        drawVertices = GUILayout.Toggle(drawVertices, drawVertices ? "Stop drawing" : "Draw Vertices", "Button");
        selectNonLinkedPoints = GUILayout.Toggle(selectNonLinkedPoints, selectNonLinkedPoints ? "Stop unlink" : "Unlink Points", "Button") && !drawVertices;
        GUILayout.EndHorizontal();

        ActiveEditorTracker.sharedTracker.isLocked = drawVertices || selectNonLinkedPoints;

        if (drawVertices != previousDrawVertices || selectNonLinkedPoints != previousSelectNonLinkedPoints) {
            if (drawVertices || selectNonLinkedPoints) {
                Selection.activeGameObject = null;
            }
            else {
                Selection.activeGameObject = tileManager.gameObject;
            }
        }

        GameObject go = Selection.activeGameObject;
        if (drawVertices) {
            if (vertex == null) vertex = new PointGraphVertex();
            if (go) {
                Point tmpPoint = go.GetComponent<Point>();
                if (tmpPoint) {
                    bool isVertexWalker = vertex.origin == null ? tmpPoint is WalkerPoint : vertex.origin is WalkerPoint;
                    selectedPoint = tmpPoint;
                    if (vertex.origin == null) {
                        vertex.origin = selectedPoint;
                        Selection.activeGameObject = null;
                    }
                    else if (vertex.origin != selectedPoint) {
                        vertex.destination = selectedPoint;
                        Selection.activeGameObject = null;
                        (isVertexWalker ? tileManager.walkerVertices : tileManager.vehicleVertices).Add(vertex);
                        vertex = null;
                    }
                    List<Point> pointList = isVertexWalker ? tileManager.walkerPoints : tileManager.vehiclePoints;
                    if (!pointList.Contains(selectedPoint)) {
                        pointList.Add(selectedPoint);
                    }
                } else {
                    Selection.activeGameObject = selectedPoint.gameObject;
                }
            }
            GUIStyle style = new GUIStyle(EditorStyles.label);
            style.alignment = TextAnchor.MiddleCenter;
            GUILayout.Label(vertex != null ? (vertex.origin + "->" + vertex.destination) : "no vertex", style);
        } else if (selectNonLinkedPoints) {
            if (go) {
                Point point = go.GetComponent<Point>();
                if (point) {
                    bool isPointWalker = point is WalkerPoint;
                    List<Point> pointsToNotLink = isPointWalker ? tileManager.walkerPointsToNotLink : tileManager.vehiclePointsToNotLink;
                    if (!pointsToNotLink.Contains(point)) {
                        pointsToNotLink.Add(point);
                    }
                    Selection.activeGameObject = null;
                }
            }
        }
        else {
            vertex = null;
        }
        previousDrawVertices = drawVertices;
        previousSelectNonLinkedPoints = selectNonLinkedPoints;


        if (EditorGUI.EndChangeCheck()) {
            EditorUtility.SetDirty(tileManager);
        }
    }
}
