using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using System.Linq;

public class ScriptFinder : MonoBehaviour {
	public List<GameObject> allGOInScene = new List<GameObject>();
	public List<SpriteColorFade> spriteColorFadeScripts = new List<SpriteColorFade>();
	public List<FadeInOutSprite> fadeInOutSpriteScripts = new List<FadeInOutSprite>();
	public List<TMPTextColorFade> tmpTextColorFadeScripts = new List<TMPTextColorFade>();
	public List<TMPWarpText> tmpWarpTextScripts = new List<TMPWarpText>();
	public List<LevelCompleteEggMoveSpin> lvlCompEggMoveScripts = new List<LevelCompleteEggMoveSpin>();
	public List<LevelCompEggAnimEvents> lvlCompEggAnimScripts = new List<LevelCompEggAnimEvents>();

	#if UNITY_EDITOR
	public void FillSceneGameObjectList() {
		Debug.Log("The time at the start of this method: " + Time.time);
		allGOInScene.Clear();
		//allGOInScene = UnityEngine.Object.FindObjectsOfType<GameObject>().ToList();

		//allGOInScene = Resources.FindObjectsOfTypeAll(typeof(GameObject)).ToList() as GameObject[];
		foreach (GameObject go in Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[])
		{
			if (go.hideFlags == HideFlags.NotEditable || go.hideFlags == HideFlags.HideAndDontSave) {
                continue;
			}
			if (EditorUtility.IsPersistent(go.transform.root.gameObject)) {
       			continue;
			}
			if (go.name == "InternalIdentityTransform") {
				continue;
			}
			allGOInScene.Add(go);
		}
		

		// List<Transform> allTransInScene = new List<Transform>();
		// var s = SceneManager.GetActiveScene();
		// allTransInScene = s.FindObjectsOfTypeAll<Transform>().ToList();
		// foreach(Transform trans in allTransInScene)
		// {
		// 	allGOInScene.Add(trans.gameObject);
		// }
		Debug.Log("The time at the start of this method: " + Time.time);
	}

	public void SpriteColorFadeRefFinder() {
		Debug.Log("The time at the start of this method: " + Time.time);
		spriteColorFadeScripts.Clear();
		foreach(GameObject go in allGOInScene)
		{
			if (go.GetComponent<SpriteColorFade>()) {
				spriteColorFadeScripts.Add(go.GetComponent<SpriteColorFade>());
			}
		}
		if (spriteColorFadeScripts.Count > 0) {
			foreach(SpriteColorFade spriteColorFadeScript in spriteColorFadeScripts)
			{
				Undo.RecordObject(spriteColorFadeScript, "Assign SpriteColorFade material reference");
				spriteColorFadeScript.GetMaterialRef();
				EditorUtility.SetDirty(spriteColorFadeScript);
			}
		}
		Debug.Log("The time at the start of this method: " + Time.time);
		//Debug.Log("Caution: If changes were made to a prefab you should apply them.");
	}
	
	public void FadeInOutSpriteRefFinder() {
		Debug.Log("The time at the start of this method: " + Time.time);
		fadeInOutSpriteScripts.Clear();
		foreach(GameObject go in allGOInScene)
		{
			if (go.GetComponent<FadeInOutSprite>()) {
				fadeInOutSpriteScripts.Add(go.GetComponent<FadeInOutSprite>());
			}
		}
		if (fadeInOutSpriteScripts.Count > 0) {
			foreach(FadeInOutSprite fadeInOutSpriteScript in fadeInOutSpriteScripts)
			{
				Undo.RecordObject(fadeInOutSpriteScript, "Assign FadeInOutSprite sprite reference");
				fadeInOutSpriteScript.GetSpriteRef();
				EditorUtility.SetDirty(fadeInOutSpriteScript);
			}
		}
		Debug.Log("The time at the start of this method: " + Time.time);
	}

	public void TMPTextColorFadeRefFinder() {
		Debug.Log("The time at the start of this method: " + Time.time);
		tmpTextColorFadeScripts.Clear();
		foreach(GameObject go in allGOInScene)
		{
			if (go.GetComponent<TMPTextColorFade>()) {
				tmpTextColorFadeScripts.Add(go.GetComponent<TMPTextColorFade>());
			}
		}
		if (tmpTextColorFadeScripts.Count > 0) {
			foreach(TMPTextColorFade tmpTextColorFadeScript in tmpTextColorFadeScripts)
			{
				Undo.RecordObject(tmpTextColorFadeScript, "Assign TMPTextColorFade start references");
				tmpTextColorFadeScript.StartSetup();
				EditorUtility.SetDirty(tmpTextColorFadeScript);
			}
		}
		Debug.Log("The time at the start of this method: " + Time.time);
	}

	public void TMPWarpTextRefFinder() {
	Debug.Log("The time at the start of this method: " + Time.time);
	tmpWarpTextScripts.Clear();
	foreach(GameObject go in allGOInScene)
	{
		if (go.GetComponent<TMPWarpText>()) {
			tmpWarpTextScripts.Add(go.GetComponent<TMPWarpText>());
		}
	}
	if (tmpWarpTextScripts.Count > 0) {
		foreach(TMPWarpText tmpWarpTextScript in tmpWarpTextScripts)
		{
			Undo.RecordObject(tmpWarpTextScript, "Assign TMPWarpText start references");
			tmpWarpTextScript.StartSetup();
			EditorUtility.SetDirty(tmpWarpTextScript);
		}
	}
	Debug.Log("The time at the start of this method: " + Time.time);
	}

	public void TempLvlCompEggMoveRefFinder() {
	Debug.Log("The time at the start of this method: " + Time.time);
	lvlCompEggMoveScripts.Clear();
	foreach(GameObject go in allGOInScene)
	{
		if (go.GetComponent<LevelCompleteEggMoveSpin>()) {
			lvlCompEggMoveScripts.Add(go.GetComponent<LevelCompleteEggMoveSpin>());
		}
	}
	if (lvlCompEggMoveScripts.Count > 0) {
		foreach(LevelCompleteEggMoveSpin lvlCompEggMoveScript in lvlCompEggMoveScripts)
		{
			Undo.RecordObject(lvlCompEggMoveScript, "Assign LevelCompleteEggMoveSpin references");
			lvlCompEggMoveScript.GetReferences();
			EditorUtility.SetDirty(lvlCompEggMoveScript);
		}
	}
	Debug.Log("The time at the end of this method: " + Time.time);
	}

	public void LvlCompEggAnimRefFinder() {
		Debug.Log("The time at the start of this method: " + Time.time);
		lvlCompEggAnimScripts.Clear();
		foreach(GameObject go in allGOInScene)
		{
			if (go.GetComponent<LevelCompEggAnimEvents>()) {
				lvlCompEggAnimScripts.Add(go.GetComponent<LevelCompEggAnimEvents>());
			}
		}
		if (lvlCompEggAnimScripts.Count > 0) {
			foreach(LevelCompEggAnimEvents lvlCompEggAnimScript in lvlCompEggAnimScripts)
			{
				Undo.RecordObject(lvlCompEggAnimScript, "Assign LevelCompEggAnim references");
				lvlCompEggAnimScript.GetReferences();
				EditorUtility.SetDirty(lvlCompEggAnimScript);
			}
		}
		Debug.Log("The time at the end of this method: " + Time.time);
	}
	#endif
}
