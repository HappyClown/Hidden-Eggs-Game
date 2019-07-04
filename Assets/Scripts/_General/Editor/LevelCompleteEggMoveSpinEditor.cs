using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LevelCompleteEggMoveSpin))]
[CanEditMultipleObjects]
public class LevelCompleteEggMoveSpinEditor : Editor {
	public LevelCompleteEggMoveSpin LvlCompEggMoveSpinScript;

	public override void OnInspectorGUI() {
		DrawDefaultInspector();
		LvlCompEggMoveSpinScript = target as LevelCompleteEggMoveSpin;
		if (GUILayout.Button("Get References")) {
			Undo.RecordObject(LvlCompEggMoveSpinScript,"Get References");
			LvlCompEggMoveSpinScript.GetReferences();
			EditorUtility.SetDirty(LvlCompEggMoveSpinScript);
		}
		
	}
}
