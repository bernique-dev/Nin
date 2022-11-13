using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PointPathFollower))]
public class PointPathFollowerInspector : Editor {

    bool showDistances = false;

    public override void OnInspectorGUI() {
        //base.OnInspectorGUI();

        PointPathFollower pointPathFollower = (PointPathFollower)target;
        PointPath path = pointPathFollower.path;
        PointGraph graph = pointPathFollower.graph;

        pointPathFollower.isGoing = EditorGUILayout.Toggle(pointPathFollower.isGoing);

        pointPathFollower.graphManager = (PointGraphManager)EditorGUILayout.ObjectField(pointPathFollower.graphManager, typeof(PointGraphManager), true);

        GUI.enabled = pointPathFollower.graphManager && pointPathFollower.hasPointsToGo;

        if (graph != null) {
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Calculate Path")) {
                pointPathFollower.CalculateShortestPath(pointPathFollower.origin, pointPathFollower.destination);
            }
            pointPathFollower.origin = (Point)EditorGUILayout.ObjectField(pointPathFollower.origin, typeof(Point), true, GUILayout.Width(120));
            pointPathFollower.destination = (Point)EditorGUILayout.ObjectField(pointPathFollower.destination, typeof(Point), true, GUILayout.Width(120));
            EditorGUILayout.EndHorizontal();

            if (GUILayout.Button("Calculate Path from here to random Destination")) {
                pointPathFollower.CalculateShortestPathFromHereToRandomPoint();
            }
            if (GUILayout.Button("Calculate random path")) {
                pointPathFollower.CalculateRandomPath();
            }

        }

        if (path.distance > 0) {
            //pointPathFollower.distanceProgress = EditorGUILayout.Slider(pointPathFollower.distanceProgress, 0, 1);
            pointPathFollower.distanceFromStart = EditorGUILayout.Slider(pointPathFollower.distanceFromStart, 0, pointPathFollower.path != null ? pointPathFollower.path.distance : 0);
            GUILayout.Label("Distance from start = " + pointPathFollower.distanceFromStart + "(" + (pointPathFollower.distanceProgress * 100).ToString("F0") + "%)");
            GUILayout.Label("Last point = " + path.GetLastPointFromDistance(pointPathFollower.distanceFromStart));

            showDistances = EditorGUILayout.Foldout(showDistances, "Distances");
            if (showDistances) {
                List<KeyValuePair<Point, float>> kvps = path.pointsAndDistance.ToList();
                kvps.Sort((kvp1, kvp2) => kvp1.Value.CompareTo(kvp2.Value));
                foreach (KeyValuePair<Point, float> kvp in kvps) {
                    GUILayout.Label(kvp.Key.gameObject.name + " - " + kvp.Value);
                }
            }
        }

        EditorUtility.SetDirty(pointPathFollower);
    }


}
