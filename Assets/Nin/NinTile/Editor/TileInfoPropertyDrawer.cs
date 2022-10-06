using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(TileInfo))]
public class TileInfoPropertyDrawer : PropertyDrawer {

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {


        SerializedProperty positionProperty = property.FindPropertyRelative(nameof(TileInfo.positionOnGrid));
        SerializedProperty tileProperty = property.FindPropertyRelative(nameof(TileInfo.tile));
        SerializedProperty boolProperty = property.FindPropertyRelative(nameof(TileInfo.willBeInstantiated));
        EditorGUI.BeginProperty(position, label, property);

        Rect positionRect = new Rect(position.min, new Vector2(position.width / 2, position.height));
        Rect tileRect = new Rect(position.min + Vector2.right * (positionRect.width + 5), new Vector2(position.width / 2 - 20, position.height));

        Rect boolRect = new Rect(position.min + Vector2.right * (positionRect.width + tileRect.width + 10), new Vector2(10, position.height));

        //Debug.Log(positionProperty + "-" + tileProperty);
        positionProperty.vector3IntValue = EditorGUI.Vector3IntField(positionRect, GUIContent.none, positionProperty.vector3IntValue);

        //tileProperty.objectReferenceValue = EditorGUI.ObjectField(tileRect, tileProperty.objectReferenceValue, typeof(Tile), false);

        EditorGUI.PropertyField(tileRect, tileProperty, GUIContent.none);

        boolProperty.boolValue = EditorGUI.Toggle(boolRect, boolProperty.boolValue);

        Handles.BeginGUI();
        //Handles.DrawSolidRectangleWithOutline(position, new Color32(0, 0, 0, 0), Color.red);
        Handles.EndGUI();


        EditorGUI.EndProperty();

    }

}