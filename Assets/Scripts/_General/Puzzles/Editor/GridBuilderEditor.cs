using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GridBuilderScript)),CanEditMultipleObjects]
public class GridBuilderEditor : Editor {

	public override void OnInspectorGUI(){
		DrawDefaultInspector();
		GridBuilderScript myScrypt = (GridBuilderScript) target;
		
		if(!myScrypt.currentCell){
			if(GUILayout.Button("Build Grid")){
				myScrypt.CreateCell();
			}
		}
		EditorGUILayout.Space();
		GUILayout.BeginVertical("Box");
		GUILayout.BeginHorizontal("Box");
		EditorGUILayout.Space();
		if(GUILayout.Button("Build Up")){
			myScrypt.CreateUp();
		}
		EditorGUILayout.Space();
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal("Box");
		if(GUILayout.Button("Build left")){
			myScrypt.CreateLeft();
		}
		if(GUILayout.Button("Build Right")){
			myScrypt.CreateRight();
		}
		
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal("Box");
		EditorGUILayout.Space();
		if(GUILayout.Button("Build Down")){
			myScrypt.CreateDown();
		}
		EditorGUILayout.Space();
		GUILayout.EndHorizontal();
        GUILayout.EndVertical();
		EditorGUILayout.Space();
		GUILayout.BeginVertical("Box");
		if(GUILayout.Button("ResetValues")){
			myScrypt.ResetValues();
		}
		if(GUILayout.Button("Delete Grid")){
			myScrypt.DeleteGrid();
		}
		
		if(GUILayout.Button("Remove Grid Sprites")){
			myScrypt.RemoveSprites();
		}
		if(GUILayout.Button("Enable Grid Sprites")){
			myScrypt.EnableSprites();
		}
		if(GUILayout.Button("Remove Grid Colliders")){
			myScrypt.RemoveColliders();
		}
		if(GUILayout.Button("Enable Grid Colliders")){
			myScrypt.EnableColliders();
		}
        GUILayout.EndVertical();
	}
}
