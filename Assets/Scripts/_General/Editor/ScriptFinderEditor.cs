using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ScriptFinder))]
[CanEditMultipleObjects]
public class ScriptFinderEditor : Editor {
	public ScriptFinder scriptFinderScript;
	
	public override void OnInspectorGUI () {
		DrawDefaultInspector();
		scriptFinderScript = target as ScriptFinder;
		if (GUILayout.Button("Get All Objects")) {
			Undo.RecordObject(scriptFinderScript,"Get All Objects");
			scriptFinderScript.FillSceneGameObjectList();
			EditorUtility.SetDirty(scriptFinderScript);
		}
		if (GUILayout.Button("Set SpriteColorFade Ref")) {
			Undo.RecordObject(scriptFinderScript,"Set SpriteColorFade Ref");
			scriptFinderScript.SpriteColorFadeRefFinder();
			EditorUtility.SetDirty(scriptFinderScript);
		}
		if (GUILayout.Button("Set FadeInOutSprite Ref")) {
			Undo.RecordObject(scriptFinderScript,"Set FadeInOutSprite Ref");
			scriptFinderScript.FadeInOutSpriteRefFinder();
			EditorUtility.SetDirty(scriptFinderScript);
		}
		if (GUILayout.Button("Set TMPTextColorFade Ref")) {
			Undo.RecordObject(scriptFinderScript,"Set TMPTextColorFade Ref");
			scriptFinderScript.TMPTextColorFadeRefFinder();
			EditorUtility.SetDirty(scriptFinderScript);
		}
		if (GUILayout.Button("Set TMPWarpText Ref")) {
			Undo.RecordObject(scriptFinderScript,"Set TMPWarpText Ref");
			scriptFinderScript.TMPWarpTextRefFinder();
			EditorUtility.SetDirty(scriptFinderScript);
		}
		if (GUILayout.Button("Set LvlCompEggMove Ref")) {
			Undo.RecordObject(scriptFinderScript,"Set LvlCompEggMove Ref");
			scriptFinderScript.TempLvlCompEggMoveRefFinder();
			EditorUtility.SetDirty(scriptFinderScript);
		}
		if (GUILayout.Button("Set LvlCompEggAnim Ref")) {
			Undo.RecordObject(scriptFinderScript,"Set LvlCompEggAnim Ref");
			scriptFinderScript.LvlCompEggAnimRefFinder();
			EditorUtility.SetDirty(scriptFinderScript);
		}
	}
}