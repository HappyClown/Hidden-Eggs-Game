using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LevelCompleteEggVariables))]
[CanEditMultipleObjects]
public class LevelCompleteEggVariablesEditor : Editor {
	public LevelCompleteEggVariables LvlCompEggVariables;

	public override void OnInspectorGUI() {
		DrawDefaultInspector();
		LvlCompEggVariables = target as LevelCompleteEggVariables;
		if (GUILayout.Button("Get References")) {
			Undo.RecordObject(LvlCompEggVariables,"Get References");
			LvlCompEggVariables.GetReferences();
			EditorUtility.SetDirty(LvlCompEggVariables);
		}
		
	}
}