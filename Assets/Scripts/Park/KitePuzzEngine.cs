using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KitePuzzEngine : MonoBehaviour 
{
	#region Old vars
	Ray2D ray;
	RaycastHit2D hit;
	Vector2 mousePos2D;
	Vector3 mousePos;
	public Vector3 updateMousePos;
	public int connections;

	public List<GameObject> lvlTilesFadeScripts;
	public List<GameObject> lvlTileBacksFadeScripts;
	public List<FadeInOutSprite> lvlBackShadowsFadeScripts;
	public List<GameObject> lvlKites;
	public List<FadeInOutSprite> lvlKitesFadeScripts;
	public List<float> bGSizes;
	public BackgroundScale bgScleScript;

	public int curntLvl;

	public ClickToRotateTile clickToRotTileScript;
	#endregion
	#region Basic Scripts Sources
	[Header("Input Detector")]
	public inputDetector myInput;
	[Header("Silver Eggs")]
	public SilverEggsManager mySilverEggMan;
	[Header("Selection Buttons")]
	public LevelSelectionButtons mySelectButton;
	#endregion
	#region GrabItem Script Variables
	[Header("General")]

	public int winLvl;
	public bool itemsWait;
	public float itemWaitAmnt;
	private float itemWaitTimer;

	private bool initialSetupOn;
	private bool setupChsnLvl;
	// For Delegate Method
	private delegate void VoidDelegate();
	private VoidDelegate voidDelegate; 
	private bool waitMethod;
	[Tooltip("For now, minimum the lenght of the silver egg tap anim.")] 
	public float waitTime;
	private float waitTimer;

	[Header("Item Parents")]
	[Tooltip("GameObjects - Game objects that hold the level items, in ascending order.")] 
	public List<GameObject> lvlItemHolders;
	public GameObject itemHolder;

	// [Header("Level Selection Buttons")]
	// public LevelSelectionButtons testButtons;
	// [Tooltip("GameObjects - The level selection buttons, in ascending order. ( 1 - 0, 2 - 1, etc.")] 
	// public List<GameObject> lvlSelectButtons;
	// [Tooltip("Scripts - The FadeInOutImage scripts, in ascending order. ( 1 - 0, 2 - 1, etc.")] 
	// public List<FadeInOutImage> lvlSelectFades;
	// [Tooltip("Scripts - The Scaler scripts, in ascending order. ( 1 - 0, 2 - 1, etc.")] 
	// public List<Scaler> lvlSelectScalers;
	// private bool noFadeDelay;
	// private bool buttonsOff;

	[Header("Initial Sequence")]
	public float seqTimer;
	public float iniSeqDelay, crateDownF, reqDownF, itemSpawnF, dotsSpawnF, iniCanPlayF;
	private bool iniSeqStart, crateDownB, reqDownB, itemSpawnB, dotsSpawnB;

	[Header("Scripts")]
	public ResetTiles resetTilesScript;
	//public Items refItemScript;
	public FadeInOutImage scrnDarkImgScript;

	[Header("Hide In Inspector ^_^")]
	public bool canPlay;
	public int lvlToLoad;
	public float setupLvlWaitTime;
	public float chngLvlTimer;
	public int maxLvl;
	#endregion

	public AudioSceneParkPuzzle audioSceneParkPuzzScript;



	void Start () 
	{
		canPlay = false;
		initialSetupOn = true;
		maxLvl = GlobalVariables.globVarScript.parkPuzzMaxLvl;
		//if (setupLvlWaitTime < refItemScript.fadeDuration) setupLvlWaitTime = refItemScript.fadeDuration;
	}
	

	void Update () 
	{
		if (canPlay)
		{
			if (chngLvlTimer < setupLvlWaitTime) { chngLvlTimer += Time.deltaTime; /* Debug.Log("do I ever run? Or am I just lazy like that?"); */ }

			if (connections == clickToRotTileScript.connectionsNeeded)
			{
				/* Debug.Log("ya win m8!"); */
				SilverEggsSetup();
			}

			if (mySelectButton.buttonsOff) { mySelectButton.buttonsOff = false; mySelectButton.InteractableThreeDots(maxLvl,curntLvl); }

			#region Click
			// Click //
			if (myInput.Tapped)
			{
				UpdateMousePos();
				hit = Physics2D.Raycast(mousePos2D, Vector3.forward, 50f);
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
					 if (seqTimer > itemSpawnF && !itemSpawnB) { itemSpawnB = true; /* lvlItemHolders[curntLvl - 1].SetActive(true); */ LvlStuffFadeIn(); bgScleScript.ScaleBG();}
					 if (seqTimer > dotsSpawnF && !dotsSpawnB) { dotsSpawnB = true; mySelectButton.EnabledThreeDots(maxLvl); mySelectButton.InteractableThreeDots(maxLvl,curntLvl); clickToRotTileScript.CalculateConnectionsNeeded();}
					 if (seqTimer > iniCanPlayF) { canPlay = true; iniSeqStart = false; }
				}
			}

			if (itemsWait)
			{
				itemWaitTimer += Time.deltaTime;
				if (itemWaitTimer > itemWaitAmnt)
				{
					itemHolder.SetActive(true);
					clickToRotTileScript.connectionsNeeded = clickToRotTileScript.CalculateConnectionsNeeded(); // For fun, to try giving a method a return type for the first time.
					resetTilesScript.FillTileResetArray();

					//for (int i = 0; i < resetTilesScript.tiles.Count; i++) // CONSIDER SAVING THE ITEM SCRIPTS TO ANOTHER LIST TO AVOID LOOPING 7 to 12 GETCOMPONENTS AT A TIME
					//{ resetTilesScript.tiles[i].GetComponent<FadeInOutSprite>().FadeIn(); }

					LvlStuffFadeIn();

					itemsWait = false;
					itemWaitTimer = 0f;
					canPlay = true;
					mySelectButton.InteractableThreeDots(maxLvl,curntLvl);
				}
			}

			if (setupChsnLvl) { ChosenLevelSetup(lvlToLoad);}
			// Turn off interaction for all three level select dots.
			if (!mySelectButton.buttonsOff) { mySelectButton.buttonsOff = true; mySelectButton.UninteractableThreeDots();}

			#region Click On SilverEggs
			// Clicking on a silver egg.
			if (myInput.Tapped)
			{
				UpdateMousePos();
				hit = Physics2D.Raycast(mousePos2D, Vector3.forward, 50f);
				if (hit)
				{
					//Debug.Log(hit.collider.name);
					if (hit.collider.CompareTag("Egg"))
					{
						//if (crateScript.curntLvl >= maxLvl) { silverEggsPickedUp += 1; }
						/* Debug.Log("Thats Silver Egg #" + silverEggsPickedUp +" mate"); */
						SilverEggs silEggTappedScript = hit.collider.gameObject.GetComponent<SilverEggs>();
						silEggTappedScript.StartSilverEggAnim();
						hit.collider.enabled = false;

						//SFX CLICK SILVER EGG
						audioSceneParkPuzzScript.silverEgg();
						
						if (!silEggTappedScript.hollow) { mySilverEggMan.silverEggsPickedUp++; }
						mySilverEggMan.SaveSilverEggsToCorrectFile();
						mySilverEggMan.SaveNewSilEggsFound(mySilverEggMan.allSilEggs.IndexOf(hit.collider.gameObject));
						
						mySilverEggMan.amntSilEggsTapped++;
						SilverEggsCheck(); // Check if the Silver Eggs have all been collected.
					}
				}
			}
			#endregion
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
	void InitialSetup()
	{
		//Debug.Log("Initial Setup");
		if(maxLvl > 3 || maxLvl < 1) { curntLvl = 1; }
		else { curntLvl = maxLvl; }
		//curntPounds = 0;
		//curntAmnt = 0;
		itemHolder = lvlItemHolders[curntLvl - 1];
		//lvlItemHolders[crateScript.curntLvl - 1].SetActive(true); // IN THE INI SEQUENCE
		resetTilesScript.FillTileResetArray();
		//canPlay = true; // IN THE INI SEQUENCE
		initialSetupOn = false;
		//crateScript.UpdateRequirements(); INSERT UPDATE REQUIREMENTS FOR LEVEL X ------------------------------------------------------------
		iniSeqStart = true;
		//EnabledThreeDots(); // IN THE INI SEQUENCE
		//InteractableThreeDots(); // IN THE INI SEQUENCE
	}

	// Level complete, load silver eggs, start crate animation.
	void SilverEggsSetup()
	{
		//Debug.Log("New Level Setup");
		canPlay = false;
		
		//Set the silver egg sprites to Hollow if the egg was found already.
		for (int i = 0; i < GlobalVariables.globVarScript.parkPuzzSilEggsCount.Count; i++)
		{
			int eggNumber = GlobalVariables.globVarScript.parkPuzzSilEggsCount[i];
			mySilverEggMan.allSilEggs[eggNumber].GetComponent<SpriteRenderer>().sprite = mySilverEggMan.hollowSilEgg;
			mySilverEggMan.allSilverEggsScripts[eggNumber].hollow = true;
			Debug.Log(mySilverEggMan.allSilEggs[eggNumber].name + "has been set to hollow, ooouuuhhhh. Like a ghost. A nice ghost. Yeeah.");
		}

		mySilverEggMan.lvlSilverEggs[curntLvl - 1].SetActive(true); // CAN probably set it to true in the lvl finished seq or wtv
		if (mySilverEggMan.lvlSilverEggs[curntLvl - 1].transform.childCount > 0)
		{
			//List<GameObject> activeSilverEggs = new List<GameObject>();
			foreach (Transform silEgg in mySilverEggMan.lvlSilverEggs[curntLvl - 1].transform)
			{
				mySilverEggMan.activeSilverEggs.Add(silEgg.gameObject);
				//Debug.Log(silEgg.name + "has been added to the active Silver Egg List!");
			}
		}

		mySilverEggMan.silverEggsActive = true;

		if (!mySelectButton.noFadeDelay) { mySelectButton.TurnFadeDelayOff(); mySelectButton.noFadeDelay = true; } // Turn off the initial fade delay for the three dots. Should only happen once.

		LvlStuffFadeOut();
		bgScleScript.ScaleBG();

		//StartCoroutine(MoveCrateRight()); // INSERT KITE ANIM SEQUENCE ------------------------------------------------------------------------------------------------------
		//reqParchMoveScript.moveToHidden = true;
		foreach(GameObject silEgg in mySilverEggMan.activeSilverEggs) // TO BE PUT IN THE ANIM SEQ -------------------------------------------------------------------------------------------
		{
			silEgg.GetComponent<SilverEggSequence>().StartSequence();
		}
		scrnDarkImgScript.FadeIn();

		connections = 0;
	}

	// Checks if the player tapped enough silver eggs to move on, change the current level.
	public void SilverEggsCheck()
	{
		//int amntSilEggsTapped = 0;
		if (mySilverEggMan.activeSilverEggs.Count > 0)
		{
			if (mySilverEggMan.amntSilEggsTapped == mySilverEggMan.activeSilverEggs.Count) 
			{			
				mySilverEggMan.activeSilverEggs.Clear();
				mySilverEggMan.silverEggsActive = false;
				mySilverEggMan.amntSilEggsTapped = 0;
				scrnDarkImgScript.FadeOut();
				//silverEggsPickedUp > GlobalVariables.globVarScript.marketSilverEggsCount
				curntLvl++;
				if (curntLvl > maxLvl) 
				{ maxLvl = curntLvl; SaveMaxLvl(); mySelectButton.EnabledThreeDots(maxLvl); }

				voidDelegate = NextLevelSetup;
				if (!waitMethod) { waitMethod = true; } else { Debug.LogError("waitMethod IS ALREADY IN PROGRESS, DONT DO THAT!!"); }
				waitTimer = waitTime;
				if (curntLvl  < winLvl) { bgScleScript.ScaleBG(); }
			}
		}
	}

	// Lets you feed set a method as a parameter in a method.
	void RunAfter(VoidDelegate methToRun)
	{
		methToRun();
	}

	// Once animations are finished, run the next level setup.
	void NextLevelSetup()
	{
		foreach(SilverEggs silEggs in mySilverEggMan.lvlSilverEggs[curntLvl - 2].GetComponentsInChildren<SilverEggs>())
		{ silEggs.ResetSilEgg(); Debug.Log(silEggs.gameObject.name);}
		mySilverEggMan.lvlSilverEggs[curntLvl - 2].SetActive(false);
		resetTilesScript.EndOfLevelReset();
		//itemHolder.SetActive(false);
		// crateAnim.SetTrigger("MoveDown");
		// StartCoroutine(MoveCrateDown());
		chngLvlTimer = 0f;

		if (curntLvl >= winLvl) 
		{
			StartCoroutine(PuzzleComplete());
			return;
		}
		itemHolder.SetActive(false);
		itemHolder = lvlItemHolders[curntLvl - 1];
		
		itemsWait = true;
		//for (int i = 0; i < resetItemsButtonScript.items.Count; i++) // CONSIDER SAVING THE ITEM SCRIPTS TO ANOTHER LIST TO AVOID LOOPING 7 to 12 GETCOMPONENTS AT A TIME
		//{ resetItemsButtonScript.items[i].GetComponent<Items>().FadeIn(); }

		//canPlay = true;
		//crateScript.UpdateRequirements();
		//reqParchMoveScript.moveToShown = true;
	}

	// Prepare to change level after a level selection button has been pressed.
	public void ChangeLevelSetup()
	{
		// Close up current level.
		canPlay = false;

		mySelectButton.UninteractableThreeDots();

		LvlStuffFadeOut();

		//for (int i = 0; i < resetTilesScript.tiles.Count; i++) // CONSIDER SAVING THE ITEM SCRIPTS TO ANOTHER LIST TO AVOID LOOPING 7 to 12 GETCOMPONENTS AT A TIME
		//{ resetTilesScript.tiles[i].GetComponent<FadeInOutSprite>().FadeOut(); }

		//reqParchMoveScript.moveToHidden = true;
		
		setupChsnLvl = true;
	}

	// Setup the chosen level after waiting for setupLvlWaitTime (minimum the fade out duration of the items).
	void ChosenLevelSetup(int lvlToLoad)
	{
		// Setup chosen level.
		chngLvlTimer += Time.deltaTime;
		if (chngLvlTimer > setupLvlWaitTime)
		{
			lvlItemHolders[curntLvl - 1].SetActive(false);
			resetTilesScript.EndOfLevelReset();
			curntLvl = lvlToLoad;
			//curntPounds = 0;
			//curntAmnt = 0;
			itemHolder = lvlItemHolders[curntLvl - 1];
			itemsWait = true;
			bgScleScript.ScaleBG();
			//crateScript.UpdateRequirements();
			//reqParchMoveScript.moveToShown = true;
			setupChsnLvl = false;
			chngLvlTimer = 0;
		}
	}
	#endregion

	#region General Methods
	// Which of the three dots are interactable based on the highest playable level.
	// void EnabledThreeDots()
	// {
	// 	if (maxLvl == 0) { lvlSelectButtons[0].SetActive(true); }
	// 	for(int i = 0; i < maxLvl && i < lvlSelectButtons.Count; i++)
	// 	{ 
	// 		if (!lvlSelectButtons[i].activeSelf) 
	// 		{ lvlSelectButtons[i].SetActive(true); } 
	// 	}
	// }

	// Which of the three dots can be interacted with, also adjust the scale accordingly.
	// void InteractableThreeDots()
	// {
	// 	if (maxLvl == 0) { lvlSelectScalers[0].ScaleUp(); }
	// 	for (int i = 0; i < maxLvl && i < lvlSelectButtons.Count; i++)
	// 	{
	// 		if (lvlSelectButtons[i] == lvlSelectButtons[curntLvl - 1])
	// 		{
	// 			lvlSelectButtons[i].GetComponent<Button>().interactable = false;
	// 			lvlSelectScalers[i].ScaleUp();
	// 		}
	// 		else 
	// 		{
	// 			lvlSelectButtons[i].GetComponent<Button>().interactable = true; 
	// 			lvlSelectScalers[i].ScaleDown();
	// 		}
	// 	}
	// }

	// Make level select buttons uninteractable between levels.
	// void UninteractableThreeDots()
	// {
	// 	foreach(GameObject lvlButton in lvlSelectButtons)
	// 	{
	// 		if (lvlButton.activeSelf) { lvlButton.GetComponent<Button>().interactable = false; }	
	// 	}
	// }
	
	// After the initial level setup turn off the three dots fade delay.
	// void TurnFadeDelayOff()
	// {
	// 	foreach(FadeInOutImage fadeImgScpt in lvlSelectFades)
	// 	{ fadeImgScpt.fadeDelay = false; }
	// }


	public void LvlStuffFadeIn()
	{
		//Debug.Log("Should fade in stuff."); // Fade in tiles
		if (!lvlItemHolders[curntLvl -1].activeSelf) lvlItemHolders[curntLvl -1].SetActive(true);
		FadeInOutSprite[] childrenTileFadeScripts; // CONSIDER SAVING THE ITEM SCRIPTS TO ANOTHER LIST TO AVOID LOOPING 7 to 12 GETCOMPONENTS AT A TIME
		childrenTileFadeScripts = lvlItemHolders[curntLvl - 1].transform.GetComponentsInChildren<FadeInOutSprite>(); 
		for (int i = 0; i < childrenTileFadeScripts.Length; i++)
		{ childrenTileFadeScripts[i].FadeIn(); }
		// Fade in tile backs
		if (!lvlTileBacksFadeScripts[curntLvl -1].activeSelf) lvlTileBacksFadeScripts[curntLvl -1].SetActive(true);
		FadeInOutSprite[] childrenBackFadeScripts; // CONSIDER SAVING THE ITEM SCRIPTS TO ANOTHER LIST TO AVOID LOOPING 7 to 12 GETCOMPONENTS AT A TIME
		childrenBackFadeScripts = lvlTileBacksFadeScripts[curntLvl - 1].transform.GetComponentsInChildren<FadeInOutSprite>(); 
		for (int i = 0; i < childrenBackFadeScripts.Length; i++)
		{ if (!childrenBackFadeScripts[i].gameObject.activeSelf) {childrenBackFadeScripts[i].gameObject.SetActive(true); } childrenBackFadeScripts[i].FadeIn(); }
		// Fade in back shadow
		if (!lvlBackShadowsFadeScripts[curntLvl - 1].gameObject.activeSelf) lvlBackShadowsFadeScripts[curntLvl - 1].gameObject.SetActive(true);
		lvlBackShadowsFadeScripts[curntLvl - 1].FadeIn();
		// Fade in kite & kite stuff
		if (!lvlKitesFadeScripts[curntLvl - 1].gameObject.activeSelf) lvlKitesFadeScripts[curntLvl - 1].gameObject.SetActive(true);
		lvlKitesFadeScripts[curntLvl - 1].FadeIn();
		if (lvlKites[curntLvl - 1].transform.childCount > 0)
		{
			foreach(Transform lvlKite in lvlKites[curntLvl - 1].transform)
			{
				lvlKite.GetComponent<FadeInOutSprite>().FadeIn();
			}
		}
	}

	public void LvlStuffFadeOut() // Fade out tiles, tile backs, kite, backshadow.
	{
		FadeInOutSprite[] childrenTileFadeScripts; // CONSIDER SAVING THE ITEM SCRIPTS TO ANOTHER LIST TO AVOID LOOPING 7 to 12 GETCOMPONENTS AT A TIME
		childrenTileFadeScripts = lvlItemHolders[curntLvl - 1].transform.GetComponentsInChildren<FadeInOutSprite>(); 
		for (int i = 0; i < childrenTileFadeScripts.Length; i++)
		{ childrenTileFadeScripts[i].FadeOut(); }

		FadeInOutSprite[] childrenBackFadeScripts; // CONSIDER SAVING THE ITEM SCRIPTS TO ANOTHER LIST TO AVOID LOOPING 7 to 12 GETCOMPONENTS AT A TIME
		childrenBackFadeScripts = lvlTileBacksFadeScripts[curntLvl - 1].transform.GetComponentsInChildren<FadeInOutSprite>(); 
		for (int i = 0; i < childrenBackFadeScripts.Length; i++)
		{ childrenBackFadeScripts[i].FadeOut(); }

		lvlBackShadowsFadeScripts[curntLvl - 1].FadeOut();

		lvlKitesFadeScripts[curntLvl - 1].FadeOut();
		if (lvlKites[curntLvl - 1].transform.childCount > 0)
		{
			foreach(Transform lvlKite in lvlKites[curntLvl - 1].transform)
			{
				lvlKite.GetComponent<FadeInOutSprite>().FadeOut();
			}
		}
	}

	/* public void SaveSilverEggsToCorrectFile()
	{
		if (silverEggsPickedUp > GlobalVariables.globVarScript.parkSilverEggsCount) 
		{
			GlobalVariables.globVarScript.parkTotalEggsFound += 1;
			GlobalVariables.globVarScript.parkSilverEggsCount = silverEggsPickedUp; 
			GlobalVariables.globVarScript.SaveEggState();
		}
	}*/

	/*public void SaveNewSilEggsFound(int newSilEggFound)
	{
		//bool alreadySaved = false;
		foreach (int silEggNumber in GlobalVariables.globVarScript.parkPuzzSilEggsCount)
		{
			if (silEggNumber == newSilEggFound)
			{
				return;
			}
		}
		GlobalVariables.globVarScript.parkPuzzSilEggsCount.Add(newSilEggFound);
		GlobalVariables.globVarScript.SaveEggState();
	}*/

	public void SaveMaxLvl()
	{
		if (maxLvl > GlobalVariables.globVarScript.parkPuzzMaxLvl)
		{
			GlobalVariables.globVarScript.parkPuzzMaxLvl = maxLvl;
			GlobalVariables.globVarScript.SaveEggState();
		}
	}

	void UpdateMousePos()
	{
		mousePos = Camera.main.ScreenToWorldPoint(myInput.TapPosition);
		mousePos2D = new Vector2 (mousePos.x, mousePos.y);
	}
	#endregion

	#region Coroutines
	// // Move crate to the right.
	// public IEnumerator MoveCrateRight ()
	// {
	// 	//Make it skip a frame to make sure that the animation has time to start.
	// 	yield return new WaitForSeconds(0.0001f);
	// 	//yield return new WaitUntil(!crateAnim.IsInTransition(0));
		

	// 	while (crateAnim.transform.parent.rotation != crateInSceneTransform.rotation)
	// 	{
	// 		// crateAnim.transform.parent.eulerAngles = Vector3.Lerp(crateAnim.transform.parent.eulerAngles, crateInSceneTransform.eulerAngles, Time.deltaTime);
	// 		float Zangle = crateAnim.transform.parent.eulerAngles.z;
	// 		Zangle = Mathf.LerpAngle(crateAnim.transform.parent.eulerAngles.z, 0f, Time.deltaTime * crateMoveSpeed);
	// 		crateAnim.transform.parent.eulerAngles = new Vector3(0, 0, Zangle);
	// 		//Debug.Log(crateAnim.transform.parent.eulerAngles);

	// 		if (Vector3.Distance(crateAnim.transform.parent.eulerAngles, crateInSceneTransform.eulerAngles) <= 0.1f)
	// 		{
	// 			crateAnim.transform.parent.rotation = crateInSceneTransform.rotation;
	// 		}

	// 		yield return null;
	// 	}
	// 	crateAnim.SetTrigger("MoveRight");

	// 	yield return new WaitForSeconds(0.0001f);

	// 	while (crateAnim.GetCurrentAnimatorStateInfo(0).IsName("CrateMoveRight"))
	// 	{
	// 		//Debug.Log("Playing anim move right.");
	// 		yield return null;
	// 	}

	// 	// foreach (Transform item in crateParent.transform) // DONT THINK ITS NEEDED 
	// 	// {
	// 	// 	//Debug.Log("Going through the keeds");
	// 	// 	if(item.gameObject.CompareTag("Item"))
	// 	// 	{
	// 	// 		SpriteRenderer sprRen = item.GetComponent<SpriteRenderer>();
	// 	// 		sprRen.color = new Color(sprRen.color.r, sprRen.color.g, sprRen.color.b, 0f);
	// 	// 	}
	// 	// }
	// 	scaleScript.itemOnScale = null; // Deleted both scale lines if we want scale arrow to reset after silver eggs have been clicked. 
	// 	scaleScript.isAnItemOnScale = false; //

	// 	crateParent.transform.parent.position = crateTopTransform.position;
	// 	crateParent.transform.parent.rotation = crateTopTransform.rotation;

	// 	lvlSilverEggs[curntLvl - 1].SetActive(true);
	// 	resetItemsButtonScript.EndOfLevelReset();
	// 	itemHolder.SetActive(false);
	// 	scrnDarkImgScript.FadeIn();
	// 	crateAnim.SetTrigger("MoveDown");
	// 	StartCoroutine(MoveCrateDown());
	// }


	// // Move crate down.
	// public IEnumerator MoveCrateDown ()
	// {
	// 	//Debug.Log("Entered Coroutine MoveCrateDown. Yo.");
	// 	yield return new WaitUntil(() => crateParent.transform.parent.position == crateTopTransform.position);
	// 	//Debug.Log("CrateParent pos = crateTop pos.");
	// 	while (crateAnim.GetCurrentAnimatorStateInfo(0).IsName("CrateMoveDown"))
	// 	{
	// 		//Debug.Log("MoveCrateDown Animating.");
	// 		yield return null;
	// 	}
	// 	//Debug.Log("should take anim pos");
	// 	crateParent.transform.parent.position = crateAnim.transform.position;
	// 	crateParent.transform.parent.rotation = crateAnim.transform.rotation;

	// 	// foreach activesilvereggs getcomponenet silvereggsequence startsequence = true
	// 	foreach(GameObject silEgg in activeSilverEggs)
	// 	{
	// 		silEgg.GetComponent<SilverEggSequence>().StartSequence();
	// 	}
	// }


	// All silver eggs picked up, what happenes?
	public IEnumerator PuzzleComplete ()
	{
		yield return new WaitForSeconds(0.5f);

		Debug.Log("Puzzle Completed cognraturations!!!");

		yield return new WaitForSeconds(0.5f);

		audioSceneParkPuzzScript.StopSceneMusic();
		audioSceneParkPuzzScript.PlayTransitionMusic();

		SceneFade.SwitchScene(GlobalVariables.globVarScript.parkName);
	}
	#endregion
}