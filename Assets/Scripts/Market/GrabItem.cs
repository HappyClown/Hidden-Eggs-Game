using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class GrabItem : MonoBehaviour 
{
	#region GrabItem Script Variables
	[Header("General")]
	public bool tutorialDone;
	public float itemScaleMult;
	public int winLvl;
	public bool itemsWait;
	public float itemWaitAmnt;
	private float itemWaitTimer;

	private bool initialSetupOn;
	private bool setupChsnLvl;
	private bool holdingItem;
	private GameObject heldItem;
	// For Raycast
	private RaycastHit2D hit;
	private Vector2 mousePos2D;
	private Vector3 mousePos;
	// For Delegate Method
	private delegate void VoidDelegate();
	private VoidDelegate voidDelegate; 
	private bool waitMethod;
	private float waitTime;

	[Header("Crate")]
	public float crateMoveSpeed;
	public Animator crateAnim;
	public Transform crateTopTransform, crateInSceneTransform;
	public GameObject inCrateCollider;
	public float waitBeforeCrteDown;

	[Header("Tagged Colliders & Position Snaps")]
	public GameObject scaleSnapPos;
	public Transform crateSnapPos;

	[Header("Item Parents")]
	[Tooltip("GameObjects - Game objects that hold the level items, in ascending order.")] public List<GameObject> lvlItemHolders;
	public GameObject itemHolder, crateParent;

	[Header("Silver Eggs")]
	public int silverEggsPickedUp;
	public Sprite hollowSilEgg;
	public List<GameObject> lvlSilverEggs, activeSilverEggs, allSilEggs;
	public List<SilverEggs> allSilverEggsScripts;
	private bool silverEggsActive;
	public int amntSilEggsTapped;

	[Header("Level Selection Buttons")]
	[Tooltip("GameObjects - The level selection buttons, in ascending order. ( 1 - 0, 2 - 1, etc.")] 
	public List<GameObject> lvlSelectButtons;
	[Tooltip("Scripts - The FadeInOutImage scripts, in ascending order. ( 1 - 0, 2 - 1, etc.")] 
	public List<FadeInOutImage> lvlSelectFades;
	[Tooltip("Scripts - The Scaler scripts, in ascending order. ( 1 - 0, 2 - 1, etc.")] 
	public List<Scaler> lvlSelectScalers;
	private bool noFadeDelay;
	private bool buttonsOff;

	[Header("Initial Sequence")]
	public float seqTimer;
	public float iniSeqDelay, crateDownF, reqDownF, itemSpawnF, dotsSpawnF, iniCanPlayF;
	private bool iniSeqStart, crateDownB, reqDownB, itemSpawnB, dotsSpawnB;

	[Header("Scripts")]
	public Scale scaleScript;
	public Crate crateScript;
	public ResetItemsButton resetItemsButtonScript;
	public Items refItemScript;
	public FadeInOutImage scrnDarkImgScript;
	public ReqParchmentMove reqParchMoveScript;
	public SlideInHelpBird slideInHelpScript;
	public LevelSelectionButtons mySelectButton;

	[Header("Hide In Inspector ^_^")]
	public bool canPlay;
	public float curntPounds, curntAmnt;
	public int lvlToLoad;
	public float setupLvlWaitTime;
	public float chngLvlTimer;
	public int maxLvl;
	public int curntLvl;
	#endregion

	public AudioSceneMarketPuzzle audioSceneMarketPuz;


	void Start ()
	{
		canPlay = false;
		initialSetupOn = true;
		heldItem = null;
		maxLvl = GlobalVariables.globVarScript.puzzMaxLvl;
		silverEggsPickedUp = GlobalVariables.globVarScript.silverEggsCount;
		if (setupLvlWaitTime < refItemScript.fadeDuration) setupLvlWaitTime = refItemScript.fadeDuration;
	}

	void Update () 
	{
		if (canPlay)
		{
			if(mySelectButton.buttonPressed){
				lvlToLoad = mySelectButton.lvlToLoad;
				if (chngLvlTimer >= setupLvlWaitTime && curntLvl != lvlToLoad && maxLvl >= lvlToLoad){
					chngLvlTimer = 0f;
					ChangeLevelSetup();
				}
				mySelectButton.buttonPressed = false;
			}

			if (chngLvlTimer < setupLvlWaitTime) { chngLvlTimer += Time.deltaTime; }

			// Current level complete.
			if (curntPounds == crateScript.reqPounds && curntAmnt == crateScript.reqItems) { SilverEggsSetup(); }

			if (buttonsOff) { buttonsOff = false; InteractableThreeDots(); }

			#region Click
			// Click //
			if (Input.GetMouseButtonDown(0) && !holdingItem)
			{
				UpdateMousePos();
				hit = Physics2D.Raycast(mousePos2D, Vector3.forward, 50f);

				if (hit)
				{
					if (hit.collider.CompareTag("Item"))
					{
						holdingItem = true;
						heldItem = hit.collider.gameObject;
						heldItem.transform.parent = itemHolder.transform;
						heldItem.transform.localScale = heldItem.transform.localScale * itemScaleMult;

						if (heldItem == scaleScript.itemOnScale)
						{
							scaleScript.isAnItemOnScale = false;
							scaleScript.itemOnScale = null;
						}

						if (heldItem.GetComponent<Items>().inCrate == true)
						{
							curntPounds -= heldItem.GetComponent<Items>().weight;
							curntAmnt -= 1;
							heldItem.GetComponent<Items>().inCrate = false;
						}

						// SFX PickUp Item
						audioSceneMarketPuz.pickupFruit();
					}
				}
			}
			#endregion

			#region Drag
			// Drag //
			else if (Input.GetMouseButton(0) && holdingItem == true)
			{
				UpdateMousePos();
				float heldItemObjX = heldItem.transform.position.x;
				float heldItemObjY = heldItem.transform.position.y;

				heldItemObjX = mousePos.x;
				heldItemObjY = mousePos.y;

				heldItem.transform.position = new Vector3(heldItemObjX, heldItemObjY, -5f);
			}
			#endregion

			#region Drop
			// Let go and decide where to put the item //
			if (Input.GetMouseButtonUp(0) && holdingItem == true)
			{
				UpdateMousePos();
				holdingItem = false;
				heldItem.transform.localScale = heldItem.transform.localScale / itemScaleMult;

				RaycastHit2D[] hits;
				hits = Physics2D.RaycastAll(mousePos2D, Vector3.forward, 50f);
				for (int i =0; i < hits.Length; i++)
				{
					//Debug.Log(hits[i].collider.name);
					// ON THE SCALE AREA//
					if (hits[i].collider.gameObject.CompareTag("Scale"))
					{
						if (scaleScript.itemOnScale != null)
						{
							scaleScript.itemOnScale.transform.position = scaleScript.itemOnScale.GetComponent<Items>().initialPos;
							scaleScript.itemOnScale.transform.parent = itemHolder.transform;
						}
						heldItem.transform.position = new Vector3(scaleSnapPos.transform.position.x, scaleSnapPos.transform.position.y, -5f);
						heldItem.transform.parent = scaleSnapPos.transform;
						
						scaleScript.itemOnScale = heldItem;
						scaleScript.isAnItemOnScale = true;

						//SFK Drop Item
						audioSceneMarketPuz.dropFruitScale();
						break;
					}

					// ON THE TABLE AREA//
					else if (hits[i].collider.gameObject.CompareTag("Table"))
					{
						heldItem.transform.position = new Vector3(mousePos.x, mousePos.y, -5f);
						//SFX DROP ON WOOD
						audioSceneMarketPuz.dropFruitCrate();
						break;
					}

					// IN THE CRATE DIRECTLY//
					else if (hits[i].collider.gameObject.CompareTag("InCrate"))
					{
						heldItem.GetComponent<Items>().inCrate = true;
						heldItem.transform.position = new Vector3(mousePos.x, mousePos.y, -5f);
						heldItem.transform.parent = crateParent.transform;
						curntPounds += heldItem.GetComponent<Items>().weight;
						curntAmnt += 1;
						//SFX DROP ON WOOD
						audioSceneMarketPuz.dropFruitCrate();
						break;
					}

					// IN THE CRATE AREA//
					else if (hits[i].collider.gameObject.CompareTag("Crate"))
					{
						heldItem.GetComponent<Items>().inCrate = true;
						heldItem.transform.position = new Vector3(crateSnapPos.transform.position.x, crateSnapPos.transform.position.y, -5f);
						heldItem.transform.parent = crateParent.transform;
						curntPounds += heldItem.GetComponent<Items>().weight;
						curntAmnt += 1;
						//SFX DROP ON WOOD
						audioSceneMarketPuz.dropFruitCrate();
						break;
					}

					// Cannot drop items outside of the screen. If item held does Not hit any of the areas send it back to its initial position on the table.
					else 
					{
						heldItem.transform.position = heldItem.GetComponent<Items>().initialPos;
					}
				}
				heldItem = null;
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
					if (seqTimer > crateDownF && !crateDownB) { crateDownB = true; crateAnim.SetTrigger("MoveDown"); audioSceneMarketPuz.crateSlideDown(); StartCoroutine(MoveCrateDown()); }
					if (seqTimer > reqDownF && !reqDownB) { reqDownB = true; reqParchMoveScript.moveToShown = true; audioSceneMarketPuz.openPanel(); } 
					if (seqTimer > itemSpawnF && !itemSpawnB) { itemSpawnB = true; lvlItemHolders[crateScript.curntLvl - 1].SetActive(true); for (int i = 0; i < resetItemsButtonScript.items.Count; i++) // CONSIDER SAVING THE ITEM SCRIPTS TO ANOTHER LIST TO AVOID LOOPING 7 to 12 GETCOMPONENTS AT A TIME
					{ resetItemsButtonScript.items[i].GetComponent<Items>().FadeIn(); } }
					if (seqTimer > dotsSpawnF && !dotsSpawnB) { dotsSpawnB = true; EnabledThreeDots(); UninteractableThreeDots(); }
					if (seqTimer > iniCanPlayF) { 
						if (tutorialDone) {
							canPlay = true; 
							InteractableThreeDots();
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
					resetItemsButtonScript.FillItemResetArray();

					for (int i = 0; i < resetItemsButtonScript.items.Count; i++) // CONSIDER SAVING THE ITEM SCRIPTS TO ANOTHER LIST TO AVOID LOOPING 7 to 12 GETCOMPONENTS AT A TIME
					{ resetItemsButtonScript.items[i].GetComponent<Items>().FadeIn(); }

					itemsWait = false;
					itemWaitTimer = 0f;
					canPlay = true;
					InteractableThreeDots();
				}
			}

			if (setupChsnLvl) { ChosenLevelSetup(lvlToLoad);}
			// Turn off interaction for all three level select dots.
			if (!buttonsOff) { buttonsOff = true; UninteractableThreeDots();}

			#region Click On SilverEggs
			// Clicking on a silver egg.
			if (Input.GetMouseButtonDown(0))
			{		
				UpdateMousePos();
				hit = Physics2D.Raycast(mousePos2D, Vector3.forward, 50f);
				if (hit)
				{
					//Debug.Log(hit.collider.name);
					if (hit.collider.CompareTag("Egg"))
					{
						//if (crateScript.curntLvl >= maxLvl) { silverEggsPickedUp += 1; }
						Debug.Log("Thats Silver Egg #" + silverEggsPickedUp +" mate");
						SilverEggs silEggTappedScript = hit.collider.gameObject.GetComponent<SilverEggs>();
						silEggTappedScript.StartSilverEggAnim();
						hit.collider.enabled = false;
						
						if (!silEggTappedScript.hollow) { silverEggsPickedUp++; }
						SaveSilverEggsToCorrectFile();
						SaveNewSilEggsFound(allSilEggs.IndexOf(hit.collider.gameObject));
						
						amntSilEggsTapped++;
						SilverEggsCheck(); // Check if the Silver Eggs have all been collected.

						//SFX Silver Egg
						//SFX DROP ON WOOD
						audioSceneMarketPuz.silverEggSnd();
					}
				}
			}
			#endregion
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
	void InitialSetup()
	{
		//Debug.Log("Initial Setup");
		if(maxLvl > 3 || maxLvl < 1) { crateScript.curntLvl = 1; }
		else { crateScript.curntLvl = maxLvl; }
		curntPounds = 0;
		curntAmnt = 0;
		itemHolder = lvlItemHolders[crateScript.curntLvl - 1];
		//lvlItemHolders[crateScript.curntLvl - 1].SetActive(true); // IN THE INI SEQUENCE
		resetItemsButtonScript.FillItemResetArray();
		//canPlay = true; // IN THE INI SEQUENCE
		initialSetupOn = false;
		crateScript.UpdateRequirements();
		iniSeqStart = true;
		//EnabledThreeDots(); // IN THE INI SEQUENCE
		//InteractableThreeDots(); // IN THE INI SEQUENCE
	}

	// Level complete, load silver eggs, start crate animation.
	void SilverEggsSetup()
	{
		//Debug.Log("New Level Setup");
		canPlay = false;
		
		// if (maxLvl > crateScript.curntLvl)
		// {
		// 	foreach (Transform silEgg in lvlSilverEggs[crateScript.curntLvl - 1].transform)
		// 	{
		// 		silEgg.GetComponent<SpriteRenderer>().sprite = hollowSilEgg;
		// 		allSilverEggsScripts.IndexOf(silEgg).hollow = true;
		// 		Debug.Log(silEgg.name + "has been set to hollow, ooouuuhhhh. Like a ghost. A nice ghost. Yeeah.");
		// 	}
		// }

		// foreach (Transform silEgg in GlobalVariables.globVarScript.marketPuzzSilEggsCount[crateScript.curntLvl - 1])
		// {
		// 	silEgg.GetComponent<SpriteRenderer>().sprite = hollowSilEgg;
		// 	Debug.Log(silEgg.name + "has been set to hollow, ooouuuhhhh. Like a ghost. A nice ghost. Yeeah.");
		// }

		//Set the silver egg sprites to Hollow if the egg was found already.
		for (int i = 0; i < GlobalVariables.globVarScript.puzzSilEggsCount.Count; i++)
		{
			int eggNumber = GlobalVariables.globVarScript.puzzSilEggsCount[i];
			allSilEggs[eggNumber].GetComponent<SpriteRenderer>().sprite = hollowSilEgg;
			allSilverEggsScripts[eggNumber].hollow = true;
			Debug.Log(allSilEggs[eggNumber].name + "has been set to hollow, ooouuuhhhh. Like a ghost. A nice ghost. Yeeah.");
		}


		//lvlSilverEggs[crateScript.curntLvl - 1].SetActive(true);
		if (lvlSilverEggs[crateScript.curntLvl - 1].transform.childCount > 0)
		{
			//List<GameObject> activeSilverEggs = new List<GameObject>();
			foreach (Transform silEgg in lvlSilverEggs[crateScript.curntLvl - 1].transform)
			{
				activeSilverEggs.Add(silEgg.gameObject);
				//Debug.Log(silEgg.name + "has been added to the active Silver Egg List!");
			}
		}

		silverEggsActive = true;

		if (!noFadeDelay) { TurnFadeDelayOff(); noFadeDelay = true; } // Turn off the initial fade delay for the three dots. Should only happen once.

		//if (crateScript.curntLvl >= maxLvl) { silverEggsPickedUp += activeSilverEggs.Count; }


		// Fade out the finished level's items. (Except the ones in the crate.)
		if (scaleScript.itemOnScale != null) { scaleScript.itemOnScale.transform.parent = itemHolder.transform; }
		
		// for (int i = 0; i < resetItemsButtonScript.items.Length; i++) // CONSIDER SAVING THE ITEM SCRIPTS TO ANOTHER LIST TO AVOID LOOPING 7 to 12 GETCOMPONENTS AT A TIME
		// { resetItemsButtonScript.items[i].GetComponent<Items>().FadeOut(); }

		Items[] childrenItemScripts; // CONSIDER SAVING THE ITEM SCRIPTS TO ANOTHER LIST TO AVOID LOOPING 7 to 12 GETCOMPONENTS AT A TIME
		childrenItemScripts = lvlItemHolders[crateScript.curntLvl - 1].transform.GetComponentsInChildren<Items>(); 
		for (int i = 0; i < childrenItemScripts.Length; i++)
		{ childrenItemScripts[i].FadeOut(); }

		StartCoroutine(MoveCrateRight());
		reqParchMoveScript.moveToHidden = true;
		audioSceneMarketPuz.closePanel();

		curntPounds = 0;
		curntAmnt = 0;
	}

	// Checks if the player tapped enough silver eggs to move on, change the current level.
	public void SilverEggsCheck()
	{
		//int amntSilEggsTapped = 0;
		if (activeSilverEggs.Count > 0)
		{
			// foreach(GameObject silEgg in activeSilverEggs)
			// {
			// 	if (silEgg.GetComponent<CircleCollider2D>().enabled == false) { amntSilEggsTapped++; }
			// 	Debug.Log("Amount of SilverEggs loaded: " + activeSilverEggs.Count + "& amount of Silver Eggs tapped: " + amntSilEggsTapped);
			// }

			if (amntSilEggsTapped == activeSilverEggs.Count) 
			{			
				activeSilverEggs.Clear();
				silverEggsActive = false;
				amntSilEggsTapped = 0;
				scrnDarkImgScript.FadeOut();
				//silverEggsPickedUp > GlobalVariables.globVarScript.marketSilverEggsCount
				crateScript.curntLvl++;
				if (crateScript.curntLvl > maxLvl) 
				{ maxLvl = crateScript.curntLvl; SaveMaxLvl(); EnabledThreeDots(); }

				voidDelegate = NextLevelSetup;
				if (!waitMethod) { waitMethod = true; } else { Debug.LogError("waitMethod IS ALREADY IN PROGRESS, DONT DO THAT!!"); }
				waitTime = waitBeforeCrteDown;
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
		foreach(SilverEggs silEggs in lvlSilverEggs[crateScript.curntLvl - 2].GetComponentsInChildren<SilverEggs>())
		{ silEggs.ResetSilEgg(); /* Debug.Log(silEggs.gameObject.name); */}
		lvlSilverEggs[crateScript.curntLvl - 2].SetActive(false);
		//resetItemsButtonScript.EndOfLevelReset();
		//itemHolder.SetActive(false);
		// crateAnim.SetTrigger("MoveDown");
		// StartCoroutine(MoveCrateDown());
		chngLvlTimer = 0f;

		if (crateScript.curntLvl >= winLvl) 
		{
			StartCoroutine(PuzzleComplete());
			return;
		}

		itemHolder = lvlItemHolders[crateScript.curntLvl - 1];
		
		itemsWait = true;
		//for (int i = 0; i < resetItemsButtonScript.items.Count; i++) // CONSIDER SAVING THE ITEM SCRIPTS TO ANOTHER LIST TO AVOID LOOPING 7 to 12 GETCOMPONENTS AT A TIME
		//{ resetItemsButtonScript.items[i].GetComponent<Items>().FadeIn(); }

		//canPlay = true;
		crateScript.UpdateRequirements();
		reqParchMoveScript.moveToShown = true;
		audioSceneMarketPuz.openPanel();
	}

	// Prepare to change level after a level selection button has been pressed.
	public void ChangeLevelSetup()
	{
		// Close up current level.
		canPlay = false;

		UninteractableThreeDots();

		for (int i = 0; i < resetItemsButtonScript.items.Count; i++) // CONSIDER SAVING THE ITEM SCRIPTS TO ANOTHER LIST TO AVOID LOOPING 7 to 12 GETCOMPONENTS AT A TIME
		{ resetItemsButtonScript.items[i].GetComponent<Items>().FadeOut(); }

		reqParchMoveScript.moveToHidden = true;
		audioSceneMarketPuz.closePanel();
		
		setupChsnLvl = true;
		// RESET SILVER EGGs ///////////////////////
	}

	// Setup the chosen level after waiting for setupLvlWaitTime (minimum the fade out duration of the items).
	void ChosenLevelSetup(int lvlToLoad)
	{
		// Setup chosen level.
		chngLvlTimer += Time.deltaTime;
		if (chngLvlTimer > setupLvlWaitTime)
		{
			lvlItemHolders[crateScript.curntLvl - 1].SetActive(false);
			resetItemsButtonScript.EndOfLevelReset();
			crateScript.curntLvl = lvlToLoad;
			curntPounds = 0;
			curntAmnt = 0;
			itemHolder = lvlItemHolders[crateScript.curntLvl - 1];

			//lvlItemHolders[crateScript.curntLvl - 1].SetActive(true);
			//resetItemsButtonScript.FillItemResetArray();
			itemsWait = true;
			// for (int i = 0; i < resetItemsButtonScript.items.Count; i++) // CONSIDER SAVING THE ITEM SCRIPTS TO ANOTHER LIST TO AVOID LOOPING 7 to 12 GETCOMPONENTS AT A TIME
			// { resetItemsButtonScript.items[i].GetComponent<Items>().FadeIn(); }

			//inCrateCollider.transform.position = new Vector3(inCrateCollider.transform.position.x, inCrateCollider.transform.position.y, inCrateCollider.transform.position.z - 1);
			//canPlay = true;
			crateScript.UpdateRequirements();
			reqParchMoveScript.moveToShown = true;
			audioSceneMarketPuz.openPanel();

			setupChsnLvl = false;
			chngLvlTimer = 0;
		}
	}
	#endregion

	#region General Methods
	// Which of the three dots are interactable based on the highest playable level.
	void EnabledThreeDots()
	{
		if (maxLvl == 0) { lvlSelectButtons[0].SetActive(true); }
		for(int i = 0; i < maxLvl && i < lvlSelectButtons.Count; i++)
		{ 
			if (!lvlSelectButtons[i].activeSelf) 
			{ lvlSelectButtons[i].SetActive(true); } 
		}
	}

	// Which of the three dots can be interacted with, also adjust the scale accordingly.
	void InteractableThreeDots()
	{
		if (maxLvl == 0) { lvlSelectScalers[0].ScaleUp(); }
		for (int i = 0; i < maxLvl && i < lvlSelectButtons.Count; i++)
		{
			if (lvlSelectButtons[i] == lvlSelectButtons[crateScript.curntLvl - 1])
			{
				lvlSelectButtons[i].GetComponent<Button>().interactable = false;
				lvlSelectScalers[i].ScaleUp();
			}
			else 
			{
				lvlSelectButtons[i].GetComponent<Button>().interactable = true; 
				lvlSelectScalers[i].ScaleDown();
			}
		}
	}

	// Make level select buttons uninteractable between levels.
	void UninteractableThreeDots()
	{
		foreach(GameObject lvlButton in lvlSelectButtons)
		{
			if (lvlButton.activeSelf) { lvlButton.GetComponent<Button>().interactable = false; }	
		}
	}
	
	// After the initial level setup turn off the three dots fade delay.
	void TurnFadeDelayOff()
	{
		foreach(FadeInOutImage fadeImgScpt in lvlSelectFades)
		{ fadeImgScpt.fadeDelay = false; }
	}

	public void SaveSilverEggsToCorrectFile()
	{
		if (silverEggsPickedUp > GlobalVariables.globVarScript.silverEggsCount) 
		{
			GlobalVariables.globVarScript.totalEggsFound += 1;
			GlobalVariables.globVarScript.silverEggsCount = silverEggsPickedUp; 
			GlobalVariables.globVarScript.SaveEggState();
		}
	}

	public void SaveNewSilEggsFound(int newSilEggFound)
	{
		//bool alreadySaved = false;
		foreach (int silEggNumber in GlobalVariables.globVarScript.puzzSilEggsCount)
		{
			if (silEggNumber == newSilEggFound)
			{
				return;
			}
		}
		GlobalVariables.globVarScript.puzzSilEggsCount.Add(newSilEggFound);
		GlobalVariables.globVarScript.SaveEggState();
	}

	public void SaveMaxLvl()
	{
		if (maxLvl > GlobalVariables.globVarScript.puzzMaxLvl)
		{
			GlobalVariables.globVarScript.puzzMaxLvl = maxLvl;
			GlobalVariables.globVarScript.SaveEggState();
		}
	}

	void UpdateMousePos()
	{
		mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		mousePos2D = new Vector2 (mousePos.x, mousePos.y);
	}
	#endregion

	#region Coroutines
	// Move crate to the right.
	public IEnumerator MoveCrateRight ()
	{
		//Make it skip a frame to make sure that the animation has time to start.
		yield return new WaitForSeconds(0.0001f);
		//yield return new WaitUntil(!crateAnim.IsInTransition(0));
		

		while (crateAnim.transform.parent.rotation != crateInSceneTransform.rotation)
		{
			// crateAnim.transform.parent.eulerAngles = Vector3.Lerp(crateAnim.transform.parent.eulerAngles, crateInSceneTransform.eulerAngles, Time.deltaTime);
			float Zangle = crateAnim.transform.parent.eulerAngles.z;
			Zangle = Mathf.LerpAngle(crateAnim.transform.parent.eulerAngles.z, 0f, Time.deltaTime * crateMoveSpeed);
			crateAnim.transform.parent.eulerAngles = new Vector3(0, 0, Zangle);
			//Debug.Log(crateAnim.transform.parent.eulerAngles);

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

		// foreach (Transform item in crateParent.transform) // DONT THINK ITS NEEDED 
		// {
		// 	//Debug.Log("Going through the keeds");
		// 	if(item.gameObject.CompareTag("Item"))
		// 	{
		// 		SpriteRenderer sprRen = item.GetComponent<SpriteRenderer>();
		// 		sprRen.color = new Color(sprRen.color.r, sprRen.color.g, sprRen.color.b, 0f);
		// 	}
		// }
		scaleScript.itemOnScale = null; // Deleted both scale lines if we want scale arrow to reset after silver eggs have been clicked. 
		scaleScript.isAnItemOnScale = false; //

		crateParent.transform.parent.position = crateTopTransform.position;
		crateParent.transform.parent.rotation = crateTopTransform.rotation;

		lvlSilverEggs[crateScript.curntLvl - 1].SetActive(true);
		resetItemsButtonScript.EndOfLevelReset();
		itemHolder.SetActive(false);
		scrnDarkImgScript.FadeIn();
		crateAnim.SetTrigger("MoveDown");
		//SFX MOVE CRATE
		audioSceneMarketPuz.crateSlideDown();
		StartCoroutine(MoveCrateDown());
	}


	// Move crate down.
	public IEnumerator MoveCrateDown ()
	{
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
		foreach(GameObject silEgg in activeSilverEggs)
		{
			silEgg.GetComponent<SilverEggSequence>().StartSequence();
		}
	}


	// All silver eggs picked up, what happenes?
	public IEnumerator PuzzleComplete ()
	{
		yield return new WaitForSeconds(0.5f);

		Debug.Log("Puzzle Completed cognraturations!!!");

		yield return new WaitForSeconds(0.5f);

		audioSceneMarketPuz.StopSceneMusic();
		audioSceneMarketPuz.PlayTransitionMusic();
		
		SceneFade.SwitchScene(GlobalVariables.globVarScript.marketName);
	}
	#endregion
}