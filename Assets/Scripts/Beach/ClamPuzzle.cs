using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ClamPuzzle : MainPuzzleEngine {
	private Ray2D ray;
	private delegate void VoidDelegate();
	private VoidDelegate voidDelegate;
	public BeachClamLevel[] myLvls;
	public List<BeachClam> openedClams;
	public List<BeachClam> currentClams;
	public ClamLevelChangeEvent clamLevelChangeScript;
	public AudioSceneBeachPuzzle audioSceneBeachPuzzScript;
	void Start () {
		canPlay = false;
		initialSetupOn = true;
		maxLvl = GlobalVariables.globVarScript.puzzMaxLvl;

		// int tempIndex = 0;
		// foreach (GameObject button in mySelectButton.lvlSelectButtons)
		// {
		// 	tempIndex++;
		// 	button.GetComponent<Button>().onClick.AddListener(delegate{TryToChangeLevel(tempIndex);});
		// }

		//if (setupLvlWaitTime < refItemScript.fadeDuration) setupLvlWaitTime = refItemScript.fadeDuration;
		tutorialDone = GlobalVariables.globVarScript.puzzIntroDone;
		audioSceneBeachPuzzScript =  GameObject.Find ("Audio").GetComponent<AudioSceneBeachPuzzle>();
	}

	void Update () {
		if (canPlay) {
			if (mySelectButton.buttonPressed) {
				lvlToLoad = mySelectButton.lvlToLoad;
				if (chngLvlTimer >= setupLvlWaitTime && curntLvl != lvlToLoad && maxLvl >= lvlToLoad){
					chngLvlTimer = 0f;
					if(currentClams.Count > 0){
						foreach (BeachClam myClam in currentClams)
						{
							myClam.ResetClams();
						}
						currentClams.Clear();
					}
					ChangeLevelSetup();
				}
				mySelectButton.buttonPressed = false;
			}
			 
			if (chngLvlTimer < setupLvlWaitTime) { chngLvlTimer += Time.deltaTime; /* Debug.Log("do I ever run? Or am I just lazy like that?"); */ }

			if (myLvls[curntLvl-1].levelComplete) {
				/* Debug.Log("ya win m8!"); */
				SilverEggsSetup();
			}

			if (mySelectButton.buttonsOff) { mySelectButton.buttonsOff = false; mySelectButton.InteractableThreeDots(maxLvl,curntLvl); }

			#region Click
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

		}
		else
		{
			// When this Scene is loaded.
			if (initialSetupOn) { InitialSetup(); }

			// After the initial set up run the first sequence.
			if (iniSeqStart)
			{
				if (iniSeqDelay > 0) { iniSeqDelay -= Time.deltaTime; }
				else
				{
					seqTimer += Time.deltaTime;
					if (seqTimer > itemSpawnF && !itemSpawnB) { 
						itemSpawnB = true; 
						/* lvlItemHolders[curntLvl - 1].SetActive(true); */ 
						LvlStuffFadeIn(); 
						myLvls[curntLvl-1].SetUpLevel();
					}
					if (seqTimer > dotsSpawnF && !dotsSpawnB) { 
						dotsSpawnB = true; mySelectButton.EnabledThreeDots(maxLvl); 
						mySelectButton.InteractableThreeDots(maxLvl,curntLvl);
					}
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

			if (itemsWait)
			{
				itemWaitTimer += Time.deltaTime;
				if (itemWaitTimer > itemWaitAmnt)
				{
					itemHolder.SetActive(true);
					itemsWait = false;
					itemWaitTimer = 0f;
					canPlay = true;
					mySelectButton.InteractableThreeDots(maxLvl,curntLvl);
				}
			}
			
			if (setupChsnLvl) { ChosenLevelSetup(lvlToLoad); }
			// Turn off interaction for all three level select dots.
			if (!mySelectButton.buttonsOff) { mySelectButton.buttonsOff = true; mySelectButton.UninteractableThreeDots();}
		}

		if (waitMethod)
		{
			if (waitTimer > 0)
			{ waitTimer -= Time.deltaTime; }
			else { RunAfter(voidDelegate); waitMethod = false; }
		}
	}
		#region Level Change Methods
	// Once, when the scene is openned.
	public new void InitialSetup()
	{
		if(maxLvl > 3 || maxLvl < 1) { curntLvl = 1; }
		else { curntLvl = maxLvl; }
		itemHolder = lvlItemHolders[curntLvl - 1];
		myLvls[curntLvl-1].ResetLevel();
		//myLvls[curntLvl-1].SetUpLevel();
		initialSetupOn = false;
		iniSeqStart = true;
	}

	// Level complete, load silver eggs, start crate animation.
	public new void SilverEggsSetup()
	{
		canPlay = false;
		for (int i = 0; i < GlobalVariables.globVarScript.puzzSilEggsCount.Count; i++)
		{
			int eggNumber = GlobalVariables.globVarScript.puzzSilEggsCount[i];
			mySilverEggMan.allSilEggs[eggNumber].GetComponent<SpriteRenderer>().sprite = mySilverEggMan.hollowSilEgg;
			mySilverEggMan.allSilverEggsScripts[eggNumber].hollow = true;
			Debug.Log(mySilverEggMan.allSilEggs[eggNumber].name + "has been set to hollow, ooouuuhhhh. Like a ghost. A nice ghost. Yeeah.");
		}

		mySilverEggMan.lvlSilverEggs[curntLvl - 1].SetActive(true); // CAN probably set it to true in the lvl finished seq or wtv
		if (mySilverEggMan.lvlSilverEggs[curntLvl - 1].transform.childCount > 0)
		{
			foreach (Transform silEgg in mySilverEggMan.lvlSilverEggs[curntLvl - 1].transform)
			{
				mySilverEggMan.activeSilverEggs.Add(silEgg.gameObject);
				//Debug.Log(silEgg.name + "has been added to the active Silver Egg List!");
			}
		}

		mySilverEggMan.silverEggsActive = true;
		 // Turn off the initial fade delay for the three dots. Should only happen once.
		if (!mySelectButton.noFadeDelay) { mySelectButton.TurnFadeDelayOff(); mySelectButton.noFadeDelay = true; }
		EndOfLevelEvent();
	}

	// Checks if the player tapped enough silver eggs to move on, change the current level.
	public override void SilverEggsCheck()
	{
		if (mySilverEggMan.activeSilverEggs.Count > 0)
		{
			if (mySilverEggMan.amntSilEggsTapped == mySilverEggMan.activeSilverEggs.Count)
			{
				mySilverEggMan.activeSilverEggs.Clear();
				mySilverEggMan.silverEggsActive = false;
				mySilverEggMan.amntSilEggsTapped = 0;
				curntLvl++;
				if (curntLvl != winLvl) {
					scrnDarkImgScript.FadeOut();
				}
				if (curntLvl > maxLvl)
				{ maxLvl = curntLvl; SaveMaxLvl(); mySelectButton.EnabledThreeDots(maxLvl); }

				voidDelegate = NextLevelSetup;
				if (!waitMethod) { waitMethod = true; } else { Debug.LogError("waitMethod IS ALREADY IN PROGRESS, DONT DO THAT!!"); }
				waitTimer = waitTime;
			}
		}
	}

	// Lets you feed set a method as a parameter in a method.
	void RunAfter(VoidDelegate methToRun)
	{
		methToRun();
	}

	// Once animations are finished, run the next level setup.
	public new void NextLevelSetup() {
		foreach(SilverEggs silEggs in mySilverEggMan.lvlSilverEggs[curntLvl - 2].GetComponentsInChildren<SilverEggs>())
		{ silEggs.ResetSilEgg(); }
		mySilverEggMan.lvlSilverEggs[curntLvl - 2].SetActive(false);
		chngLvlTimer = 0f;
		if (curntLvl >= winLvl) {
			if (!puzzleCompScript.gameObject.activeSelf) {
				puzzleCompScript.gameObject.SetActive(true);
			}
			puzzleCompScript.endSeq = true;
			return;
		}
		myLvls[curntLvl-1].ResetLevel();
		myLvls[curntLvl-1].SetUpLevel();

		itemHolder.SetActive(false);
		itemHolder = lvlItemHolders[curntLvl - 1];
		itemsWait = true;
		clamLevelChangeScript.bootFront.sortingLayerName = "Default";
	}

	// Prepare to change level after a level selection button has been pressed.
	public new void ChangeLevelSetup()
	{
		// Close up current level.
		canPlay = false;
		myLvls[curntLvl-1].CleanClamBubbles();
		mySelectButton.UninteractableThreeDots();
		LvlStuffFadeOut();
		setupChsnLvl = true;
	}

	// Setup the chosen level after waiting for setupLvlWaitTime (minimum the fade out duration of the items).
	public new void ChosenLevelSetup(int lvlToLoad)
	{
		// Setup chosen level.
		chngLvlTimer += Time.deltaTime;
		if (chngLvlTimer > setupLvlWaitTime)
		{
			lvlItemHolders[curntLvl - 1].SetActive(false);
			curntLvl = lvlToLoad;
			LvlStuffFadeIn();
			myLvls[curntLvl-1].ResetLevel();
			myLvls[curntLvl-1].SetUpLevel();
			itemHolder = lvlItemHolders[curntLvl - 1];
			itemsWait = true;
			setupChsnLvl = false;
			chngLvlTimer = 0;
		}
	}
	#endregion

	#region General Methods

	public new void LvlStuffFadeIn()
	{
		//Debug.Log("Should fade in stuff."); // Fade in tiles
		 if (!lvlItemHolders[curntLvl -1].activeSelf) lvlItemHolders[curntLvl -1].SetActive(true);
	}

	public new void LvlStuffFadeOut() // Fade out tiles, tile backs, kite, backshadow.
	{
		levelsStuff[curntLvl -1].ExitFadeOutLvl();
	}

	public void EndOfLevelEvent() {
		clamLevelChangeScript.LevelChangeEvent();
	}

	public new void SaveMaxLvl()
	{
		if (maxLvl > GlobalVariables.globVarScript.puzzMaxLvl)
		{
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

	// #region Coroutines
	// // All silver eggs picked up, what happenes?
	// public new IEnumerator PuzzleComplete ()
	// {
	// 	yield return new WaitForSeconds(0.5f);

	// 	Debug.Log("Puzzle Completed cognraturations!!!");

	// 	yield return new WaitForSeconds(0.5f);

	// 	audioSceneBeachPuzzScript.StopSceneMusic();
	// 	audioSceneBeachPuzzScript.PlayTransitionMusic();

	// 	GlobalVariables.globVarScript.sceneFadeScript.SwitchScene(GlobalVariables.globVarScript.beachName);
	// }
	// #endregion
}
