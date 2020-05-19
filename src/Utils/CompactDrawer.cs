using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(CompactAttribute))]
public class CompactDrawer : PropertyDrawer {

  public override void OnGUI(Rect position, SerializedProperty prop, GUIContent label) {

		EditorGUIUtility.LookLikeControls();
		position.xMin += 4;
		position.xMax -= 4;

		EditorGUI.BeginProperty(position, label, prop);
		EditorGUI.BeginChangeCheck();

		switch (prop.type) {
			case "Vector3f":
				var newV3 = EditorGUI.Vector3Field(position, label.text, prop.vector3Value);
				if (EditorGUI.EndChangeCheck()) {
					prop.vector3Value = newV3;
				}
				break;
			case "Vector2f":
				var newV2 = EditorGUI.Vector2Field(position, label.text, prop.vector2Value);
				if (EditorGUI.EndChangeCheck()) {
					prop.vector2Value = newV2;
				}
				break;
			case "Quaternionf":
				var newV4 = EditorGUI.Vector4Field(position, label.text, QuaternionToVector4(prop.quaternionValue));
				if (EditorGUI.EndChangeCheck()) {
					prop.quaternionValue = ConvertToQuaternion(newV4);
				}
				break;
			default:

				EditorGUI.HelpBox(position, "[Compact] doesn't work with type '" + prop.type + "' (Supported: Vector2, Vector3, Quaternion)", MessageType.Error);
				break;
		}

		EditorGUI.EndProperty();

	}

	private Quaternion ConvertToQuaternion(Vector4 v4) {
		return new Quaternion(v4.x, v4.y, v4.z, v4.w);
	}
	private Vector4 QuaternionToVector4(Quaternion q) {
		return new Vector4(q.x, q.y, q.z, q.w);
	}	

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
       float extraHeight = 20f;
       return base.GetPropertyHeight(property, label) + extraHeight;
    }

}
