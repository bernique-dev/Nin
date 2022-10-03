using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DikoDisplayer))]
public class DikoDisplayerInspector : Editor {

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        bool update = false;
        DikoDisplayer dikoDisplayer = (DikoDisplayer)target;
        if (GUILayout.Button("Sort")) {
            dikoDisplayer.diko.Sort();
            update = true;
        }
        if (GUILayout.Button("Update")) {
            update = true;
        }

        if (update) dikoDisplayer.UpdateDevinisions();
    }

}
