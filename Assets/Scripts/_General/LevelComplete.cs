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
	public float darkenScreen; public float showCongrats, startTmpWave, showEggs, showTotalCounter, startEggMove, showBag, endLevel;
	private bool darkenScreenStarted, showCongratsStarted, tmpWaveStarted, showEggsStarted, showTotalCounterStarted, startEggMoveStarted, showBagStarted, levelEnded;

	[Header("References")]
	public ClickOnEggs clickOnEggsScript;
	public SceneTapEnabler sceneTapEnabScript;
	public LevelTapMannager lvlTapManScript;
	public LevelCompleteEggSpawner levelCompleteEggSpaScript;
	public LevelCompleteEggBag levelCompleteEggbagScript;
	public TMPTextColorFade congratsColorFadeScript;
	public TMPTextWave congratsWaveScript;
	public FadeInOutImage coverFadeScript;
	public FadeInOutSprite[] eggsFadeScripts;
	public FadeInOutSprite lineFadeScript;
	public FadeInOutTMP totalCounterFadeScript;
	public SplineWalker splineWalkerScript;
	public ParticleSystem splineWalkerFX;
	public Button backToHubBtn;
	public Button tapBtn;
	public AudioSceneGeneral audioSceneGenScript;

	[Header ("Info")]
	public bool waitingToStartSeq;
	public bool inLvlCompSeqSetup;
	private bool inLvlCompSeqEnd;
	private float timer;
	#endregion

	void Start () {
		tapBtn.onClick.AddListener(TapBtnPress);
		if (!audioSceneGenScript) {
			audioSceneGenScript = GameObject.Find("Audio").GetComponent<AudioSceneGeneral>();
		}
	}
	
	void Update () {
		if (waitingToStartSeq && !ClickOnEggs.inASequence && clickOnEggsScript.eggMoving <= 0) {
			clickOnEggsScript.openEggPanel = false;
			clickOnEggsScript.lockDropDownPanel = false;
			inLvlCompSeqSetup = true;
			// In a sequence.
			ClickOnEggs.inASequence = true;
			waitingToStartSeq = false;
			backToHubBtn.interactable = false;
			sceneTapEnabScript.canTapEggRidPanPuz = false;
			sceneTapEnabScript.canTapHelpBird = false;
			sceneTapEnabScript.canTapPauseBtn = false;
			sceneTapEnabScript.canTapLvlComp = true;
		}

		if (inLvlCompSeqSetup) {
			timer += Time.deltaTime;
			if (timer > darkenScreen && !darkenScreenStarted) {
				coverFadeScript.FadeIn();
				lvlTapManScript.ZoomOutCameraReset();
				darkenScreenStarted = true;
			}
			if (timer > showCongrats && !showCongratsStarted) {
				congratsColorFadeScript.startFadeIn = true;
				splineWalkerScript.isPlaying = true;
				splineWalkerFX.Play();
				showCongratsStarted = true;
			}
			if (timer > startTmpWave && !tmpWaveStarted) {
				congratsWaveScript.waveOn = true;
				tmpWaveStarted = true;
			}
			if (timer > showEggs && !showEggsStarted) { 
				foreach (FadeInOutSprite eggFadeScript in eggsFadeScripts)
				{
					eggFadeScript.FadeIn();
				}
				showEggsStarted = true;
			}
			if (timer > showTotalCounter && !showTotalCounterStarted) {
				lineFadeScript.FadeIn();
				totalCounterFadeScript.FadeIn();
				showTotalCounterStarted = true;
			}
			if (timer > startEggMove && !startEggMoveStarted) {
				levelCompleteEggSpaScript.StartEggSpawning();
				startEggMoveStarted = true;
			}
			if (timer > showBag && !showBagStarted) {
				levelCompleteEggbagScript.MakeFirstBagAppear();
				showBagStarted = true;
			}
			if (timer > endLevel && !levelEnded) {
				audioSceneGenScript.TransitionMenu();
				clickOnEggsScript.levelComplete = true;
				clickOnEggsScript.SaveLevelComplete();
				levelCompleteEggbagScript.levelsCompleted++;
				levelCompleteEggbagScript.SaveLevelsCompleted();
				GlobalVariables.globVarScript.toHub = true;
				SceneFade.SwitchSceneWhiteFade(GlobalVariables.globVarScript.menuName);
				levelEnded = true;
			}
		}
	}

	void TapBtnPress() {
		clickOnEggsScript.levelComplete = true;
		clickOnEggsScript.SaveLevelComplete();
		levelCompleteEggbagScript.levelsCompleted++;
		levelCompleteEggbagScript.SaveLevelsCompleted();
	}
}