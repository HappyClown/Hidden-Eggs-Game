using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class CustonButtonClick : MonoBehaviour, IPointerClickHandler
{
	public string sceneName;
	public SeasonGlows seasonGlowsScript;
	[Tooltip("It needs to match its index in the SeasonGlows levelGlowScripts list. Starting from 0. ")]
	public int myLvlNumber;

	public void OnPointerClick(PointerEventData pointerEventData) {
		Debug.Log("Button clicked on " + pointerEventData.pointerCurrentRaycast.gameObject.name);
		// Load proper scene
		OpenScene();
	}

	public void OpenScene () {
		SceneFade.SwitchScene(sceneName);
		seasonGlowsScript.LevelSelect(myLvlNumber);
	}
}
