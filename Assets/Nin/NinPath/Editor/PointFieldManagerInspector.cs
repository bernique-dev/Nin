using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PointFieldManager))]
public class PointFieldManagerInspector : Editor {

    int verticesToDeleteNumber = 5;

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        PointFieldManager pointFieldManager = (PointFieldManager)target;
        if (GUILayout.Button("Generate Field")) {
            pointFieldManager.GenerateField();
        }
        if (pointFieldManager.graphManager) {
            if (GUILayout.Button("Generate Graph")) {
                pointFieldManager.GenerateGraph();
            }

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Delete Vertices")) {
                pointFieldManager.DeleteRandomVertices(verticesToDeleteNumber);
            }
            verticesToDeleteNumber = EditorGUILayout.IntField(verticesToDeleteNumber, GUILayout.Width(80));
            GUILayout.EndHorizontal();
        }
    }

}
