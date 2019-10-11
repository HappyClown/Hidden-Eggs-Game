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
	private delegate void VoidDelegate();
	private VoidDelegate voidDelegate; 
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
	public bool canPlay;
	public int lvlToLoad;
	public float setupLvlWaitTime;
	public float chngLvlTimer;
	public int maxLvl;
	#endregion

	public AudioSceneParkPuzzle audioSceneParkPuzzScript;

	void Start () {
		canPlay = false;
		initialSetupOn = true;
		maxLvl = GlobalVariables.globVarScript.puzzMaxLvl;
	}
	
	void Update () {
		if (canPlay) {
			if(mySelectButton.buttonPressed) {
				lvlToLoad = mySelectButton.lvlToLoad;
				if (chngLvlTimer >= setupLvlWaitTime && curntLvl != lvlToLoad && maxLvl >= lvlToLoad){
					chngLvlTimer = 0f;
					ChangeLevelSetup();
				}
				mySelectButton.buttonPressed = false;
			}
			if (chngLvlTimer < setupLvlWaitTime) { 
				chngLvlTimer += Time.deltaTime; 
			}
			if (mySelectButton.buttonsOff) { 
				mySelectButton.buttonsOff = false; mySelectButton.InteractableThreeDots(maxLvl,curntLvl); 
			}
		}
		else {
			// When this Scene is loaded.
			if (initialSetupOn) { InitialSetup(); }
			// After the initial set up run the first sequence.
			if (iniSeqStart) {
				if (iniSeqDelay > 0) { 
					iniSeqDelay -= Time.deltaTime; 
				}
				else {
					seqTimer += Time.deltaTime;
					 if (seqTimer > itemSpawnF && !itemSpawnB) { itemSpawnB = true; LvlStuffFadeIn(); }
					 if (seqTimer > dotsSpawnF && !dotsSpawnB) { dotsSpawnB = true; mySelectButton.EnabledThreeDots(maxLvl); mySelectButton.InteractableThreeDots(maxLvl,curntLvl); }
					 if (seqTimer > iniCanPlayF) { canPlay = true; iniSeqStart = false; }
				}
			}
			if (itemsWait) {
				itemWaitTimer += Time.deltaTime;
				if (itemWaitTimer > itemWaitAmnt) {
					itemHolder.SetActive(true);
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
			// 			//SFX CLICK SILVER EGG
			// 			audioSceneParkPuzzScript.silverEgg();
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
		if (maxLvl > 3 || maxLvl < 1) { 
			curntLvl = 1; 
		}
		else { 
			curntLvl = maxLvl; 
		}
		itemHolder = lvlItemHolders[curntLvl - 1];
		initialSetupOn = false;
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
		mySilverEggMan.lvlSilverEggs[curntLvl - 1].SetActive(true); // CAN probably set it to true in the lvl finished seq or wtv
		if (mySilverEggMan.lvlSilverEggs[curntLvl - 1].transform.childCount > 0) {
			foreach (Transform silEgg in mySilverEggMan.lvlSilverEggs[curntLvl - 1].transform) {
				mySilverEggMan.activeSilverEggs.Add(silEgg.gameObject);
			}
		}
		mySilverEggMan.silverEggsActive = true;
		if (!mySelectButton.noFadeDelay) { // Turn off the initial fade delay for the three dots. Should only happen once.
			mySelectButton.TurnFadeDelayOff(); 
			mySelectButton.noFadeDelay = true; 
		}
		LvlStuffFadeOut();
		foreach(GameObject silEgg in mySilverEggMan.activeSilverEggs) {
			silEgg.GetComponent<SilverEggSequence>().StartSequence();
		}
		scrnDarkImgScript.FadeIn();
	}

	// Checks if the player tapped enough silver eggs to move on, change the current level.
	public virtual void SilverEggsCheck() {
		if (mySilverEggMan.activeSilverEggs.Count > 0) {
			if (mySilverEggMan.amntSilEggsTapped == mySilverEggMan.activeSilverEggs.Count) {			
				mySilverEggMan.activeSilverEggs.Clear();
				mySilverEggMan.silverEggsActive = false;
				mySilverEggMan.amntSilEggsTapped = 0;
				scrnDarkImgScript.FadeOut();
				curntLvl++;
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
	void RunAfter(VoidDelegate methToRun) {
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
		itemHolder.SetActive(false);
		itemHolder = lvlItemHolders[curntLvl - 1];
		itemsWait = true;
	}

	// Prepare to change level after a level selection button has been pressed.
	public void ChangeLevelSetup() {
		// Close up current level.
		canPlay = false;

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

	// public void UpdateMousePos() {
	// 	mousePos = Camera.main.ScreenToWorldPoint(myInput.TapPosition);
	// 	mousePos2D = new Vector2 (mousePos.x, mousePos.y);
	// }
	#endregion

	#region Coroutines
	// All silver eggs picked up, what happenes?
	public IEnumerator PuzzleComplete () {
		yield return new WaitForSeconds(0.5f);

		//Debug.Log("Puzzle Completed cognraturations!!!");

		yield return new WaitForSeconds(0.5f);

		audioSceneParkPuzzScript.StopSceneMusic();
		audioSceneParkPuzzScript.PlayTransitionMusic();

		GlobalVariables.globVarScript.sceneFadeScript.SwitchScene(GlobalVariables.globVarScript.parkName);
	}
	#endregion
}