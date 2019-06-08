using UnityEditor;
using UnityEngine;

using Vector3 = PhysicsEngine.MathModule.Vector3;

[CustomPropertyDrawer(typeof(Vector3))]
public class Vector3Drawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        var xRect = new Rect(position.x + 12, position.y, 79, position.height);
        var yRect = new Rect(position.x + 106, position.y, 78, position.height);
        var zRect = new Rect(position.x + 199, position.y, 78, position.height);

        GUIStyle xColor = new GUIStyle(EditorStyles.label);
        xColor.normal.textColor = Color.red;

        GUIStyle yColor = new GUIStyle(EditorStyles.label);
        yColor.normal.textColor = Color.green;

        GUIStyle zColor = new GUIStyle(EditorStyles.label);
        zColor.normal.textColor = Color.blue;

        EditorGUI.LabelField(new Rect(position.x, position.y, position.width, position.height), "X", xColor);
        EditorGUI.PropertyField(xRect, property.FindPropertyRelative("x"), GUIContent.none);
        EditorGUI.LabelField(new Rect(position.x + 93, position.y, position.width, position.height), "Y", yColor);
        EditorGUI.PropertyField(yRect, property.FindPropertyRelative("y"), GUIContent.none);
        EditorGUI.LabelField(new Rect(position.x + 185, position.y, position.width, position.height), "Z", zColor);
        EditorGUI.PropertyField(zRect, property.FindPropertyRelative("z"), GUIContent.none);
        
        EditorGUI.indentLevel = 0;

        EditorGUI.EndProperty();
    }
}