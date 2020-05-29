using System;
using UnityEditor;
using UnityEngine;

namespace Kinematics.Collision.Editor
{
    [CustomEditor(typeof(Rigidbody))]
    public class RigidbodyEditor : UnityEditor.Editor
    {
        private SerializedObject rigidbody;

        private SerializedProperty mass;
        private SerializedProperty drag;
        private SerializedProperty useGravity;
        private SerializedProperty isKinematic;
        private SerializedProperty x;
        private SerializedProperty y;
        private SerializedProperty z;
        private SerializedProperty velocity;
        private SerializedProperty position;
        private SerializedProperty rotation;
        private SerializedProperty constraints;

        private readonly GUIContent freezePositionLabel = EditorGUIUtility.TrTextContent("Freeze Position");
        private readonly GUIContent freezeRotationLabel = EditorGUIUtility.TrTextContent("Freeze Rotation");
        private static float kLabelFloatMaxW => (float) (EditorGUIUtility.labelWidth + EditorGUIUtility.fieldWidth + 5.0);
        private const float kSingleLineHeight = 18f;
        private bool requiresConstantRepaint;
        private bool isExpanded;

        public void OnEnable()
        {
            mass = serializedObject.FindProperty("mass");
            drag = serializedObject.FindProperty("drag");
            useGravity = serializedObject.FindProperty("useGravity");
            isKinematic = serializedObject.FindProperty("isKinematic");
            x = serializedObject.FindProperty("x");
            y = serializedObject.FindProperty("y");
            z = serializedObject.FindProperty("z");
            velocity = serializedObject.FindProperty("velocity");
            position = serializedObject.FindProperty("position");
            rotation = serializedObject.FindProperty("rotation");
            constraints = serializedObject.FindProperty("constraints");
            requiresConstantRepaint = false;
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            ShowBodyInfoProperties();

            Rect position = EditorGUILayout.GetControlRect();
            isExpanded = EditorGUI.Foldout(position, isExpanded, "Freeze Constraints", true);
            serializedObject.Update();
            RigidbodyConstraints rbConstraints = (RigidbodyConstraints) constraints.intValue;
            
            if (isExpanded)
            {
                EditorGUI.indentLevel++;
                ToggleBlock(rbConstraints, freezePositionLabel, 1, 2, 3);
                ToggleBlock(rbConstraints, freezeRotationLabel, 4, 5, 6);
                EditorGUI.indentLevel--;
                constraints.intValue = (int) rbConstraints;
            }

            serializedObject.ApplyModifiedProperties();
        }

        private void ConstraintToggle(Rect r, string label, RigidbodyConstraints value, int bit)
        {
            bool toggle = ((int) value & (1 << bit)) != 0;
            EditorGUI.BeginChangeCheck();
            int oldIndent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;
            constraints.intValue = Convert.ToInt32(EditorGUI.ToggleLeft(r, label, toggle));
            EditorGUI.indentLevel = oldIndent;
            EditorGUI.showMixedValue = false;
        }
        
        private void ToggleBlock(RigidbodyConstraints constraints, GUIContent label, int x, int y, int z)
        {
            const int toggleOffset = 30;
            using (new GUILayout.HorizontalScope())
            {
                Rect rect = GUILayoutUtility.GetRect(EditorGUIUtility.fieldWidth, kLabelFloatMaxW, kSingleLineHeight, kSingleLineHeight, EditorStyles.numberField);
                int id = GUIUtility.GetControlID(7231, FocusType.Keyboard, rect);
                rect = EditorGUI.PrefixLabel(rect, id, label);
                rect.width = toggleOffset;
                ConstraintToggle(rect, "X", constraints, x);
                rect.x += toggleOffset;
                ConstraintToggle(rect, "Y", constraints, y);
                rect.x += toggleOffset;
                ConstraintToggle(rect, "Z", constraints, z);
            }
        }

        private void ShowBodyInfoProperties()
        {
            requiresConstantRepaint = false;
            EditorGUILayout.FloatField("Mass", mass.floatValue);
            EditorGUILayout.FloatField("Drag", drag.floatValue);
            EditorGUILayout.Toggle("Use Gravity", useGravity.boolValue);
            EditorGUILayout.Toggle("Is Kinematic", isKinematic.boolValue);
            /*EditorGUILayout.Vector3Field("Velocity", velocity.vector3Value);
            EditorGUILayout.Vector3Field("Position", position.vector3Value);
            EditorGUILayout.Vector3Field("Rotation", rotation.vector3Value);*/

            if (EditorApplication.isPlaying)
            {
                requiresConstantRepaint = true;
            }
        }
    }
}
