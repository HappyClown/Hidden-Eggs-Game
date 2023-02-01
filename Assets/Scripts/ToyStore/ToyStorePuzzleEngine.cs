using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToyStorePuzzleEngine : MainPuzzleEngine {

	//private delegate void VoidDelegate();
	//private VoidDelegate voidDelegate;
	public ToyStorePuzzleLevel[] myLvls;
	public ToyStorePuzzlePiece holdedPiece;
	public float mouseRadius, liftDiff;
	private bool raycastDone, puzzleDone, holdingPiece;
	public Vector2 clickdiff, holdedPos;
	public PuzzleCell[] mainGrid;
	public Sprite emptyCell, targetCell, highlightCell;
	public Color emptyCellColor, targetCellColor, highlightCellColor;

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
		//tutorialDone = GlobalVariables.globVarScript.puzzIntroDone;
		tutorialDone = true; //change this 
		raycastDone = false;
		/* if(currentLevel >= mylevels.Length){
			currentLevel = mylevels.Length - 1;
		}
		mylevels[currentLevel].SetActive(true);
		mylvl = mylevels[currentLevel].GetComponent<CafePuzzleLevel>();
		CleanGrid();
		mylvl.SetUp();*/
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
			//SET BEHAVIOR HERE			
			if(myInput.Tapped && !holdingPiece){//Rotate free piece when tapped
				UpdateMousePos(myInput.TapPosition);
				RotatePiece(mousePos2D);							
			}else if(myInput.isDragging){//check if dragging to move pieces
				UpdateMousePos(myInput.draggingPosition);
				if(holdingPiece){
					holdedPos = mousePos2D - clickdiff;
					holdedPiece.transform.position = new Vector3(holdedPos.x, holdedPos.y, -liftDiff );
					if(CheckPlacingPos(mousePos2D)){
						Debug.Log("X I love u");
					}
					/*int matchNum = 0;
					foreach (PuzzleCell cell in holdedPiece.mycells)
					{
						foreach (PuzzleCell cell2 in myLvls[curntLvl-1].gridCells)
						{
							if(Vector2.Distance(cell.gameObject.transform.position,cell2.transform.position) < myLvls[curntLvl-1].snapRadius && !cell2.occupied){
								matchNum ++;
							}
						}						
					}
					Debug.Log(matchNum);
					if(matchNum == holdedPiece.mycells.Length){
						foreach (SpriteRenderer spRend in holdedPiece.pieceSprites)
						{
							spRend.gameObject.SetActive(true);																
						}
					}else{
						foreach (SpriteRenderer spRend in holdedPiece.pieceSprites)
						{
							spRend.gameObject.SetActive(false);																
						}
					}*/
				}else{
					holdedPiece = SelectPiece(mousePos2D);
					if(holdedPiece){
						if(holdedPiece.placed){
							myLvls[curntLvl-1].FreeCells(holdedPiece);
						}
						holdingPiece = true;
						clickdiff = mousePos2D - new Vector2(holdedPiece.gameObject.transform.position.x, holdedPiece.gameObject.transform.position.y);
					}
				}
			}else{
				if(holdingPiece){
					if(CheckPlacingPos(mousePos2D)){
						Debug.Log("X I love u");
					}
					else{
						holdedPiece.ResetPiece();
					}
					//myLvls[curntLvl-1].CheckPiece(holdedPiece);
					holdedPiece = null;
					holdingPiece = false;				
				}
			}
			
			if(Input.GetKey("r")){
				myLvls[curntLvl-1].ResetLevel();myLvls[curntLvl-1].SetUpLevel();
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
		CleanGrid();
		myLvls[curntLvl-1].ResetLevel();
		myLvls[curntLvl-1].SetUpLevel();
		SetUpGrid();
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
		CleanGrid();
		myLvls[curntLvl-1].ResetLevel();
		myLvls[curntLvl-1].SetUpLevel();
		SetUpGrid();		
		itemHolder.SetActive(false);
		itemHolder = lvlItemHolders[curntLvl - 1];
		itemsWait = true;
		//clamLevelChangeScript.bootFront.sortingLayerName = "Default";
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
			CleanGrid();
			myLvls[curntLvl-1].ResetLevel();
			myLvls[curntLvl-1].SetUpLevel();
			SetUpGrid();
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

	void CleanGrid(){
		foreach (PuzzleCell cell in mainGrid)
		{
			cell.occupied = false;
			SpriteRenderer spRend = cell.gameObject.GetComponent<SpriteRenderer>();
			spRend.sprite = emptyCell; spRend.color = emptyCellColor;
			cell.goalCell = false;
		}
	}
	void SetUpGrid(){
		foreach (PuzzleCell cell in myLvls[curntLvl-1].goalCells)
		{
			cell.goalCell = true;
			SpriteRenderer spRend = cell.gameObject.GetComponent<SpriteRenderer>();
			spRend.sprite = targetCell; spRend.color = targetCellColor;
		}
	}
	//Particular Puzzle functions
	//Rotate a piece if is available
	void RotatePiece(Vector2 pos){		
		hit = Physics2D.Raycast(pos, Vector3.forward, 50f);//Create raycast on the mouse position
		if (hit){
			if (hit.collider.CompareTag("Puzzle")) {//check if raycast hits puzzle piece
				Debug.Log(hit.collider.gameObject.name);
				ToyStorePuzzlePiece toRotPiece =  hit.collider.gameObject.GetComponent<ToyStorePuzzlePiece>();//Assign puzzle piece to variable
				toRotPiece.RotatePiece();//Call rotate function on the puzzle piece
			}
		}
	}
	//Select Piece to hold
	ToyStorePuzzlePiece SelectPiece(Vector2 pos){
		hit = Physics2D.Raycast(pos, Vector3.forward, 50f);//Create raycast on the mouse position
		if (hit){
			if (hit.collider.CompareTag("Puzzle")) {//check if raycast hits puzzle piece
				Debug.Log(hit.collider.gameObject.name);
				ToyStorePuzzlePiece toMovePiece =  hit.collider.gameObject.GetComponent<ToyStorePuzzlePiece>();//assign selected piece
				if(!toMovePiece.movingBack){//check if piece was not moving back to original position
					toMovePiece.gameObject.GetComponent<BoxCollider2D>().enabled = false;
					return toMovePiece; //returns selected puzzle piece
				}
			}
		}
		return null; //retuns empty if nothing is selected
	}
	bool CheckPlacingPos(Vector2 pos){
		hit = Physics2D.Raycast(pos, Vector3.forward, 50f);//Create raycast on the mouse position
		if (hit){
			if (hit.collider.CompareTag("InCrate")) {//check if raycast hits puzzle piece
				return true;
			}
		}
		return false;
	}
}

