using System;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(StrumPattern))]
public class StrumPatternPropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);
        
        // Remove the default indent for a clean look
        var originalIndentLevel = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;
        
        int strumCount = 8;
        
        // Display a label for the whole pattern
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
        
        // Calculate the rects for each of the 6 fields
        float fieldWidth = position.width / strumCount;
        Rect fieldRect = new Rect(position.x, position.y, fieldWidth, position.height);
        
        // Loop through and draw each enum field side-by-side
        for (int i = 0; i < strumCount; i++)
        {
            string propName = "strum_" + (i + 1);
            EditorGUI.PropertyField(fieldRect, property.FindPropertyRelative(propName), GUIContent.none);
            fieldRect.x += fieldWidth;
        }
        
        EditorGUI.indentLevel = originalIndentLevel;
        EditorGUI.EndProperty();
    }
}