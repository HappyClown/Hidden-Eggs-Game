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
		// Select Level
		if (!levelSelected) {
			GetSceneInfo();
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
			NE = GlobalVariables.globVarScript.marketNE;
			SE = GlobalVariables.globVarScript.marketSE;
			GE = GlobalVariables.globVarScript.marketGE;
		}else if(sceneName == GlobalVariables.globVarScript.parkName){
			NE = GlobalVariables.globVarScript.parkNE;
			SE = GlobalVariables.globVarScript.parkSE;
			GE = GlobalVariables.globVarScript.parkGE;
		}else if(sceneName == GlobalVariables.globVarScript.beachName){
			NE = GlobalVariables.globVarScript.beachNE;
			SE = GlobalVariables.globVarScript.beachSE;
			GE = GlobalVariables.globVarScript.beachGE;
		}
	}
}
