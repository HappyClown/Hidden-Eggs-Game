using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RotationBurst))]
[CanEditMultipleObjects]
public class RotationBurstEditor : Editor {
	// public RotationBurst RotationBurstScript;

	// public override void OnInspectorGUI() {
	// 	DrawDefaultInspector();
	// 	RotationBurstScript = target as RotationBurst;
	// 	if (GUILayout.Button("Set First Randoms")) {
	// 		Undo.RecordObject(RotationBurstScript,"Set First Randoms");
	// 		RotationBurstScript.SetFirstRandoms();
	// 		EditorUtility.SetDirty(RotationBurstScript);
	// 	}
		
	// }
}
