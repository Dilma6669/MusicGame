using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(Fret))]
public class FretPropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Tell Unity that this property should be drawn as a single line
        EditorGUI.BeginProperty(position, label, property);
        
        // Remove the default indent so the field is left-aligned with our label
        var originalIndentLevel = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;
        
        // Calculate the rects for the two fields.
        float labelWidth = 40f;
        float fieldWidth = (position.width - labelWidth * 2) / 2f;
        
        Rect stringLabelRect = new Rect(position.x, position.y, labelWidth, position.height);
        Rect stringFieldRect = new Rect(position.x + labelWidth, position.y, fieldWidth, position.height);
        
        Rect fretLabelRect = new Rect(position.x + labelWidth + fieldWidth, position.y, labelWidth, position.height);
        Rect fretFieldRect = new Rect(position.x + labelWidth * 2 + fieldWidth, position.y, fieldWidth, position.height);
        
        // Draw the fields and labels
        EditorGUI.LabelField(stringLabelRect, new GUIContent("String"));
        EditorGUI.PropertyField(stringFieldRect, property.FindPropertyRelative("stringRow"), GUIContent.none);
        
        EditorGUI.LabelField(fretLabelRect, new GUIContent("Fret"));
        EditorGUI.PropertyField(fretFieldRect, property.FindPropertyRelative("fretNumber"), GUIContent.none);
        
        // Reset the indent to its original value
        EditorGUI.indentLevel = originalIndentLevel;
        EditorGUI.EndProperty();
    }
}