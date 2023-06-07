using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class ClickOnEggs : MonoBehaviour {
	RaycastHit2D hit;
	Vector2 mousePos2D;
	Vector3 mousePos;
	public static bool inASequence; // Currently in a sequence; BirdIntro, BirdDialog?, PuzzleUnlock, SilverEggsToPanel, GoldenEgg, LevelComplete 
	public bool seqChecker; // to delete

	[Header("Initial Sequence")]
	public bool iniDelayOffForTesting;
	public bool iniSeq;
	public float iniSeqTimer;
	public float iniDelay; // Waiting for the titlecard to fade away.
	public float checkNewSilEggsF;
	public float checkLvlCompleteF, allowTapF;
	public bool iniSilEggCheckB, iniLvlCompCheckB;

	[Header("Egg Info")]
	public int eggsFound;
	[Tooltip("Total amount of regular eggs in the scene.")]
	public int totalRegEggs;
	public TextMeshProUGUI eggCounterText, regEggCounterText, silverEggCounterText, goldenEggCounterText;

	[Header("Picked Up Eggs")]
	public Vector3 newCornerPos;
	public float cornerExtraPos;
	public Transform cornerPos;

	[Header("Puzzle")]
	public PuzzleUnlock puzzUnlockScript;
	public string puzzleSceneName;
	public GameObject puzzleConfirmationUI;
	[Header("Golden Egg")]
	public GameObject goldenEggSpot;
	public GameObject goldenEggGO;
	public GameObject smallGoldenEggForPanel;
	public int goldenEggFound;
	public Vector3 goldenEggCornerScale;

	[Header("Egg Panel")]
	public GameObject eggPanel;
	public List<GameObject> eggSpots;
	public List<FadeInOutSprite> eggShadowsFades;
	public List<GameObject> silverEggSpots;
	public SceneSilverEggSpawner sceneSilEggSpaScript;
	private Coroutine panelMoveCoroutine;
	public int regularEggsFound;
	public int startEggFound;
	public GameObject eggPanelHidden;
	public GameObject eggPanelShown;
	public float panelMoveSpeed;
	public float panelMoveDuration;
	public float basePanelOpenTime;
	public List<GameObject> silverEggsInPanel;
	public List<FadeInOutSprite> silEggsShadFades;
	public GameObject dropDrowArrow;
	public List<GameObject> eggs;
	private float timer;
	public int eggMoving;
	public bool lockDropDownPanel;
	public bool openEggPanel;

	[Header("Level Complete")]
	public int totalEggsFound;
	public int eggsNeeded;
	public int silverEggsFound;
	public bool levelComplete;
	public LevelComplete levelCompleteScript;

	[Header("Script References")]
	public inputDetector myInputDetector;
	public SceneTapEnabler sceneTapEnabScript;
	public SceneFade sceneFadeScript;
	public AudioSceneGeneral audioSceneGenScript;
	public MenuStatesManager menuStatesScript;
	public SceneEggMovement sceneEggMovement;
	public EggsSaveLoad eggsSaveLoad;
	private ParentStartLoadOrder parentStartLoadOrder;


	void ChildObjectStart(ParentStartLoadOrder parentStartScript) {
		parentStartLoadOrder = parentStartScript;
		StartCoroutine(StartObjectActivation());
	}
	IEnumerator StartObjectActivation() {
		if (sceneFadeScript == null) { sceneFadeScript = GlobalVariables.globVarScript.GetComponent<SceneFade>(); }
		newCornerPos = cornerPos.position;
		if (iniDelay < sceneFadeScript.fadeTime) { iniDelay = sceneFadeScript.fadeTime; }
		AdjustLevelComplete(); // Check if level has already been completed. (bool)
		yield return null;
		MakeSilverEggsAppear(); // Make the silver eggs already found appear in the egg panel.
		MakeGoldenEggAppear(); // Make the golden egg appear if the riddle is solved.
		yield return null;
		if (!audioSceneGenScript) audioSceneGenScript = GameObject.Find("Audio").GetComponent<AudioSceneGeneral>();
		parentStartLoadOrder.goNext = true;
		iniSeq = true;
		StartCoroutine(InitialSequence());
		while (iniSeq) {
			yield return null;
		}
	}

	IEnumerator InitialSequence() {
		if (!iniDelayOffForTesting) {
			while (iniDelay > 0) {
				iniDelay -= Time.deltaTime;
				yield return null;
			}
		}
		puzzUnlockScript.LoadPuzzleIntro();
		eggsSaveLoad.SetEggStates();
		panelMoveCoroutine = StartCoroutine(EggPanelInteraction(false));
		while (iniSeqTimer < allowTapF) {
			iniSeqTimer += Time.deltaTime;
			if (iniSeqTimer > checkNewSilEggsF && !iniSilEggCheckB) { 
					sceneSilEggSpaScript.NewSilverEggsCheck();
					iniSilEggCheckB = true;
					allowTapF += checkLvlCompleteF;
			}
			// if (iniSeqTimer > checkLvlCompleteF && !iniLvlCompCheckB) { 
			// 	if (totalEggsFound == eggsNeeded && !levelComplete) { 
			// 		PlayLvlCompleteSeq(); 
			// 	}
			// 	iniLvlCompCheckB = true; 
			// }
			if (iniSeqTimer > allowTapF) { 
				//if (!levelCompleteScript.inLvlCompSeqSetup) {
					sceneTapEnabScript.canTapEggRidPanPuz = true;
					sceneTapEnabScript.canTapHelpBird = true;
					sceneTapEnabScript.canTapPauseBtn = true; 
				//}
				iniSeq = false; 
			}
			yield return null;
		}
		AddEggsFound();
	}

	public void TapChecks() {
		// -- ON CLICK/TAP -- //
		mousePos = Camera.main.ScreenToWorldPoint(myInputDetector.TapPosition);
		mousePos2D = new Vector2 (mousePos.x, mousePos.y);
		hit = Physics2D.Raycast(mousePos2D, Vector3.forward, 50f);
		if (hit) {
			if (sceneTapEnabScript.canTapEggRidPanPuz) { // On regular eggs, puzzle, eggPanel
				// - Egg Tapped - //
				if (hit.collider.CompareTag("Egg")) {
					myInputDetector.cancelDoubleTap = true;
					// Starts a movement coroutine for the egg found.
					GameObject eggGO = hit.collider.gameObject;
					sceneEggMovement.StartCoroutine(sceneEggMovement.MoveSceneEggToCorner(eggGO, eggSpots[eggsFound], eggsFound));
					hit.collider.enabled = false;
					eggsFound++;
					//openEggPanel = true;
					// SFX Open Panel
					//if (!openEggPanel) { openEggPanel = true; audioSceneGenScript.openPanel(); }
					// SFX Click Egg
					audioSceneGenScript.ClickEggsSound(eggGO);
					//Play egg  click sound
					AddEggsFound();
					int eggIndex = eggs.IndexOf(eggGO);
					eggsSaveLoad.SaveEgg(eggIndex);
					return;
				}
				// - Go To Puzzle Scene - //
				if (hit.collider.CompareTag("Puzzle")) {
					if(GlobalVariables.globVarScript.puzzIntroDone){
						menuStatesScript.puzzleConfActive = true;
						menuStatesScript.puzzleConfStates = MenuStatesManager.MenuStates.TurnOn;
						
					}else{
						LoadPuzzle();
					}
					return;
				}
				// - Opening Egg Panel Manually - //
				if (hit.collider.CompareTag("EggPanel")) {
					if (lockDropDownPanel) {
						lockDropDownPanel = false;
						if (eggMoving > 0) {
							return;
						}
						//openEggPanel = false;
						if (panelMoveCoroutine != null) {
							StopCoroutine(panelMoveCoroutine);
						}
						panelMoveCoroutine = StartCoroutine(EggPanelInteraction(false));
						//audioSceneGenScript.closePanel();
						return;
					}
					if (eggMoving <= 0)	{
						//openEggPanel = true;
						lockDropDownPanel = true;
						if (panelMoveCoroutine != null) {
							StopCoroutine(panelMoveCoroutine);
						}
						panelMoveCoroutine = StartCoroutine(EggPanelInteraction(true));
						//audioSceneGenScript.openPanel();
					}
					if (eggMoving > 0) {
						lockDropDownPanel = true;
					}
					return;
				}
			}
			if (sceneTapEnabScript.canTapGoldEgg) {
				// - Golden Egg Tapped - //
				if ((hit.collider.CompareTag("GoldenEgg"))) {
					hit.collider.enabled = false;
					sceneTapEnabScript.canTapEggRidPanPuz = true;
					sceneTapEnabScript.canTapHelpBird = true;
					sceneTapEnabScript.canTapPauseBtn = true;
					sceneTapEnabScript.canTapGoldEgg = false;
					AddEggsFound();
					sceneEggMovement.StartCoroutine(sceneEggMovement.MoveSceneEggToCorner(goldenEggGO, goldenEggSpot, 31, goldenEggCornerScale, false, false, true));
					audioSceneGenScript.goldEggSound();
					audioSceneGenScript.goldEggShimmerStopSound();
				}
				return;
			}
		}
	}

	public void EggMoving(bool startOfMovement, bool regEgg = true, bool silEgg = false, bool golEgg = false) {
		if (startOfMovement) {
			eggMoving++;
			if (panelMoveCoroutine != null) {
				StopCoroutine(panelMoveCoroutine);
			}
			panelMoveCoroutine = StartCoroutine(EggPanelInteraction(true));
		}
		else { 
			eggMoving--;
			// Check what kind of egg just arrived in the panel, if any.
			if (regEgg) { 
				regularEggsFound++;
				puzzUnlockScript.PuzzleUnlockCheck(regularEggsFound);
				UpdateEggsString();
			}
			else if (silEgg) {
				silverEggsFound++;
				UpdateEggsString(false, true);
			}
			else if (golEgg) {
				goldenEggFound++;
				goldenEggGO.SetActive(false);
				smallGoldenEggForPanel.SetActive(true);
				UpdateEggsString(false, false, true);
			}
			// Close the panel if there are no more eggs moving (eggMoving is also used to open the panel even when no actual eggs are moving).
			if (eggMoving <= 0 && !lockDropDownPanel) {
				eggMoving = 0;
				if (panelMoveCoroutine != null) {
					StopCoroutine(panelMoveCoroutine);
				}
				panelMoveCoroutine = StartCoroutine(EggPanelInteraction(false));
			}
		}
	}

	IEnumerator EggPanelInteraction (bool open) {
		float timer = 0f;
		float distPercent = 0f;
		float adjustedDur = 0f;
		Vector3 curPos = eggPanel.transform.localPosition;
		Vector3 targetPos;
		if (open) {
			// Show egg panel.
			targetPos = eggPanelShown.transform.localPosition;
			distPercent = Vector2.Distance(eggPanel.transform.position, eggPanelShown.transform.position) / Vector2.Distance(eggPanelShown.transform.position, eggPanelHidden.transform.position);
			//print(distPercent);
			adjustedDur = panelMoveDuration * distPercent;
			dropDrowArrow.transform.eulerAngles = new Vector3(dropDrowArrow.transform.eulerAngles.x, dropDrowArrow.transform.eulerAngles.y, 180);
			audioSceneGenScript.openPanel();
		}
		else {
			// Hide egg panel.
			targetPos = eggPanelHidden.transform.localPosition;
			distPercent = Vector3.Distance(eggPanel.transform.position, eggPanelHidden.transform.position) / Vector3.Distance(eggPanelShown.transform.position, eggPanelHidden.transform.position);
			adjustedDur = panelMoveDuration * distPercent;
			dropDrowArrow.transform.eulerAngles = new Vector3(dropDrowArrow.transform.eulerAngles.x, dropDrowArrow.transform.eulerAngles.y, 0);
			audioSceneGenScript.closePanel();
		}
		while (timer < 1f) {
			// Update target pos every tick to go along with camera movement and scaling
			timer += Time.deltaTime / adjustedDur;
			eggPanel.transform.localPosition = Vector3.Lerp(curPos, targetPos, timer);
			yield return null;
		}
		panelMoveCoroutine = null;
	}

	#region Methods
	public void UpdateEggsString(bool updateRegularEgg = true, bool updateSilverEgg = false, bool updateGoldenEgg = false) {
		totalEggsFound = regularEggsFound + silverEggsFound + goldenEggFound;
		
		eggCounterText.text = "Eggs Found: " + totalEggsFound + "/" + eggsNeeded;

		if (updateRegularEgg) regEggCounterText.text = "" + (regularEggsFound) + "/" + totalRegEggs;
		
		if (updateSilverEgg) silverEggCounterText.text = "" + silverEggsFound + "/6";
		
		if (updateGoldenEgg) goldenEggCounterText.text = "" + goldenEggFound + "/1";
	}

	public void AddEggsFound() {
		totalEggsFound = eggsFound + silverEggsFound + goldenEggFound;
		LevelCompleteCheck();
	}
	// - Play the level complete sequence - //
	public void LevelCompleteCheck() {
		if (totalEggsFound >= eggsNeeded && !levelComplete && !iniSeq) {
			PlayLvlCompleteSeq();
		}
	}
	public void PlayLvlCompleteSeq() {
		QueueSequenceManager.AddSequenceToQueue(levelCompleteScript.StartLevelCompleteSequence);
	}

	#region Save & Load methods
	// --- Dependant On Scene Name --- //
	void MakeSilverEggsAppear() { // Could be merged with AdjustSilverEggCount since they will always be called together IF we implement the egg panel in the puzzle scene 
		if (GlobalVariables.globVarScript.sceneSilEggsCount.Count > 0)
		{
			foreach(int silEggInPanel in GlobalVariables.globVarScript.sceneSilEggsCount)
			{
				silverEggsInPanel[silEggInPanel].SetActive(true);
				silEggsShadFades[silEggInPanel].FadeIn();
			}
			silverEggsFound = GlobalVariables.globVarScript.sceneSilEggsCount.Count;
			UpdateEggsString(false, true);
		}
	}
	// If the riddle has previously been solved, make the golden egg appear in the gg panel.
	void MakeGoldenEggAppear() {
		if (GlobalVariables.globVarScript.riddleSolved) {
			goldenEggFound = 1;
			smallGoldenEggForPanel.SetActive(true);
			UpdateEggsString(false, false, true);
		}
	}

	public void SaveLevelComplete() {
		GlobalVariables.globVarScript.levelComplete = levelComplete;
		GlobalVariables.globVarScript.SaveEggState();
	}

	public void AdjustLevelComplete() {
		levelComplete = GlobalVariables.globVarScript.levelComplete;
	}
	public void LoadPuzzle(){
		GlobalVariables.globVarScript.sceneFadeScript.SwitchScene(puzzleSceneName);
		PlayerPrefs.SetString ("LastLoadedScene", SceneManager.GetActiveScene().name);
		//SFX puzz btn
		audioSceneGenScript.TransitionPuzzle();
		audioSceneGenScript.puzzleAnimationStop();
	}
	#endregion
	#endregion
}