using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EdgeFireflies))]
public class EdgeFirefliesInspector : Editor {
	public EdgeFireflies edgeFireFliesScript;
	
	public override void OnInspectorGUI () {
		DrawDefaultInspector();
		edgeFireFliesScript = target as EdgeFireflies;
		if (GUILayout.Button("Calculate Emission Rate")) {
			Undo.RecordObject(edgeFireFliesScript,"Calculate Emission Rate");
			edgeFireFliesScript.SetEmissionByRadius();
			EditorUtility.SetDirty(edgeFireFliesScript);
		}
	}
}