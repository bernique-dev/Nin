using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LoadingManager))]
class LoadingManagerInspector : Editor {

    public override void OnInspectorGUI() {
        //base.OnInspectorGUI();
        LoadingManager loadingManager = (LoadingManager)target;

        if (loadingManager.isLoadingFinished) {
            GUILayout.Label("Loaded in " + TimeParser.GetParsedString(loadingManager.loadingTime, TimeParser.TimeFormat.OPTIONAL_HOUR));
        } else {
            GUILayout.Label("Loading...");
        }
    }

}
