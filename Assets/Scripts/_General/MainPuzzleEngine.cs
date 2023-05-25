using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainPuzzleEngine : MonoBehaviour 
{
	#region Old vars
	public RaycastHit2D hit;
	public Vector2 mousePos2D;
	public Vector3 mousePos;
	public bool tutorialDone;
	public int curntLvl;
	#endregion
	#region Basic Scripts Sources
	[Header("Input Detector")]
	public inputDetector myInput;
	[Header("Silver Eggs")]
	public SilverEggsManager mySilverEggMan;
	[Header("Selection Buttons")]
	public LevelSelectionButtons mySelectButton;
	[Header("Fade in out variables")]
	public FadeInOutManager[] levelsStuff;
	#endregion
	#region GrabItem Script Variables
	[Header("General")]
	public int winLvl;
	public bool itemsWait;
	public float itemWaitAmnt;
	[HideInInspector]
	public float itemWaitTimer;
	[HideInInspector]
	public bool initialSetupOn;
	[HideInInspector]
	public bool setupChsnLvl;
	// For Delegate Method
	public delegate void VoidDelegate();
	public VoidDelegate voidDelegate; 
	[HideInInspector]
	public bool waitMethod;
	[Tooltip("For now, minimum the lenght of the silver egg tap anim.")] 
	public float waitTime;
	[HideInInspector]
	public float waitTimer;

	[Header("Item Parents")]
	[Tooltip("GameObjects - Game objects that hold the level items, in ascending order.")] 
	public List<GameObject> lvlItemHolders;
	public GameObject itemHolder;

	[Header("Initial Sequence")]
	public float seqTimer;
	public float iniSeqDelay, crateDownF, reqDownF, itemSpawnF, dotsSpawnF, iniCanPlayF;
	[HideInInspector]
	public bool iniSeqStart, crateDownB, reqDownB, itemSpawnB, dotsSpawnB;

	[Header("Scripts")]
	public FadeInOutImage scrnDarkImgScript;
	public SceneTapEnabler sceneTapScript;
	public MenuStatesManager menuStatesScript;
	public SlideInHelpBird slideInHelpScript;
	public PuzzleComplete puzzleCompScript;

	[Header("Hide In Inspector ^_^")]
	public bool canPlay, resetLevel, setupLevel, finishedLevel;
	public int lvlToLoad;
	public float setupLvlWaitTime;
	public float chngLvlTimer;
	public int maxLvl;
	#endregion

	public AudioSceneParkPuzzle audioSceneParkPuzzScript;

	public void StartSetup () {
		//initialize puzzle as not playable
		canPlay = false;
		//initial setup controls sequeces for loading the puzzle
		initialSetupOn = true;
		//-----------check with X how it works again 
		maxLvl = GlobalVariables.globVarScript.puzzMaxLvl;
		//Check if the tutorial is been done before
		tutorialDone = GlobalVariables.globVarScript.puzzIntroDone;
		//controls whenever a level has to be reseted
		resetLevel = false;
		//controls if a level needs to be setted up
		setupLevel = false;
		//Set helperbird in puzzle mode
		slideInHelpScript.inPuzzle = true;
	}
	
	public void RunBasics (bool playing) {
		//General logic to run while the game is playable
		if (playing) {
			//General ui buttons actions			
			if(mySelectButton.buttonPressed) {	
				//check if loading is done and the selected button is different than the current level and less than the max level			
				if (chngLvlTimer >= setupLvlWaitTime && curntLvl != lvlToLoad && maxLvl >= lvlToLoad){
					//assign the level number to load depending on the presed button
					lvlToLoad = mySelectButton.lvlToLoad;
					//reset the level timer for sequences
					chngLvlTimer = 0f;
					//Run changing level function
					ChangeLevelSetup();
				}
				mySelectButton.buttonPressed = false;
			}
			if (chngLvlTimer < setupLvlWaitTime) { 
				chngLvlTimer += Time.deltaTime;
				/* Debug.Log("do I ever run? Or am I just lazy like that?"); */ 
			}
			if (mySelectButton.buttonsOff) { 
				mySelectButton.buttonsOff = false; 
				mySelectButton.InteractableThreeDots(maxLvl,curntLvl); 
			}
		}
		else {
			// Run the first setup when the puzzle is open
			if (initialSetupOn) { 
				Debug.Log("initializing..");
				InitialSetup(); }
			// After the initial set up run the first sequence will play.
			if (iniSeqStart) {
				//run a delay time for everything to be ready
				if (iniSeqDelay > 0) { 
					iniSeqDelay -= Time.deltaTime; 
				}
				else {
					//after the delay, run timer for level sequence
					seqTimer += Time.deltaTime;
					//compare sequence timer to level items spawn time, then set the level items bool to true to start fading in the items
					if (seqTimer > itemSpawnF && !itemSpawnB) { 
						itemSpawnB = true; 
						LvlStuffFadeIn(); 
						setupLevel = true;
					}
					//compare sequence timer to spawn Ui time, then enable ui buttons
					if (seqTimer > dotsSpawnF && !dotsSpawnB) { 
						dotsSpawnB = true; 
						mySelectButton.EnabledThreeDots(maxLvl); 
						mySelectButton.InteractableThreeDots(maxLvl,curntLvl);
					}
					 //compare sequence timer to playable time, then allow player to play and finish the initial sequence
					if (seqTimer > iniCanPlayF) { 
						//----------Helper bird stuff here
						if (tutorialDone) {
							canPlay = true; 
							mySelectButton.InteractableThreeDots(maxLvl, curntLvl);
							sceneTapScript.canTapPauseBtn = true;
						}else{
							slideInHelpScript.MoveBirdUpDown();
						}
						iniSeqStart = false;						
					}
				}
			}
			//wait controller for any level switch
			if (itemsWait) {
				//running timer to  wait
				itemWaitTimer += Time.deltaTime;
				if (itemWaitTimer > itemWaitAmnt) {
					//activate selected level items once timer is done and make it playable
					itemHolder.SetActive(true);
					LvlStuffFadeIn();
					itemsWait = false;
					itemWaitTimer = 0f;
					canPlay = true;
					mySelectButton.InteractableThreeDots(maxLvl,curntLvl);
				}
			}

			if (setupChsnLvl) { 
				ChosenLevelSetup(lvlToLoad);}
			// Turn off interaction for all three level select dots.
			if (!mySelectButton.buttonsOff) { 
				mySelectButton.buttonsOff = true; 
				mySelectButton.UninteractableThreeDots();}
		}
		//waiter method for loading i guess please ecplain X!
		if (waitMethod) {
			if (waitTimer > 0) { 
				waitTimer -= Time.deltaTime; 
			} 
			else { 
				RunAfter(voidDelegate); waitMethod = false; 
			}
		} 
	}
	#region Level Change Methods
	// Once, when the scene is openned.
	public void InitialSetup() {
		//if player already completed all the levels, start from the first one, also set up the first one if is the first time
		if (maxLvl > lvlItemHolders.Count || maxLvl < 1) {			
			curntLvl = 1; 
			//for debuging we can switch tutolrial done manually
			if(tutorialDone){
				curntLvl = 2; 
			}
		}
		else { 
			//start the middle level if player already completed some of the levels
			curntLvl = maxLvl; 
		}
		//set holder for selected level
		itemHolder = lvlItemHolders[curntLvl - 1];
		//reset selected level
		resetLevel = true;
		//disable initial setup controller
		initialSetupOn = false;
		//enable initial setup sequece
		iniSeqStart = true;
	}

	// Level complete, load silver eggs, start crate animation.
	public void SilverEggsSetup() {
		canPlay = false;
		//Set the silver egg sprites to Hollow if the egg was found already.
		for (int i = 0; i < GlobalVariables.globVarScript.puzzSilEggsCount.Count; i++) {
			int eggNumber = GlobalVariables.globVarScript.puzzSilEggsCount[i];
			mySilverEggMan.allSilEggs[eggNumber].GetComponent<SpriteRenderer>().sprite = mySilverEggMan.hollowSilEgg;
			mySilverEggMan.allSilverEggsScripts[eggNumber].hollow = true;
			Debug.Log(mySilverEggMan.allSilEggs[eggNumber].name + "has been set to hollow, ooouuuhhhh. Like a ghost. A nice ghost. Yeeah.");
		}

		mySilverEggMan.lvlSilverEggs[curntLvl - 2].SetActive(true); // CAN probably set it to true in the lvl finished seq or wtv
		if (mySilverEggMan.lvlSilverEggs[curntLvl - 2].transform.childCount > 0) {
			foreach (Transform silEgg in mySilverEggMan.lvlSilverEggs[curntLvl - 2].transform) {
				mySilverEggMan.activeSilverEggs.Add(silEgg.gameObject);
				//Debug.Log(silEgg.name + "has been added to the active Silver Egg List!");
			}
		}
		mySilverEggMan.silverEggsActive = true;
		if (!mySelectButton.noFadeDelay) { // Turn off the initial fade delay for the three dots. Should only happen once.
			mySelectButton.TurnFadeDelayOff(); 
			mySelectButton.noFadeDelay = true; 
		}
		/*LvlStuffFadeOut();
		foreach(GameObject silEgg in mySilverEggMan.activeSilverEggs) {
			silEgg.GetComponent<SilverEggSequence>().StartSequence();
		}
		scrnDarkImgScript.FadeIn();*/
	}

	// Checks if the player tapped enough silver eggs to move on, change the current level.
	public virtual void SilverEggsCheck() {
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
					maxLvl = curntLvl; 
					SaveMaxLvl(); 
					mySelectButton.EnabledThreeDots(maxLvl);
				}
				voidDelegate = NextLevelSetup;
				if (!waitMethod) { waitMethod = true; } else { Debug.LogError("waitMethod IS ALREADY IN PROGRESS, DONT DO THAT!!"); }
				waitTimer = waitTime;
			}
		}
	}
	// Lets you feed set a method as a parameter in a method.
	public void RunAfter(VoidDelegate methToRun) {
		methToRun();
	}

	// Once animations are finished, run the next level setup.
	public void NextLevelSetup() {
		foreach(SilverEggs silEggs in mySilverEggMan.lvlSilverEggs[curntLvl - 2].GetComponentsInChildren<SilverEggs>()) {
			silEggs.ResetSilEgg(); Debug.Log(silEggs.gameObject.name);
		}
		mySilverEggMan.lvlSilverEggs[curntLvl - 2].SetActive(false);
		chngLvlTimer = 0f;
		if (curntLvl >= winLvl) {
			StartCoroutine(PuzzleComplete());
			return;
		}
		resetLevel = true;
		setupLevel = true;
		itemHolder.SetActive(false);
		itemHolder = lvlItemHolders[curntLvl - 1];
		itemsWait = true;
	}

	// Prepare to change level after a level selection button has been pressed.
	public void ChangeLevelSetup() {
		// Close up current level.
		canPlay = false;
		resetLevel = true;
		mySelectButton.UninteractableThreeDots();
		LvlStuffFadeOut();
		setupChsnLvl = true;
	}

	// Setup the chosen level after waiting for setupLvlWaitTime (minimum the fade out duration of the items).
	public void ChosenLevelSetup(int lvlToLoad) {
		// Setup chosen level.
		chngLvlTimer += Time.deltaTime;
		if (chngLvlTimer > setupLvlWaitTime) {
			lvlItemHolders[curntLvl - 1].SetActive(false);
			curntLvl = lvlToLoad;
			LvlStuffFadeIn();
			resetLevel = true;
			setupLevel = true;
			itemHolder = lvlItemHolders[curntLvl - 1];
			itemsWait = true;
			setupChsnLvl = false;
			chngLvlTimer = 0;
		}
	}
	#endregion

	#region General Methods
	public void LvlStuffFadeIn() {
		levelsStuff[curntLvl -1].StartLvlFadeIn();
		Debug.Log("Should fade in stuff."); // Fade in tiles
		if (!lvlItemHolders[curntLvl -1].activeSelf) {
			lvlItemHolders[curntLvl -1].SetActive(true);
		}
	}

	public void LvlStuffFadeOut() { // Fade out tiles, tile backs, kite, backshadow. 
		levelsStuff[curntLvl -1].ExitFadeOutLvl();
	}

	public void SaveMaxLvl() {
		if (maxLvl > GlobalVariables.globVarScript.puzzMaxLvl) {
			GlobalVariables.globVarScript.puzzMaxLvl = maxLvl;
			GlobalVariables.globVarScript.SaveEggState();
		}
	}
	public void UpdateMousePos()
	{
		mousePos = Camera.main.ScreenToWorldPoint(myInput.TapPosition);
		mousePos2D = new Vector2 (mousePos.x, mousePos.y);
	}
	#endregion

	#region Coroutines
	// All silver eggs picked up, what happenes?
	public IEnumerator PuzzleComplete () {
		yield return new WaitForSeconds(0.5f);
		yield return new WaitForSeconds(0.5f);
		audioSceneParkPuzzScript.StopSceneMusic();
		audioSceneParkPuzzScript.PlayTransitionMusic();
		GlobalVariables.globVarScript.sceneFadeScript.SwitchScene(GlobalVariables.globVarScript.parkName);
	}
	#endregion
}