using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LevelCompleteEggSpawner))]
public class LevelCompleteEggSpawnerInspector : Editor {
	public LevelCompleteEggSpawner levelCompleteEggSpawnerScript;

	public override void OnInspectorGUI () {
		DrawDefaultInspector();
		levelCompleteEggSpawnerScript = target as LevelCompleteEggSpawner;
		if (GUILayout.Button("Calculate Egg Spawn Delays")) {
			Undo.RecordObject(levelCompleteEggSpawnerScript,"Calculate Egg Spawn Delays");
			levelCompleteEggSpawnerScript.CalculateIntervals();
			EditorUtility.SetDirty(levelCompleteEggSpawnerScript);
		}
	}
}
