using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TileGridManager))]
public class TileGridManagerInspector : Editor {



    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        TileGridManager tileGridManager = (TileGridManager)target;

        tileGridManager.doesTilesFoldout = EditorGUILayout.Foldout(tileGridManager.doesTilesFoldout, "Tiles");
        if (tileGridManager.doesTilesFoldout) {
            GUILayout.BeginHorizontal();
            tileGridManager.tileInfoToAdd.position = EditorGUILayout.Vector3IntField(GUIContent.none, tileGridManager.tileInfoToAdd.position);
            tileGridManager.tileInfoToAdd.tile = (Tile)EditorGUILayout.ObjectField(tileGridManager.tileInfoToAdd.tile, typeof(Tile), false);

            GUILayout.EndHorizontal();
            if (GUILayout.Button("Add")) {
                tileGridManager.tileGrid.SetTile(tileGridManager.tileInfoToAdd.position, tileGridManager.tileInfoToAdd.tile);
            }
        }


    }

}
