using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KitePuzzEngine : MainPuzzleEngine 
{
	[Header("Kite Engine Values")]
	public int connections;
	public List<float> bGSizes;
	public BackgroundScale bgScleScript;
	public ClickToRotateTile clickToRotTileScript;
	
	// For Delegate Method
	private delegate void VoidDelegate();
	private VoidDelegate voidDelegate; 
	//private 
	[Header("Scripts")]
	public ResetTiles resetTilesScript;
	public KiteLevelChangeEvent kiteLevelChangeScript;

	void Start () {
		canPlay = false;
		initialSetupOn = true;
		maxLvl = GlobalVariables.globVarScript.puzzMaxLvl;
		tutorialDone = GlobalVariables.globVarScript.puzzIntroDone;
	}

	void Update () {
		if (canPlay) {
			if(mySelectButton.buttonPressed) {
				lvlToLoad = mySelectButton.lvlToLoad;
				if (chngLvlTimer >= setupLvlWaitTime && curntLvl != lvlToLoad && maxLvl >= lvlToLoad) {
					chngLvlTimer = 0f;
					ChangeLevelSetup();
				}
				mySelectButton.buttonPressed = false;
			}
			if (chngLvlTimer < setupLvlWaitTime) { chngLvlTimer += Time.deltaTime; /* Debug.Log("do I ever run? Or am I just lazy like that?"); */ }
			if (connections == clickToRotTileScript.connectionsNeeded) {
				SilverEggsSetup();
			}
			if (mySelectButton.buttonsOff) { mySelectButton.buttonsOff = false; mySelectButton.InteractableThreeDots(maxLvl,curntLvl); }
		}
		else {
			// When this Scene is loaded.
			if (initialSetupOn) { InitialSetup(); }

			// After the initial set up run the first sequence.
			if (iniSeqStart) {
				if (iniSeqDelay > 0) { iniSeqDelay -= Time.deltaTime; }
				else {
					seqTimer += Time.deltaTime;
					 if (seqTimer > itemSpawnF && !itemSpawnB) { itemSpawnB = true; /* lvlItemHolders[curntLvl - 1].SetActive(true); */ LvlStuffFadeIn(); bgScleScript.ScaleBG();}
					 if (seqTimer > dotsSpawnF && !dotsSpawnB) { dotsSpawnB = true; mySelectButton.EnabledThreeDots(maxLvl); mySelectButton.InteractableThreeDots(maxLvl,curntLvl); clickToRotTileScript.CalculateConnectionsNeeded();}
					 if (seqTimer > iniCanPlayF) {
						if (tutorialDone) {
							canPlay = true;
							mySelectButton.InteractableThreeDots(maxLvl, curntLvl);
							sceneTapScript.canTapPauseBtn = true;
						}
						else {
							slideInHelpScript.MoveBirdUpDown();
						}
						iniSeqStart = false;
					}
				}
			}

			if (itemsWait) {
				itemWaitTimer += Time.deltaTime;
				if (itemWaitTimer > itemWaitAmnt) {
					itemHolder.SetActive(true);
					clickToRotTileScript.connectionsNeeded = clickToRotTileScript.CalculateConnectionsNeeded(); // For fun, to try giving a method a return type for the first time.
					resetTilesScript.FillTileResetArray();
					LvlStuffFadeIn();
					itemsWait = false;
					itemWaitTimer = 0f;
					canPlay = true;
					mySelectButton.InteractableThreeDots(maxLvl,curntLvl);
				}
			}
			if (setupChsnLvl) { ChosenLevelSetup(lvlToLoad); }
			// Turn off interaction for all three level select dots.
			if (!mySelectButton.buttonsOff) { mySelectButton.buttonsOff = true; mySelectButton.UninteractableThreeDots(); }
		}

		if (waitMethod) {
			if (waitTimer > 0) 
			{ waitTimer -= Time.deltaTime; } 
			else { RunAfter(voidDelegate); waitMethod = false; }
		} 
	}

	#region Level Change Methods
	// Once, when the scene is openned.
	public new void InitialSetup() {
		if(maxLvl > 3 || maxLvl < 1) { curntLvl = 1; }
		else { curntLvl = maxLvl; }
		itemHolder = lvlItemHolders[curntLvl - 1];
		resetTilesScript.FillTileResetArray();
		initialSetupOn = false;
		iniSeqStart = true;
	}

	public new void SilverEggsSetup() {
		//Debug.Log("New Level Setup");
		canPlay = false;
		//Set the silver egg sprites to Hollow if the egg was found already.
		for (int i = 0; i < GlobalVariables.globVarScript.puzzSilEggsCount.Count; i++)
		{
			int eggNumber = GlobalVariables.globVarScript.puzzSilEggsCount[i];
			mySilverEggMan.allSilEggs[eggNumber].GetComponent<SpriteRenderer>().sprite = mySilverEggMan.hollowSilEgg;
			mySilverEggMan.allSilverEggsScripts[eggNumber].hollow = true;
			//Debug.Log(mySilverEggMan.allSilEggs[eggNumber].name + "has been set to hollow, ooouuuhhhh. Like a ghost. A nice ghost. Yeeah.");
		}
		mySilverEggMan.lvlSilverEggs[curntLvl - 1].SetActive(true); // CAN probably set it to true in the lvl finished seq or wtv
		if (mySilverEggMan.lvlSilverEggs[curntLvl - 1].transform.childCount > 0) {
			foreach (Transform silEgg in mySilverEggMan.lvlSilverEggs[curntLvl - 1].transform)
			{
				mySilverEggMan.activeSilverEggs.Add(silEgg.gameObject);
			}
		}
		mySilverEggMan.silverEggsActive = true;
		if (!mySelectButton.noFadeDelay) { mySelectButton.TurnFadeDelayOff(); mySelectButton.noFadeDelay = true; } // Turn off the initial fade delay for the three dots. Should only happen once.
		connections = 0;
		EndOfLevelEvent();
	}

	// Checks if the player tapped enough silver eggs to move on, change the current level.
	public override void SilverEggsCheck() {
		if (mySilverEggMan.activeSilverEggs.Count > 0) {
			if (mySilverEggMan.amntSilEggsTapped == mySilverEggMan.activeSilverEggs.Count) {			
				mySilverEggMan.activeSilverEggs.Clear();
				mySilverEggMan.silverEggsActive = false;
				mySilverEggMan.amntSilEggsTapped = 0;
				curntLvl++;
				if (curntLvl != winLvl) {
					scrnDarkImgScript.FadeOut();
				}
				if (curntLvl > maxLvl) { 
					maxLvl = curntLvl; SaveMaxLvl(); mySelectButton.EnabledThreeDots(maxLvl); 
				}
				voidDelegate = NextLevelSetup;
				if (!waitMethod) { waitMethod = true; } else { Debug.LogError("waitMethod IS ALREADY IN PROGRESS, DONT DO THAT!!"); }
				waitTimer = waitTime;
				if (curntLvl  < winLvl) { 
					bgScleScript.ScaleBG();
				}
			}
		}
	}

	// Lets you feed set a method as a parameter in a method.
	void RunAfter(VoidDelegate methToRun) {
		methToRun();
	}

	// Once animations are finished, run the next level setup.
	public new void NextLevelSetup() {
		foreach(SilverEggs silEggs in mySilverEggMan.lvlSilverEggs[curntLvl - 2].GetComponentsInChildren<SilverEggs>())
		{ silEggs.ResetSilEgg(); /* Debug.Log(silEggs.gameObject.name); */}
		mySilverEggMan.lvlSilverEggs[curntLvl - 2].SetActive(false);
		resetTilesScript.EndOfLevelReset();
		chngLvlTimer = 0f;
		if (curntLvl >= winLvl) {
			puzzleCompScript.endSeq = true;
			return;
		}
		itemHolder.SetActive(false);
		itemHolder = lvlItemHolders[curntLvl - 1];
		itemsWait = true;
	}

	// Prepare to change level after a level selection button has been pressed.
	public new void ChangeLevelSetup() {
		// Close up current level.
		canPlay = false;
		mySelectButton.UninteractableThreeDots();
		LvlStuffFadeOut();
		setupChsnLvl = true;
	}

	// Setup the chosen level after waiting for setupLvlWaitTime (minimum the fade out duration of the items).
	public new void ChosenLevelSetup(int lvlToLoad) {
		// Setup chosen level.
		chngLvlTimer += Time.deltaTime;
		if (chngLvlTimer > setupLvlWaitTime) {
			lvlItemHolders[curntLvl - 1].SetActive(false);
			resetTilesScript.EndOfLevelReset();
			curntLvl = lvlToLoad;
			itemHolder = lvlItemHolders[curntLvl - 1];
			itemsWait = true;
			bgScleScript.ScaleBG();
			setupChsnLvl = false;
			chngLvlTimer = 0;
		}
	}
	#endregion

	#region General Methods
	public void EndOfLevelEvent() {
		kiteLevelChangeScript.LevelChangeEvent();
	}
	
	public new void LvlStuffFadeIn() {
		levelsStuff[curntLvl -1].StartLvlFadeIn();
		//Debug.Log("Should fade in stuff."); // Fade in tiles
		if (!lvlItemHolders[curntLvl -1].activeSelf) lvlItemHolders[curntLvl -1].SetActive(true);
	}

	public new void LvlStuffFadeOut() { // Fade out tiles, tile backs, kite, backshadow. 
		levelsStuff[curntLvl -1].ExitFadeOutLvl();
	}

	public new void SaveMaxLvl() {
		if (maxLvl > GlobalVariables.globVarScript.puzzMaxLvl) {
			GlobalVariables.globVarScript.puzzMaxLvl = maxLvl;
			GlobalVariables.globVarScript.SaveEggState();
		}
	}
	#endregion

	// #region Coroutines
	// // All silver eggs picked up, what happenes?
	// public new IEnumerator PuzzleComplete () {
	// 	yield return new WaitForSeconds(0.5f);

	// 	Debug.Log("Puzzle Completed cognraturations!!!");

	// 	yield return new WaitForSeconds(0.5f);

	// 	audioSceneParkPuzzScript.StopSceneMusic();
	// 	audioSceneParkPuzzScript.PlayTransitionMusic();

	// 	GlobalVariables.globVarScript.sceneFadeScript.SwitchScene(GlobalVariables.globVarScript.parkName);
	// }
	// #endregion
}