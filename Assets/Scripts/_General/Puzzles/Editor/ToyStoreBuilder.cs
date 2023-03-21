using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ToyStoreLevelBuilderScript)),CanEditMultipleObjects]
public class ToyStoreBuilder : Editor {
   public ToyStoreLevelBuilderScript myScrypt;
    // Start is called before the first frame update
   public override void OnInspectorGUI(){
		DrawDefaultInspector();
        myScrypt = (ToyStoreLevelBuilderScript) target;
        if(GUILayout.Button("Start Selecting")){
			myScrypt.StartSelecting(true);
		}
        EditorGUILayout.Space();
        if(GUILayout.Button("Stop Selecting")){
			myScrypt.StartSelecting(false);
		}
         EditorGUILayout.Space();
         if(GUILayout.Button("Add / Remove Selected press K")){
			myScrypt.AddSelectedCell(Selection.activeTransform.gameObject);
		}
        EditorGUILayout.Space();
         if(GUILayout.Button("Empty Goals")){
			myScrypt.EmptyGoals();
		}
   }
   void OnSceneGUI() {
        Event e = Event.current;
        if(EventType.KeyDown == e.type && e.keyCode == KeyCode.K)
         {
             myScrypt.AddSelectedCell(Selection.activeTransform.gameObject);
         }
   }
}
