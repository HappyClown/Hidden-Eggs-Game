﻿using System.Collections;
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
	public TMPTextColorFade congratsColorFadeScript;
	public TMPTextWave congratsWaveScript;
	public FadeInOutImage coverFadeScript;
	public FadeInOutSprite[] eggsFadeScripts;
	public FadeInOutSprite lineFadeScript;
	public FadeInOutTMP totalCounterFadeScript;
	public SplineWalker splineWalkerScript;
	public ParticleSystem splineWalkerFX;
	public Button endLvlBtn;
	//public Button tapBtn;
	public AudioSceneGeneral audioSceneGenScript;

	[Header ("Info")]
	public bool waitingToStartSeq;
	public bool inLvlCompSeqSetup;
	private bool inLvlCompSeqEnd;
	private float timer;
	#endregion

	void Start () {
		//tapBtn.onClick.AddListener(TapBtnPress);
		if (!audioSceneGenScript) {
			audioSceneGenScript = GameObject.Find("Audio").GetComponent<AudioSceneGeneral>();
		}
		endLvlBtn.onClick.AddListener(EndLevel);
	}
	
	void Update () {
		if (waitingToStartSeq && !ClickOnEggs.inASequence && clickOnEggsScript.eggMoving <= 0) {
			clickOnEggsScript.openEggPanel = false;
			clickOnEggsScript.lockDropDownPanel = false;
			inLvlCompSeqSetup = true;
			// In a sequence.
			ClickOnEggs.inASequence = true;
			waitingToStartSeq = false;
			//endLvlBtn.interactable = false;
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
			if (timer >= birdIn && !birdInStarted) {
				lvlCompBirdScript.moveUp = true;
				birdInStarted = true;
			}
			if (timer > showCongrats && !showCongratsStarted) {
				// AUDIO - CONGRATS TITLE STARTS APPEARING!
				congratsColorFadeScript.startFadeIn = true;
				splineWalkerScript.isPlaying = true;
				splineWalkerFX.Play();
				showCongratsStarted = true;
			}
			if (timer > startTmpWave && !tmpWaveStarted) {
				congratsWaveScript.waveOn = true;
				tmpWaveStarted = true;
			}
			// if (timer > showEggs && !showEggsStarted) { 
			// 	if (eggsFadeScripts.Length > 0) {
			// 		foreach (FadeInOutSprite eggFadeScript in eggsFadeScripts)
			// 		{
			// 			eggFadeScript.FadeIn();
			// 		}
			// 	}
			// 	showEggsStarted = true;
			// }
			if (timer > showTotalCounter && !showTotalCounterStarted) {
				// lineFadeScript.FadeIn();
				// totalCounterFadeScript.FadeIn();
				lvlCompBirdScript.waitForConTxtOut = true;
				showTotalCounterStarted = true;
			}
			if (timer > spawnEggs && !spawnEggsStarted) {
				levelCompleteEggSpaScript.StartAllEggSpawn();
				spawnEggsStarted = true;
			}
			if (timer > showBag && !showBagStarted) {
				levelCompleteEggbagScript.MakeCurrentBagAppear();
				showBagStarted = true;
			}
			// if (timer > showBagGlow && !showBagGlowStarted) {
			// 	// levelCompleteEggbagScript.StartCurrentBagGlow();
			// 	// levelCompleteEggbagScript.bagAnim.SetTrigger("Rise");
			// 	showBagGlowStarted = true;
			// }
			if (timer > endLevel && !levelEnded) {
				lvlCompBirdScript.waitForCountOut = true;
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

	void EndLevel() {
		audioSceneGenScript.TransitionMenu();
		clickOnEggsScript.levelComplete = true;
		clickOnEggsScript.SaveLevelComplete();
		levelCompleteEggbagScript.levelsCompleted++;
		levelCompleteEggbagScript.SaveLevelsCompleted();
		GlobalVariables.globVarScript.toHub = true;
		SceneFade.SwitchSceneWhiteFade(GlobalVariables.globVarScript.menuName);
	}
}