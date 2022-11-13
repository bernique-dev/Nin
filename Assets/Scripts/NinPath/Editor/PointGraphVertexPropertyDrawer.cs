using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(PointGraphVertex))]
public class PointGraphVertexPropertyDrawer : PropertyDrawer {

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
        return base.GetPropertyHeight(property, label);
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {

        SerializedProperty originProperty = property.FindPropertyRelative("origin");
        SerializedProperty destinationProperty = property.FindPropertyRelative("destination");
        SerializedProperty weightProperty = property.FindPropertyRelative("weight");
        SerializedProperty isBidirectionalProperty = property.FindPropertyRelative("isBidirectional");

        float pointFieldWidth = 4 * position.width / 10;
        Rect originRect = new Rect(position.min, new Vector2(pointFieldWidth, position.height));
        EditorGUI.ObjectField(originRect, originProperty, GUIContent.none);
        Rect destinationRect = new Rect(position.min + new Vector2(originRect.width, 0), new Vector2(pointFieldWidth, position.height));
        EditorGUI.ObjectField(destinationRect, destinationProperty, GUIContent.none);
        Rect weightRect = new Rect(position.min + new Vector2(originRect.width + destinationRect.width, 0),
                                new Vector2(position.width - (originRect.width + destinationRect.width + position.width/20), position.height));
        GUIStyle floatStyle = new GUIStyle(EditorStyles.textField);
        floatStyle.alignment = TextAnchor.MiddleRight;
        weightProperty.intValue = EditorGUI.IntField(weightRect, weightProperty.intValue, floatStyle);
        //Rect bidirectionalRect = new Rect(position.min + new Vector2(originRect.width + destinationRect.width + weightRect.width + 4, 0),
        //                        new Vector2(position.width - (originRect.width + destinationRect.width + weightRect.width), position.height));
        Rect bidirectionalRect = new Rect(position.max - new Vector2(position.height, position.height),
                                new Vector2(position.height, position.height));
        isBidirectionalProperty.boolValue = EditorGUI.Toggle(bidirectionalRect, isBidirectionalProperty.boolValue);

    }

}
