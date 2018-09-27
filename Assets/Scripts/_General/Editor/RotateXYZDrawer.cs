using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(RotateVariables))]
public class RotateXYZDrawer : PropertyDrawer 
{
	public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
	{
		float totalHeight;
		totalHeight = 3 * 15;

		return totalHeight;
	}

	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		EditorGUI.BeginProperty(position, label, property);

		var indent = EditorGUI.indentLevel;
		EditorGUI.indentLevel = 0;

		var rotXBool = new Rect(position.x, position.y, 10, EditorGUI.GetPropertyHeight(property, label, true));
		 Debug.Log("rotXBool width = " + rotXBool.width);
		rotXBool.width = 5;
		rotXBool.xMax = 30;
		var rotYBool = new Rect(position.x, position.y + 15, position.width - 50, EditorGUI.GetPropertyHeight(property, label, true));
		 Debug.Log("rotYBool width = " + rotYBool.width);
		rotYBool.width = 500;
		var rotZBool = new Rect(position.x, position.y + 30, 150, EditorGUI.GetPropertyHeight(property, label, true));

		var rotXSpeed = new Rect(position.x + 150, position.y, position.width, EditorGUI.GetPropertyHeight(property, label, true));
		rotXSpeed.xMin = 150;
		var rotYSpeed = new Rect(position.x + 150, position.y + 15, position.width, EditorGUI.GetPropertyHeight(property, label, true));
		rotYSpeed.xMin = 150;
		var rotZSpeed = new Rect(position.x + 150, position.y + 30, position.width, EditorGUI.GetPropertyHeight(property, label, true));
		rotZSpeed.xMin = 150;

		EditorGUIUtility.labelWidth = 110f;
		//EditorGUIUtility.fieldWidth = 150f;

		EditorGUI.PropertyField (rotXBool, property.FindPropertyRelative("rotateInX"), new GUIContent ("Rotate X:"), true);
		EditorGUI.PropertyField (rotYBool, property.FindPropertyRelative("rotateInY"), new GUIContent ("Rotate Y:"));
		EditorGUI.PropertyField (rotZBool, property.FindPropertyRelative("rotateInZ"), new GUIContent ("Rotate Z:"));

		EditorGUI.PropertyField (rotXSpeed, property.FindPropertyRelative("rotationSpeedX"), new GUIContent ("Rotation Speed X:"));
		EditorGUI.PropertyField (rotYSpeed, property.FindPropertyRelative("rotationSpeedY"), new GUIContent ("Rotation Speed Y:"));
		EditorGUI.PropertyField (rotZSpeed, property.FindPropertyRelative("rotationSpeedZ"), new GUIContent ("Rotation Speed Z:"));

		EditorGUI.indentLevel = indent;

		EditorGUI.EndProperty();
	}
}
