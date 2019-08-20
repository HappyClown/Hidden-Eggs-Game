﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryIntro : MonoBehaviour {
	[Header("Scripts")]
	public MainMenu mainMenuScript;
	public Hub hubScript;
	public inputDetector inputDetScript;
	public FadeInOutImage blackScreenFadeScript;
	public StoryScrollingBG storyScrollBGScript;
	public StoryTimeMotions storyTimeMoScript;
	public StoryText storyTextScript;
	public StoryGustMotions storyGustScript;
	public StoryEggManager storyEggManScript;
	public StoryOneEgg storyOneEggScript;
	public StorySingleCloudManager storySingleCloudScript;
	public AudioManagerHubMenu audioManHubMenuScript;
	public AudioIntro audioIntroScript;
	[Header("Stuff")]
	public bool inStoryIntro;
	public bool testing;
	private int storyBoardTextNum;
	private bool menuFaded;
	private bool enableRaycasting;
	[Header("StoryBoard Events")]
	public List<float> onceUponATimeEvents;
	public List<float> timeFlyingEvents, gustEvents, theAccidentEvents, gustsMishapEvents, timeConfusedEvents, eggsFallingEvents, timeToTheRescueEvents, theOneEggEvents, theQuestEvents;
	private float boardTimer;
	public List<float> boardEvents = new List<float>();
	public List<bool> boardBools = new List<bool>();

	public enum IntroStates {
		TitleScreen, OnceUponATime, TimeFlying, Gust, TheAccident, GustsMishap, TimeConfused, EggsFalling, TimeToTheRescue, TheOneEgg, TheQuest
	}
	public IntroStates introStates; 

	void Start () {
		//boardEvents.Add(0f);
		//boardBools.Add(false);

		// For testing purposes, should be commented out OR set to the first IntroState.
		if (testing) {
			boardTimer = 0f;
			boardEvents.Clear();
			boardEvents = timeToTheRescueEvents;
			boardBools.Clear();
			for(int i = 0; i < timeToTheRescueEvents.Count; i++)
			{
				boardBools.Add(false);
			}
			introStates = IntroStates.TimeToTheRescue;
		}
		if (!audioIntroScript) {audioIntroScript = GameObject.Find("Audio").GetComponent<AudioIntro>();}
		if (!audioManHubMenuScript) {audioManHubMenuScript = GameObject.Find("Audio").GetComponent<AudioManagerHubMenu>();}
	}
	
	void Update () {
		//Debug.Log(introStates);
		if (inStoryIntro) {
			switch(introStates) {
				case IntroStates.TitleScreen:
					TitleScreen(); break;
				case IntroStates.OnceUponATime:
					OnceUponATime(); break;
				case IntroStates.TimeFlying:
					TimeFlying(); break;
				case IntroStates.Gust:
					Gust(); break;
				case IntroStates.TheAccident:
					TheAccident(); break;
				case IntroStates.GustsMishap:
					GustsMishap(); break;
				case IntroStates.TimeConfused:
					TimeConfused(); break;
				case IntroStates.EggsFalling:
					EggsFalling(); break;
				case IntroStates.TimeToTheRescue:
					TimeToTheRescue(); break;
				case IntroStates.TheOneEgg:
					TheOneEgg(); break;
				case IntroStates.TheQuest:
					TheQuest(); break;
			}
		}
		// intro music
		if(inStoryIntro)
			audioManHubMenuScript.audioIntro_ON =true;
		else
			audioManHubMenuScript.audioIntro_ON =false;
	}

	void TitleScreen() {
		//ResetStory();
		if (!menuFaded) {
			mainMenuScript.FadeMainMenu();
			menuFaded = true;
		}
		if (mainMenuScript.titleFade.hidden/*  && mainMenuScript.playBtnFadeScript.hidden */ && mainMenuScript.resetBtnFadeScript.hidden) {
			introStates = IntroStates.OnceUponATime;
			storyBoardTextNum = 0;
			//boardEvents.Clear();
			boardEvents = onceUponATimeEvents;
			//boardBools.Clear();
			for(int i = 0; i < onceUponATimeEvents.Count; i++)
			{
				boardBools.Add(false);
			}
		}
	}

	void OnceUponATime() {
		if (boardTimer < boardEvents[boardEvents.Count - 1]) {
			boardTimer += Time.deltaTime;
		}
		if (boardTimer >= boardEvents[0] && !boardBools[0]) {
			storyScrollBGScript.SetUpClouds(storyScrollBGScript.regularSidewaysBGs, storyScrollBGScript.regSideScrollSpeed, true, true);
			storySingleCloudScript.PlayClouds(storySingleCloudScript.xPartSys);
			boardBools[0] = true;
		}
		if (boardTimer >= boardEvents[1] && !boardBools[1]) {
			storyTextScript.SetupText(storyBoardTextNum);
			storyBoardTextNum++;
			boardBools[1] = true;
		}
		if (boardTimer >= boardEvents[2] && !boardBools[2]) {
			if (!storyTimeMoScript.timeMovesIn) {

				storyTimeMoScript.timeMovesIn = true;

				// AUDIO - TIME DIVES IN!
				audioIntroScript.introTimeHoverLoopSFX();
				Debug.Log("AUDIO: TIME FLY + HOVER Start");
			}
			boardBools[2] = true;
		}

		// Condition to change the story board.
		if (boardTimer >= boardEvents[boardEvents.Count-1] && inputDetScript.Tapped) {
			boardTimer = 0f;
			storyTextScript.ChangeTextFade(storyBoardTextNum);
			storyBoardTextNum++;
			boardEvents.Clear();
			boardEvents = timeFlyingEvents;
			boardBools.Clear();
			for(int i = 0; i < timeFlyingEvents.Count; i++)
			{
				boardBools.Add(false);
			}
			introStates = IntroStates.TimeFlying;

			// // AUDIO - BOARD CHANGE TIME HOVER SOUND SHOULD STOP!
			// audioIntroScript.STOP_introTimeHoverLoopSFX();
			// Debug.Log("AUDIO: TIME HOVER STOP once upon time");

			audioManHubMenuScript.ButtonSound(); //clicking sound
			Debug.Log("AUDIO: skip board CLICK");
		}
	}

	void TimeFlying() {
		// if (timeFlyingEvents[0] != storyTextScript.fadeCanvasScript.fadeDuration) {
		// 	timeFlyingEvents[0] = storyTextScript.fadeCanvasScript.fadeDuration;
		// }
		// boardTimer += Time.deltaTime;
		if (storyTextScript.fadeCanvasScript.shown && inputDetScript.Tapped) {
			blackScreenFadeScript.FadeIn();
			boardBools[0] = true;

			audioManHubMenuScript.ButtonSound(); //clicking sound
			Debug.Log("AUDIO: skip board CLICK");
		}
		if (blackScreenFadeScript.shown && boardBools[0]) {
			introStates = IntroStates.Gust;
			storyTimeMoScript.normalTime.SetActive(false);
			storyTextScript.TurnTextOff();
			boardTimer = 0f;
			boardEvents.Clear();
			boardEvents = gustEvents;
			boardBools.Clear();
			for(int i = 0; i < gustEvents.Count; i++)
			{
				boardBools.Add(false);
			}

			// AUDIO - BOARD CHANGE TIME HOVER SOUND SHOULD STOP!
			audioIntroScript.STOP_introTimeHoverLoopSFX();
			Debug.Log("AUDIO: TIME HOVER STOP time flying");
		}
	}

	void Gust() {
		if (blackScreenFadeScript.shown && !boardBools[0]) {
			blackScreenFadeScript.FadeOut();
			storyScrollBGScript.SetUpClouds(storyScrollBGScript.blurredSidewaysBGs, storyScrollBGScript.blurSideScrollSpeed);
			storySingleCloudScript.PlayClouds(storySingleCloudScript.xPartSys, storySingleCloudScript.gustSpeedMult, true);
		}
		// Distorted sky instead of normal.
		if (boardTimer < boardEvents[boardEvents.Count - 1]) {
			boardTimer += Time.deltaTime;
		}
		if (boardTimer >= boardEvents[0] && !boardBools[0]) {
			storyTextScript.SetupText(storyBoardTextNum);
			storyBoardTextNum++;
			boardBools[0] = true;
		}
		if (boardTimer >= boardEvents[1] && !boardBools[1]) {
			storyGustScript.SetupXMove(storyGustScript.startTrans.position.x, storyGustScript.midTrans.position.x, storyGustScript.moveInDur, storyGustScript.moveInXCurve);
			storyGustScript.yHover = true;
			boardBools[1] = true;

			// AUDIO - GUST MOVES IN!
			audioIntroScript.introGustHoverLoopSFX();
			Debug.Log("AUDIO: GUST HOVER start");
		}
		if (boardTimer >= boardEvents[2] && !boardBools[2]) {
			storyGustScript.SetupXMove(storyGustScript.midTrans.position.x, storyGustScript.endTrans.position.x, storyGustScript.moveInDur, storyGustScript.moveOutXCurve);
			// // AUDIO - GUST MOVES OUT!
			// audioIntroScript.STOP_introGustHoverLoopSFX();
			// Debug.Log("AUDIO: GUST HOVER STOP");

			boardBools[2] = true;
		}
		if (boardBools[2] && inputDetScript.Tapped) {
			blackScreenFadeScript.FadeIn();

			//AUDIo stop wind stop from Gust HOver loop
			audioIntroScript.STOP_introGustHoverLoopSFX();
			Debug.Log("AUDIO: GUST HOVER STOP");

			audioManHubMenuScript.ButtonSound(); //clicking sound
			Debug.Log("AUDIO: skip board CLICK");
		}
		if (boardBools[2] && blackScreenFadeScript.shown) {
			introStates = IntroStates.TheAccident;
			boardTimer = 0f;
			boardEvents.Clear();
			boardEvents = theAccidentEvents;
			boardBools.Clear();
			for(int i = 0; i < theAccidentEvents.Count; i++)
			{
				boardBools.Add(false);
			}
		}
	}

	void TheAccident() {
		if (blackScreenFadeScript.shown && !boardBools[0]) {
			blackScreenFadeScript.FadeOut();
			storyTextScript.TurnTextOff();
			storyTimeMoScript.normalTime.SetActive(true);
			storyTimeMoScript.SetTimeScale(true);
			storyTimeMoScript.SmallTimeHover();
			storyScrollBGScript.SetUpClouds(storyScrollBGScript.regularSidewaysBGs, storyScrollBGScript.regSideScrollSpeed);
			storySingleCloudScript.PlayClouds(storySingleCloudScript.xPartSys, storySingleCloudScript.ogSpeedMult, true);
			storyGustScript.ChangeScale();
			// Back to normal sky scrolling.



			// AUDIO - Collision sequence	
			audioIntroScript.introCollisionSFX();
			Debug.Log("AUDIO: collision");
		}
		if (boardTimer < boardEvents[boardEvents.Count - 1]) {
			boardTimer += Time.deltaTime;
		}
		if (boardTimer >= boardEvents[0] && !boardBools[0]) {
			storyTextScript.SetupText(storyBoardTextNum);
			storyBoardTextNum++;
			boardBools[0] = true;
		}
		if (boardTimer >= boardEvents[1] && !boardBools[1]) {
			storyGustScript.SetupXMove(storyGustScript.startTrans.position.x, storyGustScript.endTrans.position.x, storyGustScript.moveAcrossDur, storyGustScript.moveInXCurve);
			boardBools[1] = true;
		}
		if (boardTimer >= boardEvents[2] && !boardBools[2]) {
			storyTimeMoScript.SetupTimeSpin(storyTimeMoScript.fastSpinDuration);
			storyTimeMoScript.timeHovers = false;
			storyTimeMoScript.changeSpinTime = true;
			storyScrollBGScript.slowDownClouds = true;
			storySingleCloudScript.SlowDownCloudsSetup(storySingleCloudScript.xPartSys);
			boardBools[2] = true;

			// AUDIO - TIME SPINS!
			audioIntroScript.introTimeSpinLoopSFX();
			Debug.Log("AUDIO: time spin");
		}
		if (boardTimer >= boardEvents[3] && !boardBools[3]) {
			boardBools[3] = true;
		}
		if (boardBools[3] && inputDetScript.Tapped) {
			blackScreenFadeScript.FadeIn();

			audioManHubMenuScript.ButtonSound(); //clicking sound
			Debug.Log("AUDIO: skip board CLICK");
		}
		if (boardBools[3] && blackScreenFadeScript.shown) {
			introStates = IntroStates.GustsMishap;
			boardTimer = 0f;
			boardEvents.Clear();
			boardEvents = gustsMishapEvents;
			boardBools.Clear();
			for(int i = 0; i < gustsMishapEvents.Count; i++)
			{
				boardBools.Add(false);
			}

			// AUDIO - TIME STOP SPINNING!
			audioIntroScript.STOP_introTimeSpinLoopSFX();
			Debug.Log("AUDIO: time stop spin");

			//AUDIO PLAY GUST HOVER?
			audioIntroScript.introGustMishapLoopSFX();
			Debug.Log("AUDIO: gust mishap");
		}
	}

	void GustsMishap() {
		if (blackScreenFadeScript.shown && !boardBools[0]/*  || Input.GetKeyDown("space") */) {
			blackScreenFadeScript.FadeOut();
			storyTextScript.TurnTextOff();
			storyTimeMoScript.currentTime.SetActive(false);
			storyScrollBGScript.slowDownClouds = false;
			//storyScrollBGScript.SetCloudSpeed(storyScrollBGScript.regSideScrollSpeed);
			storyScrollBGScript.SetUpClouds(storyScrollBGScript.blurredSidewaysBGs, storyScrollBGScript.blurSideScrollSpeed);
			storySingleCloudScript.PlayClouds(storySingleCloudScript.xPartSys, storySingleCloudScript.gustSpeedMult, true);
			storyGustScript.ChangeEyePos();
			storyGustScript.ChangeScale();
			// Back to the distorted sky.
			// boardTimer = 0f;
			// boardEvents.Clear();
			// boardEvents = gustsMishapEvents;
			// boardBools.Clear();
			// for(int i = 0; i < gustsMishapEvents.Count; i++)
			// {
			// 	boardBools.Add(false);
			// }
			// storyGustScript.yHover = true;
		}
		if (boardTimer < boardEvents[boardEvents.Count - 1]) {
			boardTimer += Time.deltaTime;
		}
		if (boardTimer >= boardEvents[0] && !boardBools[0]) {
			storyTextScript.SetupText(storyBoardTextNum);
			storyBoardTextNum++;
			boardBools[0] = true;
		}
		if (boardTimer >= boardEvents[1] && !boardBools[1]) {
			storyGustScript.SetupXMove(storyGustScript.startTrans.position.x, storyGustScript.midTrans.position.x, storyGustScript.moveInDur, storyGustScript.moveInXCurve);
			boardBools[1] = true;
		}
		if (boardTimer >= boardEvents[2] && !boardBools[2]) {
			storyGustScript.ChangeEyePos();
			storyGustScript.SetupXMove(storyGustScript.midTrans.position.x, storyGustScript.topEndTrans.position.x, storyGustScript.moveAcrossDur, storyGustScript.moveOutXCurve);
			storyGustScript.SetupYMove(storyGustScript.gust.transform.localPosition.y, storyGustScript.topEndTrans.position.y, storyGustScript.moveAcrossDur, storyGustScript.moveOutTopYCurve);
			storyGustScript.SetupScaleDown(storyGustScript.gust.transform.localScale.x, storyGustScript.moveOutScale, storyGustScript.moveAcrossDur, storyGustScript.moveOutTopYCurve);
			storyGustScript.gustFadeScript.FadeOut();
			boardBools[2] = true;
		}
		if (boardBools[2] && inputDetScript.Tapped) {
			blackScreenFadeScript.FadeIn();

			audioManHubMenuScript.ButtonSound(); //clicking sound
			Debug.Log("AUDIO: skip board CLICK");

		}
		if (boardBools[2] && blackScreenFadeScript.shown) {
			introStates = IntroStates.TimeConfused;
			boardTimer = 0f;
			boardEvents.Clear();
			boardEvents = timeConfusedEvents;
			boardBools.Clear();
			for(int i = 0; i < timeConfusedEvents.Count; i++)
			{
				boardBools.Add(false);
			}

			// AUDIO - TIME SPINS!
			audioIntroScript.introTimeSpinLoopSFX();
			Debug.Log("AUDIO: time spin");

			//AUDIO PLAY GUST HOVER?
			audioIntroScript.STOP_introGusMishapLoopSFX();
			Debug.Log("AUDIO: STOP gust mishap");

		}
	}

	void TimeConfused() {
		if (blackScreenFadeScript.shown && !boardBools[0]/*  || Input.GetKeyDown("space") */) {
			//
			// boardTimer = 0f;
			// boardEvents.Clear();
			// boardEvents = timeConfusedEvents;
			// boardBools.Clear();
			// for(int i = 0; i < timeConfusedEvents.Count; i++)
			// {
			// 	boardBools.Add(false);
			// }
			//
			blackScreenFadeScript.FadeOut();
			storyTextScript.TurnTextOff();
			storyTimeMoScript.ChangeCurrentTime(storyTimeMoScript.bewilderedTime);
			storyTimeMoScript.SetTimeScale(false);
			storyTimeMoScript.timeHovers = false;
			storyTimeMoScript.SetupTimeSpin(storyTimeMoScript.slowSpinDuration);
			storyTimeMoScript.SetTimePos(storyTimeMoScript.bewilderedMidTrans.position, false);
			storyScrollBGScript.SetUpClouds(storyScrollBGScript.regularSidewaysBGs, 0f);
			//storyScrollBGScript.SetCloudSpeed(0f);
			storySingleCloudScript.StopActivePartSys();
			//storySingleCloudScript.PlayClouds(storySingleCloudScript.xPartSys, storySingleCloudScript.ogSpeedMult, true);
		}
		if (boardTimer < boardEvents[boardEvents.Count - 1]) {
			boardTimer += Time.deltaTime;
		}
		if (boardTimer >= boardEvents[0] && !boardBools[0]) {
			storyTextScript.SetupText(storyBoardTextNum);
			storyBoardTextNum++;
			boardBools[0] = true;
		}
		if (boardTimer >= boardEvents[1] && !boardBools[1]) {
			storyEggManScript.spawnBagEggs = true;
			boardBools[1] = true;
		}
		if (boardBools[1] && inputDetScript.Tapped) {
			blackScreenFadeScript.FadeIn();	

			audioManHubMenuScript.ButtonSound(); //clicking sound
			Debug.Log("AUDIO: skip board CLICK");
		}
		if (boardBools[1] && blackScreenFadeScript.shown) {
			storyEggManScript.ResetEggs();
			introStates = IntroStates.EggsFalling;
			boardTimer = 0f;
			boardEvents.Clear();
			boardEvents = eggsFallingEvents;
			boardBools.Clear();
			for(int i = 0; i < eggsFallingEvents.Count; i++)
			{
				boardBools.Add(false);
			}
					
			// AUDIO - TIME STOP SPINNING!
			audioIntroScript.STOP_introTimeSpinLoopSFX();
			Debug.Log("AUDIO: time stop spin");
		}
	}

	void EggsFalling() {
		if (blackScreenFadeScript.shown && !boardBools[0]) {
			//
			// boardTimer = 0f;
			// boardEvents.Clear();
			// boardEvents = eggsFallingEvents;
			// boardBools.Clear();
			// for(int i = 0; i < eggsFallingEvents.Count; i++)
			// {
			// 	boardBools.Add(false);
			// }
			//
			blackScreenFadeScript.FadeOut();
			storyTextScript.TurnTextOff();
			storyEggManScript.spawnBagEggs = false;
			storyTimeMoScript.ChangeCurrentTime(null);
			storyTimeMoScript.timeSpins = false;
			storyScrollBGScript.SetUpClouds(storyScrollBGScript.verticalBGs, storyScrollBGScript.verticalScrollSpeed, false);
			storySingleCloudScript.PlayClouds(storySingleCloudScript.yPartSys, storySingleCloudScript.vertSpeedMult, true);
		}
		if (boardTimer < boardEvents[boardEvents.Count - 1]) {
			boardTimer += Time.deltaTime;
		}
		if (boardTimer >= boardEvents[0] && !boardBools[0]) {
			storyScrollBGScript.SetUpClouds(storyScrollBGScript.verticalBGs, storyScrollBGScript.verticalScrollSpeed, false); // Take out when not testing
			storyTextScript.SetupText(storyBoardTextNum);
			storyBoardTextNum++;
			boardBools[0] = true;
		}
		if (boardTimer >= boardEvents[1] && !boardBools[1]) {
			storyEggManScript.SpawnFallingEggs();
			boardBools[1] = true;
		}
		if (boardBools[1] && inputDetScript.Tapped) {
			blackScreenFadeScript.FadeIn();	

			audioManHubMenuScript.ButtonSound(); //clicking sound
			Debug.Log("AUDIO: skip board CLICK");
		}
		if (boardBools[1] && blackScreenFadeScript.shown) {
			storyEggManScript.ResetEggs();
			introStates = IntroStates.TimeToTheRescue;
			boardTimer = 0f;
			boardEvents.Clear();
			boardEvents = timeToTheRescueEvents;
			boardBools.Clear();
			for(int i = 0; i < timeToTheRescueEvents.Count; i++)
			{
				boardBools.Add(false);
			}


			//AUDIO TIME DIVE?
			audioIntroScript.introTimeDiveSFX();
			Debug.Log("AUDIO: time DIve");
		}
	}

	void TimeToTheRescue() {
		if (blackScreenFadeScript.shown && !boardBools[0]) {
			blackScreenFadeScript.FadeOut();
			storyTextScript.TurnTextOff();
			storyTimeMoScript.ChangeCurrentTime(storyTimeMoScript.divingTime);
		}
		if (boardTimer < boardEvents[boardEvents.Count - 1]) {
			boardTimer += Time.deltaTime;
		}
		if (boardTimer >= boardEvents[0] && !boardBools[0]) {
			storyTextScript.SetupText(storyBoardTextNum);
			storyBoardTextNum++;
			boardBools[0] = true;
		}
		if (boardTimer >= boardEvents[1] && !boardBools[1]) {
			storyTimeMoScript.timeDives = true;
			//storyTimeMoScript.SetupDiveHover();
			//storyTimeMoScript.diveHover = true;
			boardBools[1] = true;
		}
		if (boardTimer >= boardEvents[2] && !boardBools[2]) {
			// Will have to put a delay somehwere else.
			boardBools[2] = true;
		}
		if (boardBools[2] && inputDetScript.Tapped) {
			blackScreenFadeScript.FadeIn();	

			audioManHubMenuScript.ButtonSound(); //clicking sound
			Debug.Log("AUDIO: skip board CLICK");
		}
		if (boardBools[2] && blackScreenFadeScript.shown) {
			introStates = IntroStates.TheOneEgg;
			boardTimer = 0f;
			boardEvents.Clear();
			boardEvents = theOneEggEvents;
			boardBools.Clear();
			for(int i = 0; i < theOneEggEvents.Count; i++)
			{
				boardBools.Add(false);
			}
			//AUDIO SINGLE EGG SPIN?
			audioIntroScript.introSingleEggSpinLoopSFX();
			Debug.Log("AUDIO: single egg spin loop");
		}
	}

	void TheOneEgg() {
		if (blackScreenFadeScript.shown && !boardBools[0]) {
			blackScreenFadeScript.FadeOut();
			storyTextScript.TurnTextOff();
			storyTimeMoScript.SetTimePos(storyTimeMoScript.diveStartTrans.position, true);
			storyOneEggScript.theOneEgg.SetActive(true);
			storyOneEggScript.eggTrailFX.Play();
			//storyOneEggScript.rotate = true;
		}
		if (boardTimer < boardEvents[boardEvents.Count - 1]) {
			boardTimer += Time.deltaTime;
		}
		if (boardTimer >= boardEvents[0] && !boardBools[0]) {
			storyTextScript.SetupText(storyBoardTextNum);
			storyBoardTextNum++;
			boardBools[0] = true;
		}
		if (boardTimer >= boardEvents[1] && !boardBools[1]) {
			//storyTimeMoScript.timeDivesThrough = true;
			storyOneEggScript.tapIconFadeScript.FadeIn();
			storyOneEggScript.scaleTapIcon = true;
			enableRaycasting = true;
			boardBools[1] = true;
		}
		// if (boardTimer >= boardEvents[2] && !boardBools[2]) {
		// 	//storyOneEggScript.theOneEgg.SetActive(false);
		// 	//storyOneEggScript.rotate = false;
		// 	boardBools[2] = true;
		// }
		if (enableRaycasting && inputDetScript.Tapped && !boardBools[2]) {
			Vector3 mousePos = Camera.main.ScreenToWorldPoint(inputDetScript.TapPosition);
			Vector2 mousePos2D = new Vector2 (mousePos.x, mousePos.y);
			RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector3.forward, 50f);
			if (hit && hit.collider.tag == "Egg") {
				// TheOneEgg is set inactive in the StoryTimeMotion script.
				storyOneEggScript.EggTap();

				//AUDIO SINGLE EGG SPIN?
				audioIntroScript.STOP_introSingleEggSpinLoopSFX();
				Debug.Log("AUDIO: STOP single egg spin loop");

				// AUDIO - EGG CLICKED/TAPPED!
				audioManHubMenuScript.ButtonSound(); //clicking sound
				Debug.Log("AUDIO: skip board CLICK EGG SOUND");
				//button sound ftm, will change the sound

				storyOneEggScript.tapIconFadeScript.FadeOut();
				storyTimeMoScript.timeDivesThrough = true;	
				
				//AUDIO TIME DIVE?
				audioIntroScript.introTimeDiveSFX();
				Debug.Log("AUDIO: time DIve again");

				enableRaycasting = false;
				boardBools[2] = true;
				return;



			}
		}
		if (boardBools[2] && inputDetScript.Tapped) {
			blackScreenFadeScript.FadeIn();

			audioManHubMenuScript.ButtonSound(); //clicking sound
			Debug.Log("AUDIO: skip board CLICK");
		}
		if (boardBools[2] && blackScreenFadeScript.shown) {
			introStates = IntroStates.TheQuest;
			boardTimer = 0f;
			boardEvents.Clear();
			boardEvents = theQuestEvents;
			boardBools.Clear();
			for(int i = 0; i < theQuestEvents.Count; i++)
			{
				boardBools.Add(false);
			}
		}
	}

	void TheQuest() {
		if (blackScreenFadeScript.shown && !boardBools[0]) {
			blackScreenFadeScript.FadeOut();
			storyTextScript.TurnTextOff();
			storyTimeMoScript.ChangeCurrentTime(storyTimeMoScript.glidingTime);
			// turn off the scrolling clouds
			storyScrollBGScript.TurnOffScrollClouds();
			storySingleCloudScript.StopActivePartSys();
			// mainMenuScript.ToHub(); Without the hubScript.startHubActive = true; so that it fades out the main menu but only shows the grey village
			storyOneEggScript.behindTheOneEgg.SetActive(true);
			mainMenuScript.ToHub(false);
			storyOneEggScript.eggTrailFX.Stop();
			// Make sure the summer dissolve is reset.
			hubScript.dissolveMats[0].SetFloat ("_Threshold", 0f);
		}
		if (boardTimer < boardEvents[boardEvents.Count - 1]) {
			boardTimer += Time.deltaTime;
		}
		if (boardTimer >= boardEvents[0] && !boardBools[0]) {
			storyTextScript.SetupText(storyBoardTextNum);
			storyBoardTextNum++;
			boardBools[0] = true;
		}
		if (boardTimer >= boardEvents[1] && !boardBools[1]) {
			// TimeTopView flies in from the bottom right
			storyTimeMoScript.timeGlides = true;
			boardBools[1] = true;

			//AUDIO TIME APPEARS ON MAP sequence?
			audioIntroScript.introTimeAppearMapSFX();
			Debug.Log("AUDIO: time appears map");
		}
		if (boardTimer >= boardEvents[2] && !boardBools[2]) {
			// TheOneEgg flies from under time to the middle scaling to regular scene egg size
			storyOneEggScript.SetupFlyOutOfTime();
			boardBools[2] = true;
		}
		if (boardTimer >= boardEvents[3] && !boardBools[3]) {
			// It shakes and sparkles

		}
		if (boardTimer >= boardEvents[4] && !boardBools[4] && inputDetScript.Tapped) {
			// Tap (anywhere or on it)
			// Text fades out, time fades out,
			storyTextScript.FadeOutText();
			storyTimeMoScript.FadeOutGlidingTime();
			storyOneEggScript.theOneEggFadeScript.FadeOut();
			// Regular hub gets activated
			hubScript.startHubActive = true;
			boardBools[4] = true;

			audioManHubMenuScript.ButtonSound(); //clicking sound
			Debug.Log("AUDIO: skip board CLICK");
		}
		if (boardBools[4]) {
			introStates = IntroStates.TitleScreen;
			boardTimer = 0f;
			boardEvents.Clear();
			boardEvents = onceUponATimeEvents;
			boardBools.Clear();
			for(int i = 0; i < onceUponATimeEvents.Count; i++)
			{
				boardBools.Add(false);
			}
			inStoryIntro = false;
			mainMenuScript.fullIntro = false;
		}
	}
	
	void ResetStory() {
		menuFaded = false;
		storyTimeMoScript.ResetNormalTime();
		storyGustScript.gust.transform.position = storyGustScript.startTrans.position;
		storyGustScript.gustFadeScript.FadeIn();
		
	}
}
