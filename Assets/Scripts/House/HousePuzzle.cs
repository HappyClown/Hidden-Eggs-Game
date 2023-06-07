﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HousePuzzle : MainPuzzleEngine {
	//private delegate void VoidDelegate();
	//private VoidDelegate voidDelegate;	
	public List<HousePuzzleMainLevel> myLvls;
	public List<SinkPiece> myPieces;
	public SinkPiece currentPiece;
	public float cellRadius;
	private int currentHashCode;
	public bool soapSelected, canSelect, firstTouch;
	public Vector3 currentCellPos;
	// Use this for initialization
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
		//audioSceneBeachPuzzScript =  GameObject.Find ("Audio").GetComponent<AudioSceneBeachPuzzle>();


		currentHashCode = 0;
		soapSelected= false;
		canSelect = true;
		firstTouch = true;
		//curntLvl = 1;
	}
	
	// Update is called once per frame
	void Update () {

		if (canPlay) {
			if (mySelectButton.buttonPressed) {
				lvlToLoad = mySelectButton.lvlToLoad;
				if (chngLvlTimer >= setupLvlWaitTime && curntLvl != lvlToLoad && maxLvl >= lvlToLoad){
					chngLvlTimer = 0f;
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
			if(myInput.Tapped){
				UpdateMousePos(myInput.TapPosition);
				hit = Physics2D.Raycast(mousePos2D, Vector3.forward, 50f);
				if (hit) {
						Debug.Log(hit.collider.gameObject.name);
						if (hit.collider.CompareTag("Puzzle")) {
							currentPiece = hit.collider.gameObject.GetComponent<SinkPiece>();
							if(currentPiece.pieceType == SinkPiece.pieceTypes.bubble){
								currentPiece.matched = true;
								ResetPieces();
							}
							else{
								ResetPieces();
							}
						}
					}
			}
			else if(myInput.isDragging){
				if(firstTouch){
					UpdateMousePos(myInput.startDragTouch);
					firstTouch = false;
				}else{
					UpdateMousePos(myInput.draggingPosition);
				}
				hit = Physics2D.Raycast(mousePos2D, Vector3.forward, 50f);
				if (hit) {
						Debug.Log(hit.collider.gameObject.name);
						if (hit.collider.CompareTag("Puzzle") && canSelect) {
							if(currentHashCode != hit.collider.gameObject.GetHashCode()){
								currentHashCode = hit.collider.gameObject.GetHashCode();
								if(myPieces.Count > 0){
									CheckConnection(hit.collider.gameObject.GetComponent<SinkPiece>());
								}							
								currentPiece = hit.collider.gameObject.GetComponent<SinkPiece>();
								if(currentPiece.pieceType == SinkPiece.pieceTypes.bubble){
									currentPiece.matched = true;
									ResetPieces();
								}
								currentCellPos =  hit.collider.gameObject.transform.position;
								myPieces.Add(hit.collider.gameObject.GetComponent<SinkPiece>());
								if(myPieces.Count == 1){
									currentPiece.selected = true;
									if(currentPiece.pieceType == SinkPiece.pieceTypes.sponge || currentPiece.pieceType == SinkPiece.pieceTypes.soap){
										if(currentPiece.pieceType == SinkPiece.pieceTypes.soap){
											soapSelected = true;
										}								
									}
									else{
										ResetPieces();
									}
								}else if(myPieces.Count < 5){								
									if(currentPiece.pieceType == SinkPiece.pieceTypes.dish){
										currentPiece.selected = true;
									}
									else{
										ResetPieces();
									}
								}else if(myPieces.Count == 5){
									if(currentPiece.pieceType == SinkPiece.pieceTypes.soap){
										if(soapSelected){
											ResetPieces();
										}
										else{
											MatchPieces();
										}
									}else if(currentPiece.pieceType == SinkPiece.pieceTypes.sponge){
										if(soapSelected){
											MatchPieces();
										}
										else{
											ResetPieces();
										}
									}else{
										ResetPieces();
									}
								}
							}
						}
						
					}else{
						//Debug.Log(Vector2.Distance(currentCellPos,Camera.main.ScreenToWorldPoint(myInput.draggingPosition)));
						if(Vector2.Distance(currentCellPos,Camera.main.ScreenToWorldPoint(myInput.draggingPosition)) > cellRadius && myPieces.Count > 0){
							ResetPieces();
						}
					}
			}
			else{			
				ResetPieces();
				canSelect = true;
				firstTouch = true;
			}
			if(Input.GetKey("r")){
				myLvls[curntLvl-1].ResetLevel();
				myLvls[curntLvl-1].SetUpLevel();
			}
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
					if (seqTimer > itemSpawnF && !itemSpawnB) { itemSpawnB = true; /* lvlItemHolders[curntLvl - 1].SetActive(true); */ LvlStuffFadeIn(); }
					if (seqTimer > dotsSpawnF && !dotsSpawnB) { dotsSpawnB = true; mySelectButton.EnabledThreeDots(maxLvl); mySelectButton.InteractableThreeDots(maxLvl,curntLvl);}
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

					//for (int i = 0; i < resetTilesScript.tiles.Count; i++) // CONSIDER SAVING THE ITEM SCRIPTS TO ANOTHER LIST TO AVOID LOOPING 7 to 12 GETCOMPONENTS AT A TIME
					//{ resetTilesScript.tiles[i].GetComponent<FadeInOutSprite>().FadeIn(); }

					LvlStuffFadeIn();

					itemsWait = false;
					itemWaitTimer = 0f;
					canPlay = true;
					mySelectButton.InteractableThreeDots(maxLvl,curntLvl);
				}
			}
			

			if (setupChsnLvl) { ChosenLevelSetup(lvlToLoad); }
			// Turn off interaction for all three level select dots.
			if (!mySelectButton.buttonsOff) { mySelectButton.buttonsOff = true; mySelectButton.UninteractableThreeDots();}

			// #region Click On SilverEggs
			// // Clicking on a silver egg.
			// if (myInput.Tapped) {
			// 	UpdateMousePos();
			// 	hit = Physics2D.Raycast(mousePos2D, Vector3.forward, 50f);
			// 	if (hit) {
			// 		if (hit.collider.CompareTag("Egg")) {
			// 			SilverEggs silEggTappedScript = hit.collider.gameObject.GetComponent<SilverEggs>();
			// 			silEggTappedScript.StartSilverEggAnim();
			// 			hit.collider.enabled = false;
		
			// 			if (!silEggTappedScript.hollow) { mySilverEggMan.silverEggsPickedUp++; }
			// 			mySilverEggMan.SaveSilverEggsToCorrectFile();
			// 			mySilverEggMan.SaveNewSilEggsFound(mySilverEggMan.allSilEggs.IndexOf(hit.collider.gameObject));
			// 			mySilverEggMan.amntSilEggsTapped++;
			// 			SilverEggsCheck(); // Check if the Silver Eggs have all been collected.
			// 		}
			// 	}
			// }
			// #endregion
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
		myLvls[curntLvl-1].SetUpLevel();
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

		if (!mySelectButton.noFadeDelay) { mySelectButton.TurnFadeDelayOff(); mySelectButton.noFadeDelay = true; } // Turn off the initial fade delay for the three dots. Should only happen once.

		//EndOfLevelEvent();
		//LvlStuffFadeOut();
		
		foreach(GameObject silEgg in mySilverEggMan.activeSilverEggs) // TO BE PUT IN THE ANIM SEQ -------------------------------------------------------------------------------------------
		{
			silEgg.GetComponent<SilverEggSequence>().StartSequence();
		}
		scrnDarkImgScript.FadeIn();

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
				scrnDarkImgScript.FadeOut();
				curntLvl++;
				if (curntLvl > maxLvl)
				{ maxLvl = curntLvl; SaveMaxLvl(); mySelectButton.EnabledThreeDots(maxLvl); }

				voidDelegate = NextLevelSetup;
				if (!waitMethod) { waitMethod = true; } else { Debug.LogError("waitMethod IS ALREADY IN PROGRESS, DONT DO THAT!!"); }
				waitTimer = waitTime;
			}
		}
	}
	// Once animations are finished, run the next level setup.
	public new void NextLevelSetup() {
		foreach(SilverEggs silEggs in mySilverEggMan.lvlSilverEggs[curntLvl - 2].GetComponentsInChildren<SilverEggs>())
		{ silEggs.ResetSilEgg(); }
		mySilverEggMan.lvlSilverEggs[curntLvl - 2].SetActive(false);
		chngLvlTimer = 0f;
		if (curntLvl >= winLvl) {
			StartCoroutine(PuzzleComplete());
			return;
		}
		myLvls[curntLvl-1].ResetLevel();
		myLvls[curntLvl-1].SetUpLevel();

		itemHolder.SetActive(false);
		itemHolder = lvlItemHolders[curntLvl - 1];
		itemsWait = true;
		//clamLevelChangeScript.bootFront.sortingLayerName = "Default";
	}

	// Prepare to change level after a level selection button has been pressed.
	public new void ChangeLevelSetup()
	{
		// Close up current level.
		canPlay = false;
			//myLvls[curntLvl-1].CleanClamBubbles();

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
			myLvls[curntLvl-1].ResetLevel();
			curntLvl = lvlToLoad;
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
		Debug.Log("Should fade in stuff."); // Fade in tiles
		 if (!lvlItemHolders[curntLvl -1].activeSelf) lvlItemHolders[curntLvl -1].SetActive(true);
	}

	public new void LvlStuffFadeOut() // Fade out tiles, tile backs, kite, backshadow.
	{
		//levelsStuff[curntLvl -1].ExitFadeOutLvl();
	}

	public void EndOfLevelEvent() {
		//clamLevelChangeScript.LevelChangeEvent();
	}

	public new void SaveMaxLvl()
	{
		if (maxLvl > GlobalVariables.globVarScript.puzzMaxLvl)
		{
			GlobalVariables.globVarScript.puzzMaxLvl = maxLvl;
			GlobalVariables.globVarScript.SaveEggState();
		}
	}

	#endregion

	#region Coroutines
	// All silver eggs picked up, what happenes?
	public new IEnumerator PuzzleComplete ()
	{
		yield return new WaitForSeconds(0.5f);

		Debug.Log("Puzzle Completed cognraturations!!!");

		yield return new WaitForSeconds(0.5f);

		//audioSceneBeachPuzzScript.StopSceneMusic();
		//audioSceneBeachPuzzScript.PlayTransitionMusic();

		//GlobalVariables.globVarScript.sceneFadeScript.SwitchScene(GlobalVariables.globVarScript.beachName);
	}
	#endregion
	public void UpdateMousePos(Vector3 Pos)
	{
		mousePos = Camera.main.ScreenToWorldPoint(Pos);
		mousePos2D = new Vector2 (mousePos.x, mousePos.y);
	}
	public void ResetPieces(){
		foreach (SinkPiece item in myPieces)
		{
			item.selected = false;
		}
		myPieces.Clear();
		canSelect = false;
		currentCellPos = Vector3.zero;
		currentHashCode = 0;
		soapSelected = false;
	}
	public void MatchPieces(){
		foreach (SinkPiece item in myPieces)
		{
			item.selected = false;
			item.matched = true;
		}
		myPieces.Clear();
		canSelect = false;
		currentCellPos = Vector3.zero;
		currentHashCode = 0;
		soapSelected = false;
	}
	public void CheckConnection(SinkPiece newPiece){
		bool badConnection = true;
		if(newPiece.currentCell.cellDown == currentPiece.currentCell){
			badConnection = false;
		}else if(newPiece.currentCell.cellUp == currentPiece.currentCell){
			badConnection = false;
		}
		else if(newPiece.currentCell.cellLeft == currentPiece.currentCell){
			badConnection = false;
		}
		else if(newPiece.currentCell.cellRight == currentPiece.currentCell){
			badConnection = false;
		}
		if(badConnection){
			ResetPieces();
		}

	}
}
