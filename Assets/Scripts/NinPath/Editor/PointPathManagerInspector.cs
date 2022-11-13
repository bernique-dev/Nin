using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PointPathManager))]
public class PointPathManagerInspector : Editor {


    bool alwaysCalculateDistances = false;
    bool showDistances = true;
    bool showNexts = true;

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        PointPathManager pointPathManager = (PointPathManager)target;
        PointPath path = pointPathManager.path;

        GUILayout.Label("Distance = " + pointPathManager.path.distance);

        if (path.distance > 0) {
            showDistances = EditorGUILayout.Foldout(showDistances, "Distances");
            if (showDistances) {
                alwaysCalculateDistances = EditorGUILayout.Toggle("Always calculate", alwaysCalculateDistances);
                if (alwaysCalculateDistances ? true : GUILayout.Button("Calculate Distances")) {
                    pointPathManager.path.CalculateDistances();
                }
                foreach (KeyValuePair<Point, float> kvp in path.pointsAndDistance) {
                    GUILayout.Label(kvp.Key.gameObject.name + " - " + kvp.Value);
                }
            }
            showNexts = EditorGUILayout.Foldout(showNexts, "Nexts");
            if (showNexts) {
                if (GUILayout.Button("Calculate Nexts")) {
                    pointPathManager.path.SetNextPoints();
                }
                foreach (KeyValuePair<Point, Point> kvp in path.pointsAndNext) {
                    GUILayout.Label(kvp.Key.gameObject.name + " - " + kvp.Value.gameObject.name);
                }
            }

        }


        //EditorUtility.SetDirty(pointPathManager);
    }

}
