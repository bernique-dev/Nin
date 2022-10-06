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
            tileGridManager.tileInfoToAdd.positionOnGrid = EditorGUILayout.Vector3IntField(GUIContent.none, tileGridManager.tileInfoToAdd.positionOnGrid);
            tileGridManager.tileInfoToAdd.tile = (Tile)EditorGUILayout.ObjectField(tileGridManager.tileInfoToAdd.tile, typeof(Tile), false);

            GUILayout.EndHorizontal();

            tileGridManager.tileInfoToAdd.rotation = Mathf.RoundToInt(EditorGUILayout.Slider(tileGridManager.tileInfoToAdd.rotation, 0, 270) / 90) * 90;

            if (GUILayout.Button("Add")) {
                tileGridManager.tileGrid.SetTile(tileGridManager.tileInfoToAdd.positionOnGrid, tileGridManager.tileInfoToAdd.tile, tileGridManager.tileInfoToAdd.rotation);
            }
        }


    }

}
