using UnityEditor;
using UnityEngine;

namespace Kinematics.MathModule.Editor
{
    [CustomPropertyDrawer(typeof(Vector3))]
    public class Vector3Drawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            Rect xRect = new Rect(position.x + 12, position.y, 79, position.height);
            Rect yRect = new Rect(position.x + 106, position.y, 78, position.height);
            Rect zRect = new Rect(position.x + 199, position.y, 78, position.height);

            EditorGUI.LabelField(new Rect(position.x, position.y, position.width, position.height), "X", SetColor(Color.white));
            EditorGUI.PropertyField(xRect, property.FindPropertyRelative("x"), GUIContent.none);
            EditorGUI.LabelField(new Rect(position.x + 93, position.y, position.width, position.height), "Y", SetColor(Color.white));
            EditorGUI.PropertyField(yRect, property.FindPropertyRelative("y"), GUIContent.none);
            EditorGUI.LabelField(new Rect(position.x + 185, position.y, position.width, position.height), "Z", SetColor(Color.white));
            EditorGUI.PropertyField(zRect, property.FindPropertyRelative("z"), GUIContent.none);

            EditorGUI.indentLevel = 0;

            EditorGUI.EndProperty();
        }

        private GUIStyle SetColor(Color color)
        {
            return new GUIStyle(EditorStyles.label)
            {
                normal = { textColor = color }
            };
        }
    }
}