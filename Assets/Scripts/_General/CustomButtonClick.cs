using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class CustomButtonClick : MonoBehaviour, IPointerClickHandler {
	public string sceneName;
	public int myLvlNumber;
	public bool levelSelected = false;
	public Transform myCenterPoint;
	[Tooltip("Populate with all the OTHER level's CustomButtonClick scripts.")]
	public List<CustomButtonClick> customButtonClickScripts;
	public ZoomAtHub myZoom;
	public LevelTitleVillage myTitle;
	public SeasonGlows seasonGlowsScript;
	public LevelFireflies levelFirefliesScript;

	public void OnPointerClick(PointerEventData pointerEventData) {
		// Debug.Log("Button clicked on " + pointerEventData.pointerCurrentRaycast.gameObject.name);
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
			myTitle.OpenTitle();
			levelFirefliesScript.PlayLevelFireflies();
		}
		else {		
			myZoom.StartZoom(new Vector3(myCenterPoint.position.x, myCenterPoint.position.y, myZoom.myCam.transform.position.z));
			OpenScene();
		}
	}

	public void OpenScene () {
		GlobalVariables.globVarScript.sceneFadeScript.SwitchScene(sceneName);
	}
}
