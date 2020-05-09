using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(UnityAttribute))]
public class UnityDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var labelAttribute = attribute as UnityAttribute;
        EditorGUI.PropertyField(position, property, label);
        GUI.Label(new Rect(labelAttribute.width + 2f, position.y, position.width, position.height), labelAttribute.label, labelAttribute.labelStyle);
    }
}