using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ClamPuzzle : MainPuzzleEngine {
	public BeachClamLevel Tutorial;
	public BeachClamLevel[] myLvls;
	public List<BeachClam> openedClams;
	public List<BeachClam> currentClams;
	public ClamLevelChangeEvent clamLevelChangeScript;
	public AudioSceneBeachPuzzle audioSceneBeachPuzzScript;
	void Start () {
		StartSetup();	
		audioSceneBeachPuzzScript =  GameObject.Find ("Audio").GetComponent<AudioSceneBeachPuzzle>();
	}

	void Update () {
		if(resetLevel){
			clamLevelChangeScript.ResetChestSprite();
			myLvls[curntLvl-1].ResetLevel();	
			resetLevel = false;
		}
		if(setupLevel){
			myLvls[curntLvl-1].SetUpLevel();					
			clamLevelChangeScript.chest.sortingLayerName = "Default";
			clamLevelChangeScript.coral.sortingLayerName = "Default";
			setupLevel = false;
		}
		if (canPlay) {
			RunBasics(canPlay);	
			if (myLvls[curntLvl-1].levelComplete) {
				/* Debug.Log("ya win m8!"); */
				/*if(!tutorialDone){
					tutorialDone = true;
				}
				SilverEggsSetup();
				clamLevelChangeScript.LevelChangeEvent();*/
				if(tutorialDone){
					SilverEggsSetup();
					clamLevelChangeScript.LevelChangeEvent();
				}
				else{
					if(myTutorial.tutorialFinished){
						chngLvlTimer = 0;
						lvlToLoad = curntLvl+1;
						maxLvl = lvlToLoad;
						SaveMaxLvl(); 
						tutorialDone = true;
						selectButtonInOut.MoveInOut();
						mySelectButton.EnabledThreeDots(maxLvl); 
						mySelectButton.InteractableThreeDots(lvlToLoad,maxLvl);
						ChangeLevelSetup();
					}
					
				}
			}
			#region Click
			//check if player tapped
			if(myInput.Tapped) {
				UpdateMousePos();
				hit = Physics2D.Raycast(mousePos2D, Vector3.forward, 50f);
				if (hit) {
					Debug.Log(hit.collider.gameObject.name);
					if (hit.collider.CompareTag("Puzzle")) {
						if (openedClams.Count > 0) {
							openedClams[0].forceClose = true;
							openedClams[1].forceClose = true;
							openedClams.Clear();
						}
						BeachClam tappedClam = hit.collider.gameObject.GetComponent<BeachClam>();
						currentClams.Add(tappedClam);
						tappedClam.Tapped = true;
						if (currentClams.Count == 2) {
							if (tappedClam.myMatch.open) {
								tappedClam.matched = true;
								tappedClam.myMatch.matched = true;
								myLvls[curntLvl -1].CheckClams();
								currentClams.Clear();
							}
							else {
								currentClams[0].failed = true;
								currentClams[1].failed = true;
								openedClams.Add(currentClams[0]);
								openedClams.Add(currentClams[1]);
								currentClams.Clear();
							}

						}
					}
				}
			}
			#endregion
		}else{			
			RunBasics(canPlay);	
		}			
	}
	
	#region Singular Puzzle Methods

	#endregion
}
