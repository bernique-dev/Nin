using System.Linq;
using System;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Diko))]
public class DikoInspector : Editor {

    private Devinision tmpDevinision;

    private int selectedDevinisionIndex;
    private bool isModifying;


    public override void OnInspectorGUI() {
        Diko diko = (Diko)target;
        //base.OnInspectorGUI();

        if (tmpDevinision == null) {
            tmpDevinision = new Devinision();
            tmpDevinision.nindaVersion = "";
            tmpDevinision.humanVersion = "";
            tmpDevinision.commentary = "";
        }

        EditorGUILayout.BeginHorizontal();
        GUI.SetNextControlName("nindaVersion");
        tmpDevinision.nindaVersion = EditorGUILayout.TextField(tmpDevinision.nindaVersion);
        tmpDevinision.humanVersion = EditorGUILayout.TextField(tmpDevinision.humanVersion);
        EditorGUILayout.EndHorizontal();
        tmpDevinision.commentary = EditorGUILayout.TextArea(tmpDevinision.commentary, GUILayout.Height(120));

        GUI.enabled = tmpDevinision.nindaVersion.Length > 0;
        if (GUILayout.Button(isModifying ? "Modify" : "Add")) {
            if (isModifying) {
                isModifying = false;
            } else {
                diko.Add(tmpDevinision);
            }
            diko.Sort();
            diko.lastModificationDate = DateTime.Now;
            tmpDevinision = null;
            serializedObject.Update();
            GUI.FocusControl("nindaVersion");
        }
        GUI.enabled = true;
        DrawHorizontalBar();

        EditorGUILayout.BeginHorizontal();
        string[] devinisionNames = diko.devinisions.Select(dev => dev.nindaVersion).ToArray();
        string[] devinisionStrings = diko.devinisions.Select(dev => dev.ToString()).ToArray();
        selectedDevinisionIndex = EditorGUILayout.Popup(selectedDevinisionIndex, devinisionStrings);

        if (GUILayout.Button("Modify")) {
            Devinision devinisionToDelete = diko.devinisions.First(dev => dev.nindaVersion == devinisionNames[selectedDevinisionIndex]);
            tmpDevinision = devinisionToDelete;
            isModifying = true;
            serializedObject.Update();
        }

        if (GUILayout.Button("Delete")) {
            Devinision devinisionToDelete = diko.devinisions.First(dev => dev.nindaVersion == devinisionNames[selectedDevinisionIndex]);
            diko.devinisions.Remove(devinisionToDelete);
            serializedObject.Update();
        }

        EditorGUILayout.EndHorizontal();

        SerializedProperty list = serializedObject.FindProperty("devinisions");
        EditorGUILayout.PropertyField(list, true);
        EditorUtility.SetDirty(diko);
    }

    public void DrawHorizontalBar() {
        var rect = EditorGUILayout.BeginHorizontal();
        Handles.color = Color.gray;
        Handles.DrawLine(new Vector2(rect.x - 15, rect.y), new Vector2(rect.width + 15, rect.y));
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space();
    }

}
