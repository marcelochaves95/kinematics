using UnityEditor;
using UnityEngine;

namespace Kinematics.MathModule.Editor
{
    [CustomPropertyDrawer(typeof(Quaternion))]
    public class QuaternionDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            var xRect = new Rect(position.x + 12, position.y, 79, position.height);
            var yRect = new Rect(position.x + 106, position.y, 78, position.height);
            var zRect = new Rect(position.x + 199, position.y, 78, position.height);
            var wRect = new Rect(position.x + 199, position.y, 78, position.height);

            EditorGUI.LabelField(new Rect(position.x, position.y, position.width, position.height), "X");
            EditorGUI.PropertyField(xRect, property.FindPropertyRelative("x"), GUIContent.none);
            EditorGUI.LabelField(new Rect(position.x + 93, position.y, position.width, position.height), "Y");
            EditorGUI.PropertyField(yRect, property.FindPropertyRelative("y"), GUIContent.none);
            EditorGUI.LabelField(new Rect(position.x + 185, position.y, position.width, position.height), "Z");
            EditorGUI.PropertyField(zRect, property.FindPropertyRelative("z"), GUIContent.none);
            EditorGUI.LabelField(new Rect(position.x + 185, position.y, position.width, position.height), "W");
            EditorGUI.PropertyField(zRect, property.FindPropertyRelative("w"), GUIContent.none);

            EditorGUI.indentLevel = 0;

            EditorGUI.EndProperty();
        }
    }
}
