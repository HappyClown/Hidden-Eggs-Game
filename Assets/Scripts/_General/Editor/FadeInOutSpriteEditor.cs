using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(FadeInOutSprite))]
[CanEditMultipleObjects]
public class FadeInOutSpriteEditor : Editor {
	public FadeInOutSprite fadeInOutSpriteScript;

	public override void OnInspectorGUI() {
		DrawDefaultInspector();
		if (!EditorApplication.isPlaying) {
			fadeInOutSpriteScript = target as FadeInOutSprite;
			//0 = startShow, 1 = startHidden
			if ((int)fadeInOutSpriteScript.myStartState == 0) {
				fadeInOutSpriteScript.shown = true;
				fadeInOutSpriteScript.hidden = false;
				EditorUtility.SetDirty(this);
			}
			else if ((int)fadeInOutSpriteScript.myStartState == 1) {
				fadeInOutSpriteScript.hidden = true;
				fadeInOutSpriteScript.shown = false;
				EditorUtility.SetDirty(this);
			}
		}
		
	}
}
