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
	public bool iniSeq;
	public float iniSeqTimer;
	public float iniDelay; // waiting for titlecard to fade away
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

	[Header("Egg Panel")]
	public GameObject eggPanel;
	public List<GameObject> eggSpots;
	public List<FadeInOutSprite> eggShadowsFades;
	public List<GameObject> silverEggSpots;
	public SceneSilverEggSpawner sceneSilEggSpaScript;
	public GameObject goldenEggSpot;
	public int goldenEggFound;
	private Coroutine panelMoveCoroutine;
	public int eggsInPanel;
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
	//[HideInInspector]
	public int eggMoving;
	//[HideInInspector]
	public bool lockDropDownPanel;
	//[HideInInspector]
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
	// Birdstory 2.0
	public SceneEggMovement sceneEggMovement;

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
		yield return null;
		AdjustSilverEggCount(); // Silver egg text.
		AdjustGoldenEggCount(); // Golden egg text.
		AdjustTotalEggsFound();	// Total eggs found = to what it was last time the scene was openned. // Not sure if this is needed.
		UpdateEggsString();
		audioSceneGenScript =  GameObject.Find ("Audio").GetComponent<AudioSceneGeneral>();
		parentStartLoadOrder.goNext = true;
		iniSeq = true;
		StartCoroutine(InitialSequence());
		while (iniSeq) {
			yield return null;
		}
	}

	IEnumerator InitialSequence() {
		while (iniDelay > 0) {
			iniDelay -= Time.deltaTime;
			yield return null;
		}
		while (iniSeqTimer < allowTapF) {
			iniSeqTimer += Time.deltaTime;
			if (iniSeqTimer > checkNewSilEggsF && !iniSilEggCheckB) { 
					sceneSilEggSpaScript.SpawnNewSilverEggs();
					 iniSilEggCheckB = true; 
					 allowTapF += checkLvlCompleteF;
			}
			if (iniSeqTimer > checkLvlCompleteF && !iniLvlCompCheckB) { 
				if (totalEggsFound == eggsNeeded && !levelComplete) { 
					PlayLvlCompleteSeq(); 
				}
				iniLvlCompCheckB = true; 
			}
			if (iniSeqTimer > allowTapF) { 
				if (!levelCompleteScript.inLvlCompSeqSetup) {
					sceneTapEnabScript.canTapEggRidPanPuz = true;
					sceneTapEnabScript.canTapHelpBird = true;
					sceneTapEnabScript.canTapPauseBtn = true; 
				}
				iniSeq = false; 
			}
			yield return null;
		}
		//yield return null;
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
					sceneEggMovement.StartCoroutine(sceneEggMovement.MoveSceneEggToCorner(hit.collider.gameObject, eggSpots[eggsFound], eggsFound));
					GlobalVariables.globVarScript.eggsFoundOrder[eggs.IndexOf(hit.collider.gameObject)] = eggsFound;
					hit.collider.enabled = false;
					eggsFound++;
					EggMoving(true);
					openEggPanel = true;
					// SFX Open Panel
					if (!openEggPanel) { openEggPanel = true; audioSceneGenScript.openPanel(); }
					// SFX Click Egg
					audioSceneGenScript.ClickEggsSound(hit.collider.gameObject);
					//Play egg  click sound
					AddEggsFound();
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
						openEggPanel = false;
						lockDropDownPanel = false;
						if (panelMoveCoroutine != null) {
							StopCoroutine(panelMoveCoroutine);
						}
						panelMoveCoroutine = StartCoroutine(EggPanelInteraction(false));
						audioSceneGenScript.closePanel();
						return;
					}
					if (eggMoving <= 0)	{
						openEggPanel = true;
						lockDropDownPanel = true;
						if (panelMoveCoroutine != null) {
							StopCoroutine(panelMoveCoroutine);
						}
						panelMoveCoroutine = StartCoroutine(EggPanelInteraction(true));
						audioSceneGenScript.openPanel();
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
					EggGoToCorner eggScript = hit.collider.gameObject.GetComponent<EggGoToCorner>();
					eggScript.EggFound();
					hit.collider.enabled = false;
					sceneTapEnabScript.canTapEggRidPanPuz = true;
					sceneTapEnabScript.canTapHelpBird = true;
					sceneTapEnabScript.canTapPauseBtn = true;
					sceneTapEnabScript.canTapGoldEgg = false;
					//AdjustGoldenEggCount();
					AddEggsFound();
					eggScript.SaveEggToCorrectFile();
					// SFX Click GOLD Egg
					audioSceneGenScript.goldEggSound();
					audioSceneGenScript.goldEggShimmerStopSound();
				}
				return;
			}
		}
	}

	void Update () {
		if (seqChecker != inASequence) { //delete this later plzzz ty
			seqChecker = inASequence;
			//Debug.Log("Cmon just say inASequence is: " + seqChecker);
		}
	}

	public void EggMoving(bool addEgg) {
		if (addEgg) {
			eggMoving++;
			if (panelMoveCoroutine != null) {
				StopCoroutine(panelMoveCoroutine);
			}
			panelMoveCoroutine = StartCoroutine(EggPanelInteraction(true));
		}
		else { 
			eggMoving--; 
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
			print(distPercent);
			adjustedDur = panelMoveDuration * distPercent;
			dropDrowArrow.transform.eulerAngles = new Vector3(dropDrowArrow.transform.eulerAngles.x, dropDrowArrow.transform.eulerAngles.y, 180);
		}
		else {
			// Hide egg panel.
			targetPos = eggPanelHidden.transform.localPosition;
			distPercent = Vector3.Distance(eggPanel.transform.position, eggPanelHidden.transform.position) / Vector3.Distance(eggPanelShown.transform.position, eggPanelHidden.transform.position);
			adjustedDur = panelMoveDuration * distPercent;
			dropDrowArrow.transform.eulerAngles = new Vector3(dropDrowArrow.transform.eulerAngles.x, dropDrowArrow.transform.eulerAngles.y, 0);
		}
		while (timer < 1f) {
			// Update target pos every tick to go along with camera movement and scaling
			timer += Time.deltaTime / adjustedDur;
			eggPanel.transform.localPosition = Vector3.Lerp(curPos, targetPos, timer);
			yield return null;
		}
		panelMoveCoroutine = null;
	}

	void OnDisable () {
		inASequence = false;
		//Debug.Log("Oh no, ClickOnEggs has been disabled! Bummer.");
	}

	#region Methods

	public void UpdateEggsString() {
		totalEggsFound = /* startEggFound +  */eggsInPanel + silverEggsFound + goldenEggFound;
		
		eggCounterText.text = "Eggs Found: " + totalEggsFound + "/" + eggsNeeded;

		regEggCounterText.text = "" + (/* startEggFound +  */eggsInPanel) + "/" + totalRegEggs;
		
		silverEggCounterText.text = "" + silverEggsFound + "/6";
		
		goldenEggCounterText.text = "" + goldenEggFound + "/1";
	}

	public void AddEggsFound() {
		totalEggsFound = eggsFound + silverEggsFound + goldenEggFound;
		LevelCompleteCheck();
	}
	// - Play the level complete sequence - //
	public void LevelCompleteCheck() {
		if (totalEggsFound == eggsNeeded && !levelComplete && !iniSeq) {
			PlayLvlCompleteSeq();
		}
	}

	public void PlayLvlCompleteSeq() {
		levelCompleteScript.waitingToStartSeq = true;
	}

	#region Save & Load methods
	// --- Dependant On Scene Name --- //
	public void MakeSilverEggsAppear() { // Could be merged with AdjustSilverEggCount since they will always be called together IF we implement the egg panel in the puzzle scene 
		if (GlobalVariables.globVarScript.sceneSilEggsCount.Count > 0)
		{
			foreach(int silEggInPanel in GlobalVariables.globVarScript.sceneSilEggsCount)
			{
				silverEggsInPanel[silEggInPanel].SetActive(true);
				silEggsShadFades[silEggInPanel].FadeIn();
			}
		}
	}

	public void AdjustSilverEggCount() {
		silverEggsFound = GlobalVariables.globVarScript.sceneSilEggsCount.Count;
		UpdateEggsString();
	}

	public void AdjustGoldenEggCount() {
		if (GlobalVariables.globVarScript.riddleSolved) { goldenEggFound = 1; } else { goldenEggFound = 0; }
		UpdateEggsString();
	}

	public void AdjustTotalEggsFound() {
		totalEggsFound = GlobalVariables.globVarScript.totalEggsFound;
		UpdateEggsString();
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