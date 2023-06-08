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
	public LevelInfoPopup infoPopup;
	public SeasonGlows seasonGlowsScript;
	public LevelFireflies levelFirefliesScript;
	private bool selectedOnce;
	public bool NE, SE, GE;
	public Sprite mySceneLogo;

	public void OnPointerClick(PointerEventData pointerEventData) {
		if (!selectedOnce) {
			selectedOnce = true;
			GetSceneInfo();
		}
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
			infoPopup.OpenTitle(this);
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
	public void GetSceneInfo() {
		if(sceneName == GlobalVariables.globVarScript.marketName){
			if(GlobalVariables.globVarScript.marketNE)
				NE = true;
			if(GlobalVariables.globVarScript.marketSE)
				SE = true;
			if(GlobalVariables.globVarScript.marketGE)
				GE = true;
		}else if(sceneName == GlobalVariables.globVarScript.parkName){
			if(GlobalVariables.globVarScript.parkNE)
				NE = true;
			if(GlobalVariables.globVarScript.parkSE)
				SE = true;
			if(GlobalVariables.globVarScript.parkGE)
				GE = true;
		}else if(sceneName == GlobalVariables.globVarScript.beachName){
			if(GlobalVariables.globVarScript.beachNE)
				NE = true;
			if(GlobalVariables.globVarScript.beachSE)
				SE = true;
			if(GlobalVariables.globVarScript.beachGE)
				GE = true;
		}
	}
}
