using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelComplete : MonoBehaviour 
{
	#region LevelComplete Script Variables
	[Header("Sequence")]
	public float darkenScreen; public float showCongrats, startTmpWave, /* showEggs, */ showTotalCounter, spawnEggs, showBag, /* showBagGlow, */ endLevel;
	public float birdIn;
	private bool darkenScreenStarted, showCongratsStarted, tmpWaveStarted, /* showEggsStarted, */ showTotalCounterStarted, spawnEggsStarted, showBagStarted, /* showBagGlowStarted, */ levelEnded;
	private bool birdInStarted;

	[Header("References")]
	public ClickOnEggs clickOnEggsScript;
	public SceneTapEnabler sceneTapEnabScript;
	public LevelTapMannager lvlTapManScript;
	public LevelCompleteEggSpawner levelCompleteEggSpaScript;
	public LevelCompleteEggBag levelCompleteEggbagScript;
	public LevelCompHelpBird lvlCompBirdScript;
	public GameObject titleCG;
	public TMPTextColorFade congratsColorFadeScript;
	public TMPTextWave congratsWaveScript;
	public FadeInOutImage coverFadeScript;
	public GameObject coverCanvasObject;
	//public FadeInOutSprite[] eggsFadeScripts;
	//public FadeInOutTMP totalCounterFadeScript;
	public GameObject splineWalkerGO;
	public SplineWalker splineWalkerScript;
	public ParticleSystem splineWalkerFX;
	public Button endLvlBtn;
	public AudioSceneGeneral audioSceneGenScript;
	public AudioLevelCompleteAnim audioLevelCompleteScript;
	public inputDetector inputDetScript;

	[Header ("Info")]
	private bool tapToLeave;
	private float timer;
	#endregion

	void Start () {
		if (!audioSceneGenScript) {audioSceneGenScript = GameObject.Find("Audio").GetComponent<AudioSceneGeneral>();}
		if (!audioLevelCompleteScript) {audioLevelCompleteScript = GameObject.Find("Audio").GetComponent<AudioLevelCompleteAnim>();}
		endLvlBtn.onClick.AddListener(EndLevel);
	}

	public void StartLevelCompleteSequence() {
			clickOnEggsScript.openEggPanel = false;
			clickOnEggsScript.lockDropDownPanel = false;
			// In a sequence.
			clickOnEggsScript.menuStatesScript.menuStates = MenuStatesManager.MenuStates.TurnOff;
			sceneTapEnabScript.canTapEggRidPanPuz = false;
			sceneTapEnabScript.canTapHelpBird = false;
			sceneTapEnabScript.canTapPauseBtn = false;
			sceneTapEnabScript.canTapLvlComp = true;
			StartCoroutine(LevelCompleteSequence());
	}
	
	void Update () {
		if (Input.GetKeyDown("t")) {
			StartLevelCompleteSequence();
		}
		if (tapToLeave && inputDetScript.Tapped) {
			endLvlBtn.interactable = false;
			EndLevel();
			this.enabled = false;
		}
	}
	IEnumerator LevelCompleteSequence() {
		while (timer < endLevel) {
			timer += Time.deltaTime;
			if (timer >= darkenScreen && !darkenScreenStarted) {
				coverCanvasObject.SetActive(true);
				coverFadeScript.FadeIn();
				lvlTapManScript.ZoomOutCameraReset();
				darkenScreenStarted = true;
			}
			if (timer >= birdIn && !birdInStarted) {
				lvlCompBirdScript.StartLevelCompBird();
				birdInStarted = true;
			}
			if (timer >= showBag && !showBagStarted) {
				levelCompleteEggbagScript.MakeCurrentBagAppear();
				showBagStarted = true;
			}
			if (timer >= spawnEggs && !spawnEggsStarted) {
				//levelCompleteEggSpaScript.StartAllEggSpawn();
				levelCompleteEggSpaScript.StartCoroutine(levelCompleteEggSpaScript.StartAllEggs());
				spawnEggsStarted = true;
			}
			if (timer >= showTotalCounter && !showTotalCounterStarted) {
				lvlCompBirdScript.StartCoroutine(lvlCompBirdScript.SwitchTextBubbleContent());
				showTotalCounterStarted = true;
			}
			if (timer >= startTmpWave && !tmpWaveStarted) {
				titleCG.SetActive(true);
				congratsWaveScript.waveOn = true;
				tmpWaveStarted = true;
			}
			if (timer >= showCongrats && !showCongratsStarted) {
				audioLevelCompleteScript.congratsTxtSnd();
				congratsColorFadeScript.startFadeIn = true;
				splineWalkerGO.SetActive(true);
				splineWalkerScript.isPlaying = true;
				splineWalkerFX.Play();
				showCongratsStarted = true;
			}
			if (timer >= endLevel && !levelEnded) {
				lvlCompBirdScript.StartCoroutine(lvlCompBirdScript.SetupEndLevelButton());
				tapToLeave = true;
				levelEnded = true;
				this.enabled = true;
			}
			yield return null;
		}
	}

	void TapBtnPress() {
		clickOnEggsScript.levelComplete = true;
		clickOnEggsScript.SaveLevelComplete();
		levelCompleteEggbagScript.levelsCompleted++;
		levelCompleteEggbagScript.SaveLevelsCompleted();
	}

	void EndLevel() {
		audioSceneGenScript.TransitionMenu();
		clickOnEggsScript.levelComplete = true;
		clickOnEggsScript.SaveLevelComplete();
		levelCompleteEggbagScript.levelsCompleted++;
		levelCompleteEggbagScript.SaveLevelsCompleted();
		GlobalVariables.globVarScript.toHub = true;
		GlobalVariables.globVarScript.sceneFadeScript.SwitchScene(GlobalVariables.globVarScript.menuName, true);
	}
}