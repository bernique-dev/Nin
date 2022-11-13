using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(NinbazBuildingManager))]
public class NinbazBuildingManagerEditor : Editor {

    public override void OnInspectorGUI() {
        //base.OnInspectorGUI();
        NinbazBuildingManager ninbazBuildingManager = (NinbazBuildingManager)target;
        ninbazBuildingManager.ninPrefab = (GameObject)EditorGUILayout.ObjectField(ninbazBuildingManager.ninPrefab, typeof(GameObject), false);

        if (GUILayout.Button("Create Nin")) {
            ninbazBuildingManager.CreateNin();
        }

    }

}
