using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(FretPattern))]
public class FretPatternPropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);
        
        var originalIndentLevel = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;
        
        // Display a label for the whole pattern
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        int stringCount = 6;
        
        // Calculate the rects for each of the 6 fields
        float fieldWidth = position.width / stringCount;
        Rect fieldRect = new Rect(position.x, position.y, fieldWidth, position.height);
        
        // Loop through and draw each enum field side-by-side
        for (int i = 0; i < stringCount; i++)
        {
            string propName = "string_" + (i + 1) + "_Fret";
            EditorGUI.PropertyField(fieldRect, property.FindPropertyRelative(propName), GUIContent.none);
            fieldRect.x += fieldWidth;
        }
        
        EditorGUI.indentLevel = originalIndentLevel;
        EditorGUI.EndProperty();
    }
}