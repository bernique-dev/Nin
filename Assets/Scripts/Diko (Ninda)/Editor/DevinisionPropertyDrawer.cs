using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(Devinision))]
public class DevinisionPropertyDrawer : PropertyDrawer {

    public static float heightMultiplier = 5;
    public static float propertyHeightMultiplier {
        get {
            return heightMultiplier + verticalGap * 3;
        }
    }

    public static float verticalGap = 1.5f;


    public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
        SerializedProperty isFoldedOut = property.FindPropertyRelative(nameof(Devinision.isFoldedOut));
        return base.GetPropertyHeight(property, label) * (isFoldedOut.boolValue ? propertyHeightMultiplier : 1);
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {

        SerializedProperty nindaVersionSP = property.FindPropertyRelative(nameof(Devinision.nindaVersion));
        SerializedProperty humanVersionSP = property.FindPropertyRelative(nameof(Devinision.humanVersion));
        SerializedProperty commentarySP = property.FindPropertyRelative(nameof(Devinision.commentary));
        SerializedProperty isFoldedOut = property.FindPropertyRelative(nameof(Devinision.isFoldedOut));
        Rect blankRect = new Rect(position.min, new Vector2(position.width, position.height/ (isFoldedOut.boolValue ? propertyHeightMultiplier : 1)));
        isFoldedOut.boolValue = EditorGUI.Foldout(blankRect, isFoldedOut.boolValue, (nindaVersionSP.stringValue.Length > 0 ? nindaVersionSP.stringValue : "???") + " - " + (humanVersionSP.stringValue.Length > 0 ? humanVersionSP.stringValue : "???"));


        Rect propertyRect = new Rect(position.min + Vector2.up * blankRect.height, new Vector2(position.width, position.height - blankRect.height));
        Handles.BeginGUI();
        //Handles.DrawSolidRectangleWithOutline(blankRect, new Color32(0, 0, 0, 0), Color.yellow);
        //Handles.DrawSolidRectangleWithOutline(propertyRect, new Color32(0, 0, 0, 0), Color.red);
        Handles.EndGUI();
        if (isFoldedOut.boolValue) {

            EditorGUI.BeginProperty(propertyRect, label, property);

            Vector2 textFieldDimensions = new Vector2(propertyRect.width, propertyRect.height / 5);

            GUIStyle textFieldStyle = new GUIStyle(EditorStyles.textField);
            textFieldStyle.alignment = TextAnchor.MiddleLeft;
            GUIStyle textAreatyle = new GUIStyle(EditorStyles.textArea);
            textAreatyle.alignment = TextAnchor.UpperLeft;

            Rect nindaRect = new Rect(propertyRect.min, textFieldDimensions);
            nindaVersionSP.stringValue = EditorGUI.TextField(nindaRect, nindaVersionSP.stringValue, textFieldStyle);
            Rect humanRect = new Rect(propertyRect.min + Vector2.up * (textFieldDimensions.y + verticalGap), textFieldDimensions);
            humanVersionSP.stringValue = EditorGUI.TextField(humanRect, humanVersionSP.stringValue, textFieldStyle);
            Rect commentaryRect = new Rect(propertyRect.min + Vector2.up * (textFieldDimensions.y * 2 + verticalGap * 2.5f), new Vector2(textFieldDimensions.x, textFieldDimensions.y * (heightMultiplier / 2)));
            commentarySP.stringValue = EditorGUI.TextArea(commentaryRect, commentarySP.stringValue, textAreatyle);

            EditorGUI.EndProperty();
        }

        




    }


}
