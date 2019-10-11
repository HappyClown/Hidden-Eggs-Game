using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class MarketPuzzleEngine : MainPuzzleEngine {
	#region MarketPuzzleEngine Script Variables
	[Header("General")]
	public List<Items> currentLvlItems;
	public float maxZPos, ammontZ, currentZPos;
	public float itemScaleMult;
	private bool holdingItem;
	private GameObject heldItem;
	private delegate void VoidDelegate();
	private VoidDelegate voidDelegate; 
	[Header("Crate")]
	public float crateMoveSpeed;
	public Animator crateAnim;
	public Transform crateTopTransform, crateInSceneTransform;
	public float waitBeforeCrteDown;
	[Header("Tagged Colliders & Position Snaps")]
	public GameObject scaleSnapPos;
	public Transform crateSnapPos;
	[Header("Item Parents")]
	public GameObject crateParent;
	[Header("Scripts")]
	public Scale scaleScript;
	public Crate crateScript;
	public ResetItemsButton resetItemsButtonScript;
	public Items refItemScript;
	public ReqParchmentMove reqParchMoveScript;
	[Header("Hide In Inspector ^_^")]
	public float curntPounds;
	public float curntAmnt;
	#endregion

	public AudioSceneMarketPuzzle audioSceneMarketPuz;

	void Start () {
		canPlay = false;
		initialSetupOn = true;
		heldItem = null;
		maxLvl = GlobalVariables.globVarScript.puzzMaxLvl;
		//mySilverEggMan.silverEggsPickedUp = GlobalVariables.globVarScript.silverEggsCount;
		if (setupLvlWaitTime < refItemScript.fadeDuration) setupLvlWaitTime = refItemScript.fadeDuration;
		tutorialDone = GlobalVariables.globVarScript.puzzIntroDone;
		audioSceneMarketPuz =  GameObject.Find ("Audio").GetComponent<AudioSceneMarketPuzzle>();
	}

	void Update () {
		if (canPlay) {
			if (mySelectButton.buttonPressed) {
				lvlToLoad = mySelectButton.lvlToLoad;
				if (chngLvlTimer >= setupLvlWaitTime && curntLvl != lvlToLoad && maxLvl >= lvlToLoad) {
					chngLvlTimer = 0f;
					ChangeLevelSetup();
				}
				mySelectButton.buttonPressed = false;
			}

			if (chngLvlTimer < setupLvlWaitTime) { chngLvlTimer += Time.deltaTime; }

			// Current level complete.
			if (curntPounds == crateScript.reqPounds && curntAmnt == crateScript.reqItems && !holdingItem) { SilverEggsSetup(); }

			if (mySelectButton.buttonsOff) { mySelectButton.buttonsOff = false; mySelectButton.InteractableThreeDots(maxLvl, curntLvl); }

			#region Click
			// Click //
			if (Input.GetMouseButtonDown(0) && !holdingItem) {
				UpdateMousePos();
				hit = Physics2D.Raycast(mousePos2D, Vector3.forward, 50f);

				if (hit) {
					if (hit.collider.CompareTag("Item")) {
						holdingItem = true;
						heldItem = hit.collider.gameObject;
						heldItem.transform.parent = itemHolder.transform;
						heldItem.transform.localScale = heldItem.transform.localScale * itemScaleMult;
						heldItem.GetComponent<SpriteRenderer>().sprite = heldItem.GetComponent<Items>().item;
						currentZPos =  heldItem.GetComponent<Items>().zPos;	
						foreach (Items theItems in currentLvlItems)
						{
							if(theItems.zPos < currentZPos)
							{
								theItems.zPos += ammontZ;
								theItems.gameObject.transform.position = new Vector3(theItems.gameObject.transform.position.x,theItems.gameObject.transform.position.y, theItems.zPos);
							}
						}					

						if (heldItem == scaleScript.itemOnScale) {
							scaleScript.isAnItemOnScale = false;
							scaleScript.itemOnScale = null;
						}

						if (heldItem.GetComponent<Items>().inCrate == true) {
							curntPounds -= heldItem.GetComponent<Items>().weight;
							curntAmnt -= 1;
							heldItem.GetComponent<Items>().inCrate = false;
						}
						audioSceneMarketPuz.pickupFruit();
					}
				}
			}
			#endregion

			#region Drag
			// Drag //
			else if (Input.GetMouseButton(0) && holdingItem == true) {
				UpdateMousePos();
				float heldItemObjX = heldItem.transform.position.x;
				float heldItemObjY = heldItem.transform.position.y;

				heldItemObjX = mousePos.x;
				heldItemObjY = mousePos.y;

				heldItem.transform.position = new Vector3(heldItemObjX, heldItemObjY, maxZPos);
			}
			#endregion

			#region Drop
			// Let go and decide where to put the item //
			if (Input.GetMouseButtonUp(0) && holdingItem == true) {
				UpdateMousePos();
				holdingItem = false;
				heldItem.transform.localScale = heldItem.transform.localScale / itemScaleMult;
				heldItem.GetComponent<SpriteRenderer>().sprite = heldItem.GetComponent<Items>().itemWithShadow;

				RaycastHit2D[] hits;
				hits = Physics2D.RaycastAll(mousePos2D, Vector3.forward, 50f);
				for (int i =0; i < hits.Length; i++) {
					// ON THE SCALE AREA//
					if (hits[i].collider.gameObject.CompareTag("Scale")) {
						if (scaleScript.itemOnScale != null) {
							scaleScript.itemOnScale.transform.position = scaleScript.itemOnScale.GetComponent<Items>().initialPos;
							scaleScript.itemOnScale.transform.parent = itemHolder.transform;
						}
						heldItem.transform.position = new Vector3(scaleSnapPos.transform.position.x, scaleSnapPos.transform.position.y, -5f);
						heldItem.transform.parent = scaleSnapPos.transform;
						
						scaleScript.itemOnScale = heldItem;
						scaleScript.isAnItemOnScale = true;

						audioSceneMarketPuz.dropFruitScale();
						break;
					}

					// ON THE TABLE AREA//
					else if (hits[i].collider.gameObject.CompareTag("Table")) {
						heldItem.transform.position = new Vector3(mousePos.x, mousePos.y, maxZPos);
						heldItem.GetComponent<Items>().zPos = maxZPos;
						//SFX DROP ON WOOD
						audioSceneMarketPuz.dropFruitCrate();
						break;
					}

					// IN THE CRATE DIRECTLY//
					else if (hits[i].collider.gameObject.CompareTag("InCrate")) {
						heldItem.GetComponent<Items>().inCrate = true;
						heldItem.transform.position = new Vector3(mousePos.x, mousePos.y, maxZPos);
						heldItem.GetComponent<Items>().zPos = maxZPos;
						heldItem.transform.parent = crateParent.transform;
						curntPounds += heldItem.GetComponent<Items>().weight;
						curntAmnt += 1;
						//SFX DROP ON WOOD
						audioSceneMarketPuz.dropFruitCrate();
						break;
					}

					// IN THE CRATE AREA//
					else if (hits[i].collider.gameObject.CompareTag("Crate")) {
						heldItem.GetComponent<Items>().inCrate = true;
						heldItem.transform.position = new Vector3(crateSnapPos.transform.position.x, crateSnapPos.transform.position.y, maxZPos);
						heldItem.GetComponent<Items>().zPos = maxZPos;
						heldItem.transform.parent = crateParent.transform;
						curntPounds += heldItem.GetComponent<Items>().weight;
						curntAmnt += 1;
						//SFX DROP ON WOOD
						audioSceneMarketPuz.dropFruitCrate();
						break;
					}

					// Cannot drop items outside of the screen. If item held does Not hit any of the areas send it back to its initial position on the table.
					else {
						heldItem.transform.position = heldItem.GetComponent<Items>().initialPos;
					}
				}
				heldItem = null;
			}
		#endregion
		}
		else {
			// When this Scene is loaded.
			if (initialSetupOn) { InitialSetup(); }

			// After the initial set up run the first sequence.
			if (iniSeqStart) {
				if (iniSeqDelay > 0) { iniSeqDelay -= Time.deltaTime; }
				else {
					seqTimer += Time.deltaTime;
					if (seqTimer > crateDownF && !crateDownB) { crateDownB = true; crateAnim.SetTrigger("MoveDown"); audioSceneMarketPuz.crateSlideDown(); StartCoroutine(MoveCrateDown()); }
					if (seqTimer > reqDownF && !reqDownB) { reqDownB = true; reqParchMoveScript.moveToShown = true; audioSceneMarketPuz.openPanel(); } 
					if (seqTimer > itemSpawnF && !itemSpawnB) { itemSpawnB = true; lvlItemHolders[curntLvl - 1].SetActive(true); for (int i = 0; i < resetItemsButtonScript.items.Count; i++) // CONSIDER SAVING THE ITEM SCRIPTS TO ANOTHER LIST TO AVOID LOOPING 7 to 12 GETCOMPONENTS AT A TIME
					{ resetItemsButtonScript.items[i].GetComponent<Items>().FadeIn(); } }
					if (seqTimer > dotsSpawnF && !dotsSpawnB) { dotsSpawnB = true; mySelectButton.EnabledThreeDots(maxLvl); mySelectButton.UninteractableThreeDots(); }
					if (seqTimer > iniCanPlayF) {
						if (tutorialDone) {
							canPlay = true;
							mySelectButton.InteractableThreeDots(maxLvl, curntLvl);
							//sceneTapScript.canTapPauseBtn = true;
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
					resetItemsButtonScript.FillItemResetArray();

					for (int i = 0; i < resetItemsButtonScript.items.Count; i++) { // CONSIDER SAVING THE ITEM SCRIPTS TO ANOTHER LIST TO AVOID LOOPING 7 to 12 GETCOMPONENTS AT A TIME
						resetItemsButtonScript.items[i].GetComponent<Items>().FadeIn(); 
					}

					itemsWait = false;
					itemWaitTimer = 0f;
					canPlay = true;
					mySelectButton.InteractableThreeDots(maxLvl, curntLvl);
				}
			}

			if (setupChsnLvl) { ChosenLevelSetup(lvlToLoad); }
			// Turn off interaction for all three level select dots.
			if (!mySelectButton.buttonsOff) { mySelectButton.buttonsOff = true; mySelectButton.UninteractableThreeDots(); }
		}

		if (waitMethod) 
		{
			if (waitTime > 0) 
			{ waitTime -= Time.deltaTime; } 
			else { RunAfter(voidDelegate); waitMethod = false; }
		} 
	}

	#region Level Change Methods
	// Once, when the scene is openned.
	public new void InitialSetup() {
		//Debug.Log("Initial Setup");
		if(maxLvl > 3 || maxLvl < 1) { curntLvl = 1; }
		else { curntLvl = maxLvl; }
		curntPounds = 0;
		curntAmnt = 0;
		itemHolder = lvlItemHolders[curntLvl - 1];
		resetItemsButtonScript.FillItemResetArray();
		initialSetupOn = false;
		crateScript.UpdateRequirements();
		iniSeqStart = true;		
		currentLvlItems.Clear();
		itemHolder.gameObject.GetComponentsInChildren<Items>(currentLvlItems); 
	}

	// Level complete, load silver eggs, start crate animation.
	public new void SilverEggsSetup() {
		//Debug.Log("New Level Setup");
		canPlay = false;
		//sceneTapScript.canTapPauseBtn = false;
		//Set the silver egg sprites to Hollow if the egg was found already.
		for (int i = 0; i < GlobalVariables.globVarScript.puzzSilEggsCount.Count; i++)
		{
			int eggNumber = GlobalVariables.globVarScript.puzzSilEggsCount[i];
			mySilverEggMan.allSilEggs[eggNumber].GetComponent<SpriteRenderer>().sprite = mySilverEggMan.hollowSilEgg;
			mySilverEggMan.allSilverEggsScripts[eggNumber].hollow = true;
			//Debug.Log(mySilverEggMan.allSilEggs[eggNumber].name + "has been set to hollow, ooouuuhhhh. Like a ghost. A nice ghost. Yeeah.");
		}
		if (mySilverEggMan.lvlSilverEggs[curntLvl - 1].transform.childCount > 0) {
			foreach (Transform silEgg in mySilverEggMan.lvlSilverEggs[curntLvl - 1].transform)
			{
				mySilverEggMan.activeSilverEggs.Add(silEgg.gameObject);
				//Debug.Log(silEgg.name + "has been added to the active Silver Egg List!");
			}
		}
		mySilverEggMan.silverEggsActive = true;
		if (!mySelectButton.noFadeDelay) { mySelectButton.TurnFadeDelayOff(); mySelectButton.noFadeDelay = true; } // Turn off the initial fade delay for the three dots. Should only happen once.
		// Fade out the finished level's items. (Except the ones in the crate.)
		if (scaleScript.itemOnScale != null) { scaleScript.itemOnScale.transform.parent = itemHolder.transform; }
		Items[] childrenItemScripts; // CONSIDER SAVING THE ITEM SCRIPTS TO ANOTHER LIST TO AVOID LOOPING 7 to 12 GETCOMPONENTS AT A TIME
		childrenItemScripts = lvlItemHolders[curntLvl - 1].transform.GetComponentsInChildren<Items>(); 
		for (int i = 0; i < childrenItemScripts.Length; i++)
		{ childrenItemScripts[i].FadeOut(); }
		StartCoroutine(MoveCrateRight());
		reqParchMoveScript.moveToHidden = true;
		audioSceneMarketPuz.closePanel();
		curntPounds = 0;
		curntAmnt = 0;
	}

	// Checks if the player tapped enough silver eggs to move on, change the current level.
	public override void SilverEggsCheck() {
		//int amntSilEggsTapped = 0;
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
					maxLvl = curntLvl; SaveMaxLvl(); 
					mySelectButton.EnabledThreeDots(maxLvl); 
				}
				voidDelegate = NextLevelSetup;
				if (!waitMethod) { waitMethod = true; } else { Debug.LogError("waitMethod IS ALREADY IN PROGRESS, DONT DO THAT!!"); }
				waitTime = waitBeforeCrteDown;
				//sceneTapScript.canTapPauseBtn = true;
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
		{ silEggs.ResetSilEgg(); }
		mySilverEggMan.lvlSilverEggs[curntLvl - 2].SetActive(false);
		chngLvlTimer = 0f;
		if (curntLvl >= winLvl) {
			puzzleCompScript.endSeq = true;
			return;
		}
		itemHolder = lvlItemHolders[curntLvl - 1];
		itemsWait = true;
		crateScript.UpdateRequirements();
		reqParchMoveScript.moveToShown = true;
		audioSceneMarketPuz.openPanel();
		currentLvlItems.Clear();
		itemHolder.gameObject.GetComponentsInChildren<Items>(currentLvlItems); 
	}

	// Prepare to change level after a level selection button has been pressed.
	public new void ChangeLevelSetup() {
		// Close up current level.
		canPlay = false;
		mySelectButton.UninteractableThreeDots();
		for (int i = 0; i < resetItemsButtonScript.items.Count; i++) // CONSIDER SAVING THE ITEM SCRIPTS TO ANOTHER LIST TO AVOID LOOPING 7 to 12 GETCOMPONENTS AT A TIME
		{ resetItemsButtonScript.items[i].GetComponent<Items>().FadeOut(); }
		reqParchMoveScript.moveToHidden = true;
		audioSceneMarketPuz.closePanel();
		setupChsnLvl = true;
		// RESET SILVER EGGs ///////////////////////
	}

	// Setup the chosen level after waiting for setupLvlWaitTime (minimum the fade out duration of the items).
	public new void ChosenLevelSetup(int lvlToLoad) {
		// Setup chosen level.
		chngLvlTimer += Time.deltaTime;
		if (chngLvlTimer > setupLvlWaitTime) {
			lvlItemHolders[curntLvl - 1].SetActive(false);
			resetItemsButtonScript.EndOfLevelReset();
			curntLvl = lvlToLoad;
			curntPounds = 0;
			curntAmnt = 0;
			itemHolder = lvlItemHolders[curntLvl - 1];
			itemsWait = true;
			crateScript.UpdateRequirements();
			reqParchMoveScript.moveToShown = true;
			audioSceneMarketPuz.openPanel();
			setupChsnLvl = false;
			chngLvlTimer = 0;
			currentLvlItems.Clear();
			itemHolder.gameObject.GetComponentsInChildren<Items>(currentLvlItems);
		}
	}
	#endregion

	#region General Methods

	public void SaveSilverEggsToCorrectFile() {
		if (mySilverEggMan.silverEggsPickedUp > GlobalVariables.globVarScript.silverEggsCount) {
			GlobalVariables.globVarScript.totalEggsFound += 1;
			GlobalVariables.globVarScript.silverEggsCount = mySilverEggMan.silverEggsPickedUp; 
			GlobalVariables.globVarScript.SaveEggState();
		}
	}

	public void SaveNewSilEggsFound(int newSilEggFound) {
		foreach (int silEggNumber in GlobalVariables.globVarScript.puzzSilEggsCount)
		{
			if (silEggNumber == newSilEggFound){
				return;
			}
		}
		GlobalVariables.globVarScript.puzzSilEggsCount.Add(newSilEggFound);
		GlobalVariables.globVarScript.SaveEggState();
	}

	public new void SaveMaxLvl() {
		if (maxLvl > GlobalVariables.globVarScript.puzzMaxLvl) {
			GlobalVariables.globVarScript.puzzMaxLvl = maxLvl;
			GlobalVariables.globVarScript.SaveEggState();
		}
	}

	public void UpdateMousePos() {
		mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		mousePos2D = new Vector2 (mousePos.x, mousePos.y);
	}
	#endregion

	#region Coroutines
	// Move crate to the right.
	public IEnumerator MoveCrateRight () {
		//Make it skip a frame to make sure that the animation has time to start.
		yield return new WaitForSeconds(0.0001f);

		while (crateAnim.transform.parent.rotation != crateInSceneTransform.rotation)
		{
			float Zangle = crateAnim.transform.parent.eulerAngles.z;
			Zangle = Mathf.LerpAngle(crateAnim.transform.parent.eulerAngles.z, 0f, Time.deltaTime * crateMoveSpeed);
			crateAnim.transform.parent.eulerAngles = new Vector3(0, 0, Zangle);

			if (Vector3.Distance(crateAnim.transform.parent.eulerAngles, crateInSceneTransform.eulerAngles) <= 0.1f)
			{
				crateAnim.transform.parent.rotation = crateInSceneTransform.rotation;
			}

			yield return null;
		}
		crateAnim.SetTrigger("MoveRight");

		//SFX MOVE CRATE
		audioSceneMarketPuz.crateSlideRight();

		yield return new WaitForSeconds(0.0001f);

		while (crateAnim.GetCurrentAnimatorStateInfo(0).IsName("CrateMoveRight"))
		{
			//Debug.Log("Playing anim move right.");
			yield return null;
		}
		
		scaleScript.itemOnScale = null; // Deleted both scale lines if we want scale arrow to reset after silver eggs have been clicked. 
		scaleScript.isAnItemOnScale = false; //

		crateParent.transform.parent.position = crateTopTransform.position;
		crateParent.transform.parent.rotation = crateTopTransform.rotation;

		mySilverEggMan.lvlSilverEggs[curntLvl - 1].SetActive(true);
		resetItemsButtonScript.EndOfLevelReset();
		itemHolder.SetActive(false);
		scrnDarkImgScript.FadeIn();
		crateAnim.SetTrigger("MoveDown");
		//SFX MOVE CRATE
		audioSceneMarketPuz.crateSlideDown();
		StartCoroutine(MoveCrateDown());
	}

	// Move crate down.
	public IEnumerator MoveCrateDown () {
		//Debug.Log("Entered Coroutine MoveCrateDown. Yo.");
		yield return new WaitUntil(() => crateParent.transform.parent.position == crateTopTransform.position);
		//Debug.Log("CrateParent pos = crateTop pos.");
		while (crateAnim.GetCurrentAnimatorStateInfo(0).IsName("CrateMoveDown"))
		{
			//Debug.Log("MoveCrateDown Animating.");
			yield return null;
		}
		//Debug.Log("should take anim pos");
		crateParent.transform.parent.position = crateAnim.transform.position;
		crateParent.transform.parent.rotation = crateAnim.transform.rotation;

		// foreach activesilvereggs getcomponenet silvereggsequence startsequence = true
		foreach(GameObject silEgg in mySilverEggMan.activeSilverEggs)
		{
			silEgg.GetComponent<SilverEggSequence>().StartSequence();
		}
	}

	// All silver eggs picked up, what happenes?
	// public new IEnumerator PuzzleComplete () {
	// 	yield return new WaitForSeconds(0.5f);

	// 	Debug.Log("Puzzle Completed cognraturations!!!");
	// 	puzzleCompScript.reveal = true;

	// 	yield return new WaitForSeconds(0.5f);

	// 	audioSceneMarketPuz.StopSceneMusic();
	// 	audioSceneMarketPuz.PlayTransitionMusic();
		
	// 	GlobalVariables.globVarScript.sceneFadeScript.SwitchScene(GlobalVariables.globVarScript.marketName);
	// }
	#endregion
}