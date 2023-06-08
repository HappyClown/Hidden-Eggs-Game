using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ClickOnEggs))]
public class ClickOnEggsInspector : Editor {
	public ClickOnEggs clickOnEggs;

	public override void OnInspectorGUI () {
		DrawDefaultInspector();
		clickOnEggs = target as ClickOnEggs;
		if (GUILayout.Button("PlayLvlCompleteSeq")) {
			Undo.RecordObject(clickOnEggs,"PlayLvlCompleteSeq");
			clickOnEggs.PlayLvlCompleteSeq();
			EditorUtility.SetDirty(clickOnEggs);
		}
	}
}
