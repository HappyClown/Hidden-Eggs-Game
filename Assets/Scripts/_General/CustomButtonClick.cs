using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class CustomButtonClick : MonoBehaviour, IPointerClickHandler {
	public string sceneName;
	public SeasonGlows seasonGlowsScript;
	public int myLvlNumber;
	public bool levelSelected = false;
	public Transform myCenterPoint;
	public ZoomAtHub myZoom;
	[Tooltip("Populate with all the OTHER level's CustomButtonClick scripts.")]
	public List<CustomButtonClick> customButtonClickScripts;

	public void OnPointerClick(PointerEventData pointerEventData) {
		Debug.Log("Button clicked on " + pointerEventData.pointerCurrentRaycast.gameObject.name);
		// Load proper scene
		//OpenScene();
		//Select Level / deselect other levels
		if (!levelSelected) {
			foreach(CustomButtonClick customButtonClickScript in customButtonClickScripts)
			{
				customButtonClickScript.levelSelected = false;
				seasonGlowsScript.LevelSelect(myLvlNumber);
			}
			levelSelected = true;
		}
		else {
			myZoom.StartZoom(myCenterPoint.position);
			OpenScene();
		}
	}

	public void OpenScene () {
		SceneFade.SwitchScene(sceneName);
	}
}
