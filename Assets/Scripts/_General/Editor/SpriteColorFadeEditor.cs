using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SpriteColorFade))]
[CanEditMultipleObjects]
public class SpriteColorFadeEditor : Editor {
	// public SpriteColorFade spriteColorFadeScript;

	// public override void OnInspectorGUI() {
	// 	DrawDefaultInspector();
	// 	spriteColorFadeScript = target as SpriteColorFade;
	// 	if (GUILayout.Button("Get Sprite Reference")) {
	// 		Undo.RecordObject(spriteColorFadeScript,"Get Sprite Reference");
	// 		spriteColorFadeScript.GetMaterialRef();
	// 		EditorUtility.SetDirty(spriteColorFadeScript);
	// 	}
		
	// }
}
