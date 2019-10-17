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
	public StoryIcons storyIconsScript;
	public StorySingleCloudManager storySingleCloudScript;
	public AudioManagerHubMenu audioManHubMenuScript;
	public AudioIntro audioIntroScript;
	[Header("Stuff")]
	public bool inStoryIntro;
	public bool testing;
	private int storyBoardTextNum;
	private bool menuFaded;
	private bool enableRaycasting;
	private float tapTime;
	private bool exitingBoard;
	public bool nextBtnPressed;
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
		audioManHubMenuScript.audioIntro_ON = inStoryIntro;

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
	}
	void TitleScreen() {
		//AUDIO : intro music
		//audioManHubMenuScript.TransitionIntro();

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
			tapTime = 999999f;
		}
	}

	void OnceUponATime() {
		if (boardTimer < boardEvents[boardEvents.Count - 1] + tapTime) {
			boardTimer += Time.deltaTime;
		}
		if (boardTimer >= boardEvents[0] && !boardBools[0]) {
			storyScrollBGScript.SetUpClouds(storyScrollBGScript.regularSidewaysBGs, storyScrollBGScript.regSideScrollSpeed, true, true);
			storySingleCloudScript.PlayClouds(storySingleCloudScript.xPartSys);
			boardBools[0] = true;

			//AUDIO wind
			audioIntroScript.introWindLoopSFX();

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

			}
			boardBools[2] = true;
		}
		// SHOW TAP ICON
		if (boardTimer >= boardEvents[3] && !boardBools[3]) {
			storyIconsScript.ShowNextButton();
			boardBools[3] = true;
		}
		// TAP EVENT
		if (boardTimer >= boardEvents[4] && !boardBools[4] && nextBtnPressed || boardTimer >= boardEvents[4] && !boardBools[4] && inputDetScript.Tapped) {
			storyIconsScript.HideNextButton();
			tapTime = boardTimer;
			exitingBoard = true;
			nextBtnPressed = false;
			boardBools[4] = true;
			audioManHubMenuScript.ButtonSound(); //clicking sound
		}
		if (exitingBoard) {
			// Condition to change the story board.
			if (boardTimer >= boardEvents[boardEvents.Count-1]) {
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
				exitingBoard = false;
				tapTime = 99999f;
				introStates = IntroStates.TimeFlying;
			}
		}
	}

	void TimeFlying() {
		// if (timeFlyingEvents[0] != storyTextScript.fadeCanvasScript.fadeDuration) {
		// 	timeFlyingEvents[0] = storyTextScript.fadeCanvasScript.fadeDuration;
		// }
		if (boardTimer < boardEvents[boardEvents.Count - 1] + tapTime) {
			boardTimer += Time.deltaTime;
		}
		if (storyTextScript.fadeCanvasScript.shown && inputDetScript.Tapped) {
			//audioManHubMenuScript.ButtonSound(); //clicking sound
			boardBools[0] = true;
		}
		// SHOW TAP ICON
		if (boardTimer >= boardEvents[1] && !boardBools[1]) {
			storyIconsScript.ShowNextButton();
			boardBools[1] = true;
		}
		// TAP EVENT
		if (boardTimer >= boardEvents[2] && !boardBools[2] && nextBtnPressed || boardTimer >= boardEvents[2] && !boardBools[2] && inputDetScript.Tapped) {
			storyIconsScript.HideNextButton();
			tapTime = boardTimer;
			exitingBoard = true;
			nextBtnPressed = false;
			boardBools[2] = true;
			audioManHubMenuScript.ButtonSound(); //clicking sound
		}
		if (exitingBoard) {
			if (boardTimer >= boardEvents[3] + tapTime && !boardBools[3]) {
				blackScreenFadeScript.FadeIn();
				boardBools[3] = true;
			}
			if (blackScreenFadeScript.shown && boardBools[3]) {
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
				exitingBoard = false;
				tapTime = 99999f;
				// AUDIO - BOARD CHANGE TIME HOVER SOUND SHOULD STOP!
				audioIntroScript.STOP_introTimeHoverLoopSFX();
				audioIntroScript.STOP_introWindLoopSFX();

			}
		}
	}

	void Gust() {
		if (blackScreenFadeScript.shown && !boardBools[0]) {
			blackScreenFadeScript.FadeOut();
			storyScrollBGScript.SetUpClouds(storyScrollBGScript.blurredSidewaysBGs, storyScrollBGScript.blurSideScrollSpeed);
			storySingleCloudScript.PlayClouds(storySingleCloudScript.xPartSys, storySingleCloudScript.gustSpeedMult, true);
		}
		// Distorted sky instead of normal.
		if (boardTimer < boardEvents[boardEvents.Count - 1] + tapTime) {
			boardTimer += Time.deltaTime;
		}
		if (boardTimer >= boardEvents[0] && !boardBools[0]) {
			storyTextScript.SetupText(storyBoardTextNum);
			storyBoardTextNum++;
			boardBools[0] = true;
			// AUDIO - GUST MOVES IN!
			audioIntroScript.introGustHoverSFX();
			audioIntroScript.introWindLoopSFX();
		}
		if (boardTimer >= boardEvents[1] && !boardBools[1]) {
			storyGustScript.SetupXMove(storyGustScript.startTrans.position.x, storyGustScript.midTrans.position.x, storyGustScript.moveInDur, storyGustScript.moveInXCurve);
			storyGustScript.yHover = true;
			boardBools[1] = true;
		}
		// SHOW TAP ICON
		if (boardTimer >= boardEvents[2] && !boardBools[2]) {
			storyIconsScript.ShowNextButton();
			boardBools[2] = true;
		}
		// TAP EVENT
		if (boardTimer >= boardEvents[3] && !boardBools[3] && nextBtnPressed || boardTimer >= boardEvents[3] && !boardBools[3] && inputDetScript.Tapped) {
			storyIconsScript.HideNextButton();
			tapTime = boardTimer;
			exitingBoard = true;
			nextBtnPressed = false;
			boardBools[3] = true;
			audioManHubMenuScript.ButtonSound(); //clicking sound
		}
		if (exitingBoard) {
			if (boardTimer >= (boardEvents[4] + tapTime) && !boardBools[4]) {
				storyGustScript.SetupXMove(storyGustScript.midTrans.position.x, storyGustScript.endTrans.position.x, storyGustScript.moveInDur, storyGustScript.moveOutXCurve);
				boardBools[4] = true;

			//AUDIo stop wind stop from Gust HOver loop
			audioIntroScript.STOP_introGustHoverSFX();
			// AUDIO - GUST MOVES OUT!
			audioIntroScript.introGustSFX(); //stop loop play a one shot Gust sound

			}
			if (boardTimer >= (boardEvents[5] + tapTime) && !boardBools[5]) {
				blackScreenFadeScript.FadeIn();
				boardBools[5] = true;
			}
			if (boardBools[5] && blackScreenFadeScript.shown) {
				introStates = IntroStates.TheAccident;
				boardTimer = 0f;
				boardEvents.Clear();
				boardEvents = theAccidentEvents;
				boardBools.Clear();
				for(int i = 0; i < theAccidentEvents.Count; i++)
				{
					boardBools.Add(false);
				}
				tapTime = 999999f;
				exitingBoard = false;

				audioIntroScript.STOP_introWindLoopSFX();
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
		}
		if (boardTimer < boardEvents[boardEvents.Count - 1] + tapTime) {
			boardTimer += Time.deltaTime;
		}
		if (boardTimer >= boardEvents[0] && !boardBools[0]) {
			storyTextScript.SetupText(storyBoardTextNum);
			storyBoardTextNum++;
			boardBools[0] = true;

			
				// AUDIO - TIME Loop farther
				audioIntroScript.introTimeHoverLoopFarSFX();

				//AUDIO wind
				audioIntroScript.introWindLoopSFX();
		}
		// SHOW TAP ICON
		if (boardTimer >= boardEvents[1] && !boardBools[1]) {
			storyIconsScript.ShowNextButton();
			boardBools[1] = true;
		}
		// TAP EVENT
		if (boardTimer >= boardEvents[2] && !boardBools[2] && nextBtnPressed || boardTimer >= boardEvents[2] && !boardBools[2] && inputDetScript.Tapped) {
			storyIconsScript.HideNextButton();
			tapTime = boardTimer;
			exitingBoard = true;
			nextBtnPressed = false;
			boardBools[2] = true;
			audioManHubMenuScript.ButtonSound(); //clicking sound
		}
		if (exitingBoard) {
			if (boardTimer >= boardEvents[3] + tapTime && !boardBools[3]) {
				storyGustScript.SetupXMove(storyGustScript.startTrans.position.x, storyGustScript.endTrans.position.x, storyGustScript.moveAcrossDur, storyGustScript.moveAcrossCurve);
				boardBools[3] = true;

			//AUDIO COLLISION
			audioIntroScript.STOP_introTimeHoverLoopFarSFX();
			audioIntroScript.introCollisionSFX();
			}
			if (boardTimer >= boardEvents[4] + tapTime && !boardBools[4]) {
				storyTimeMoScript.SetupTimeSpin(storyTimeMoScript.fastSpinDuration);
				storyTimeMoScript.timeHovers = false;
				storyTimeMoScript.changeSpinTime = true;
				storyScrollBGScript.slowDownClouds = true;
				storySingleCloudScript.SlowDownCloudsSetup(storySingleCloudScript.xPartSys);
				boardBools[4] = true;

				// AUDIO - TIME SPINS!
				storyTimeMoScript.audioSpin = true;

			}
			if (boardTimer >= boardEvents[5] + tapTime && !boardBools[5]) {
				blackScreenFadeScript.FadeIn();
				boardBools[5] = true;

				//AUDIO - TIME STOP SPINNING!
				storyTimeMoScript.audioSpin = false;

			}

				
			if (boardBools[5] && blackScreenFadeScript.shown) {
				introStates = IntroStates.GustsMishap;
				boardTimer = 0f;
				boardEvents.Clear();
				boardEvents = gustsMishapEvents;
				boardBools.Clear();
				for(int i = 0; i < gustsMishapEvents.Count; i++)
				{
					boardBools.Add(false);
				}
				tapTime = 999999f;
				exitingBoard = false;

				//AUDIO 
				audioIntroScript.STOP_introWindLoopSFX();
			}
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
		if (boardTimer < boardEvents[boardEvents.Count - 1] + tapTime) {
			boardTimer += Time.deltaTime;
		}
		if (boardTimer >= boardEvents[0] && !boardBools[0]) {
			storyTextScript.SetupText(storyBoardTextNum);
			storyBoardTextNum++;
			boardBools[0] = true;

				//AUDIo wind Gust HOver loop
				audioIntroScript.introGustHoverSFX();
				audioIntroScript.introWindLoopSFX();


		}
		if (boardTimer >= boardEvents[1] && !boardBools[1]) {
			storyGustScript.SetupXMove(storyGustScript.startTrans.position.x, storyGustScript.midTrans.position.x, storyGustScript.moveInDur, storyGustScript.moveInXCurve);
			boardBools[1] = true;
		}		
		// SHOW TAP ICON
		if (boardTimer >= boardEvents[2] && !boardBools[2]) {
			storyIconsScript.ShowNextButton();
			boardBools[2] = true;
		}
		// TAP EVENT
		if (boardTimer >= boardEvents[3] && !boardBools[3] && nextBtnPressed || boardTimer >= boardEvents[3] && !boardBools[3] && inputDetScript.Tapped) {
			storyIconsScript.HideNextButton();
			tapTime = boardTimer;
			exitingBoard = true;
			nextBtnPressed = false;
			boardBools[3] = true;
			audioManHubMenuScript.ButtonSound(); //clicking sound
		}
		if (exitingBoard) {
			if (boardTimer >= boardEvents[4] + tapTime && !boardBools[4]) {
				storyGustScript.ChangeEyePos();
				storyGustScript.SetupXMove(storyGustScript.midTrans.position.x, storyGustScript.topEndTrans.position.x, storyGustScript.moveAcrossDur, storyGustScript.moveOutXCurve);
				storyGustScript.SetupYMove(storyGustScript.gust.transform.localPosition.y, storyGustScript.topEndTrans.position.y, storyGustScript.moveAcrossDur, storyGustScript.moveOutTopYCurve);
				storyGustScript.SetupScaleDown(storyGustScript.gust.transform.localScale.x, storyGustScript.moveOutScale, storyGustScript.moveAcrossDur, storyGustScript.moveOutTopYCurve);
				storyGustScript.gustFadeScript.FadeOut();
				boardBools[4] = true;

				//AUDIo stop wind stop from Gust HOver loop
				audioIntroScript.STOP_introGustHoverSFX();
				audioIntroScript.introGustSFX(); //stop loop , play one shot gust sound exit

			}
			if (boardTimer >= boardEvents[5] + tapTime && !boardBools[5]) {
				blackScreenFadeScript.FadeIn();
				boardBools[5] = true;
			}
			if (boardBools[5] && blackScreenFadeScript.shown) {
				introStates = IntroStates.TimeConfused;
				boardTimer = 0f;
				boardEvents.Clear();
				boardEvents = timeConfusedEvents;
				boardBools.Clear();
				for(int i = 0; i < timeConfusedEvents.Count; i++)
				{
					boardBools.Add(false);
				}
				exitingBoard = false;
				tapTime = 99999f;

				audioIntroScript.STOP_introWindLoopSFX();
			}
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

			// AUDIO - wind
			audioIntroScript.introWindLoopSFX();
			
			// AUDIO - TIME SPINS!
			//audioIntroScript.introTimeSpinLoopSFX();
			storyTimeMoScript.audioSpin = true;
		}
		if (boardTimer < boardEvents[boardEvents.Count - 1] + tapTime) {
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
		// SHOW TAP ICON
		if (boardTimer >= boardEvents[2] && !boardBools[2]) {
			storyIconsScript.ShowNextButton();
			boardBools[2] = true;
		}
		// TAP EVENT
		if (boardTimer >= boardEvents[3] && !boardBools[3] && nextBtnPressed || boardTimer >= boardEvents[3] && !boardBools[3] && inputDetScript.Tapped) {
			storyIconsScript.HideNextButton();
			tapTime = boardTimer;
			exitingBoard = true;
			nextBtnPressed = false;
			boardBools[3] = true;
			audioManHubMenuScript.ButtonSound(); //clicking sound
		}
		if (exitingBoard) {
			if (boardTimer >= boardEvents[4] + tapTime && !boardBools[4]) {
				blackScreenFadeScript.FadeIn();	

				boardBools[4] = true;

								
				// AUDIO - TIME STOP SPINNING!
				//audioIntroScript.STOP_introTimeSpinLoopSFX();
				storyTimeMoScript.audioSpin = false;
			}
			if (boardBools[4] && blackScreenFadeScript.shown) {
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
				exitingBoard = false;
				tapTime = 99999f;
				
			// AUDIO - wind
			audioIntroScript.STOP_introWindLoopSFX();
			}
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

			//AUDIO
			audioIntroScript.introWindLoopSFX();
		}
		if (boardTimer < boardEvents[boardEvents.Count - 1] + tapTime) {
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
		if (boardTimer >= boardEvents[2] && !boardBools[2]) {
			boardBools[2] = true;
		}		
		// SHOW TAP ICON
		if (boardTimer >= boardEvents[3] && !boardBools[3]) {
			storyIconsScript.ShowNextButton();
			boardBools[3] = true;
		}
		// TAP EVENT
		if (boardTimer >= boardEvents[4] && !boardBools[4] && nextBtnPressed || boardTimer >= boardEvents[4] && !boardBools[4] && inputDetScript.Tapped) {
			storyIconsScript.HideNextButton();
			tapTime = boardTimer;
			exitingBoard = true;
			nextBtnPressed = false;
			boardBools[4] = true;
			audioManHubMenuScript.ButtonSound(); //clicking sound
		}
		if (exitingBoard) {
			if (boardTimer >= boardEvents[5] + tapTime && !boardBools[5]) {
				boardBools[5] = true;
				storyEggManScript.EggsFallOffScreen();

				//AuDIO egg falling sequence
				audioIntroScript.introEggFallingSFX();
			}
			if (boardTimer >= boardEvents[6] + tapTime && !boardBools[6]) {
				blackScreenFadeScript.FadeIn();
				boardBools[6] = true;
			}
			if (boardBools[6] && blackScreenFadeScript.shown) {
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
				exitingBoard = false;
				tapTime = 99999f;
				//AuDIO egg falling sequence
				audioIntroScript.STOP_introEggFallingSFX();
				//AUDIO
				audioIntroScript.STOP_introWindLoopSFX();

			}
		}
	}

	void TimeToTheRescue() {
		if (blackScreenFadeScript.shown && !boardBools[0]) {
			blackScreenFadeScript.FadeOut();
			storyTextScript.TurnTextOff();
			storyTimeMoScript.ChangeCurrentTime(storyTimeMoScript.divingTime);

			//AUDIO
			audioIntroScript.introWindLoopSFX();
		}
		if (boardTimer < boardEvents[boardEvents.Count - 1] + tapTime) {
			boardTimer += Time.deltaTime;
		}
		if (boardTimer >= boardEvents[0] && !boardBools[0]) {
			storyTextScript.SetupText(storyBoardTextNum);
			storyBoardTextNum++;
			boardBools[0] = true;

			//AUDIO TIME DIVE?
			audioIntroScript.introTimeDiveLoopSFX();
		}
		if (boardTimer >= boardEvents[1] && !boardBools[1]) {
			storyTimeMoScript.timeDives = true;
			storyTimeMoScript.diveIn = true;
			//storyTimeMoScript.SetupDiveHover();
			//storyTimeMoScript.diveHover = true;
			boardBools[1] = true;
		}
		// if (boardTimer >= boardEvents[2] && !boardBools[2]) {
		// 	// Will have to put a delay somehwere else.
		// 	boardBools[2] = true;
		// }
		// SHOW TAP ICON
		if (boardTimer >= boardEvents[2] && !boardBools[2]) {
			storyIconsScript.ShowNextButton();
			boardBools[2] = true;
		}
		// TAP EVENT
		if (boardTimer >= boardEvents[3] && !boardBools[3] && nextBtnPressed || boardTimer >= boardEvents[3] && !boardBools[3] && inputDetScript.Tapped) {
			storyIconsScript.HideNextButton();
			tapTime = boardTimer;
			exitingBoard = true;
			nextBtnPressed = false;
			boardBools[3] = true;

			audioManHubMenuScript.ButtonSound(); //clicking sound

		}
		if (exitingBoard) {
			if (boardTimer >= boardEvents[4] + tapTime && !boardBools[4]) {
				storyTimeMoScript.diveOut = true;
				boardBools[4] = true;

			//AUDIO TIME DIVE OUT?
			audioIntroScript.STOP_introTimeDiveLoopSFX();
			audioIntroScript.introTimeDiveSFX(); //stop the loop, play a whoosh sound

			}
			if (boardTimer >= boardEvents[5] + tapTime && !boardBools[5]) {
				blackScreenFadeScript.FadeIn();	
				boardBools[5] = true;
			}
			if (boardBools[5] && blackScreenFadeScript.shown) {
				introStates = IntroStates.TheOneEgg;
				boardTimer = 0f;
				boardEvents.Clear();
				boardEvents = theOneEggEvents;
				boardBools.Clear();
				for(int i = 0; i < theOneEggEvents.Count; i++)
				{
					boardBools.Add(false);
				}
				exitingBoard = false;
				tapTime = 99999f;

				//AUDIO
				audioIntroScript.STOP_introWindLoopSFX();

			}
		}
	}

	void TheOneEgg() {
		if (blackScreenFadeScript.shown && !boardBools[0]) {
			blackScreenFadeScript.FadeOut();
			storyTextScript.TurnTextOff();
			storyTimeMoScript.SetTimePos(storyTimeMoScript.diveStartTrans.position, true);
			storyOneEggScript.theOneEgg.SetActive(true);
			storyOneEggScript.eggTrailFX.Play();
			storyScrollBGScript.SetUpClouds(storyScrollBGScript.verticalBGs, storyScrollBGScript.verticalScrollSpeedTwo, false);
			//storyOneEggScript.rotate = true;

						//AUDIO
			audioIntroScript.introWindLoopSFX();
		}
		if (boardTimer < boardEvents[boardEvents.Count - 1] + tapTime) {
			boardTimer += Time.deltaTime;
		}
		if (boardTimer >= boardEvents[0] && !boardBools[0]) {
			storyTextScript.SetupText(storyBoardTextNum);
			storyBoardTextNum++;
			boardBools[0] = true;

							
			//AUDIO SINGLE EGG SPIN?
			audioIntroScript.introSingleEggSpinLoopSFX();
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
				storyOneEggScript.tapIconFadeScript.FadeOut();
				storyTimeMoScript.timeDivesThrough = true;	

				//AUDIO TIME DIVE?
				audioIntroScript.introTimeDiveSFX();
				//AUDIO SINGLE EGG SPIN?
				audioIntroScript.STOP_introSingleEggSpinLoopSFX();
				// AUDIO - EGG CLICKED/TAPPED!
				audioIntroScript.silverEggSnd();

				enableRaycasting = false;
				tapTime = boardTimer;
				boardBools[2] = true;
				return;
			}
		}
		if (boardTimer >= boardEvents[3] + tapTime && !boardBools[3]) {
			blackScreenFadeScript.FadeIn();

			//audioManHubMenuScript.ButtonSound(); //clicking sound

			boardBools[3] = true;
		}
		if (boardBools[3] && blackScreenFadeScript.shown) {
			introStates = IntroStates.TheQuest;
			boardTimer = 0f;
			boardEvents.Clear();
			boardEvents = theQuestEvents;
			boardBools.Clear();
			for(int i = 0; i < theQuestEvents.Count; i++)
			{
				boardBools.Add(false);
			}
			exitingBoard = false;
			tapTime = 99999f;

			//AUDIO
			audioIntroScript.STOP_introWindLoopSFX();
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

			//AUDIO : HUB MUSIC + CLOUDS
			audioManHubMenuScript.audioIntro_ON = false;
			audioManHubMenuScript.CloudsMoving();
			audioManHubMenuScript.TransitionHub();
		}
		if (boardTimer < boardEvents[boardEvents.Count - 1] + tapTime) {
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
		}
		if (boardTimer >= boardEvents[2] && !boardBools[2]) {
			// TheOneEgg flies from under time to the middle scaling to regular scene egg size
			storyOneEggScript.SetupFlyOutOfTime();
			boardBools[2] = true;

			//AUDIO egg slide
			audioIntroScript.SilverEggTrailSFX();
		}
		if (boardTimer >= boardEvents[3] && !boardBools[3]) {
			// It shakes and sparkles
		}
		// SHOW TAP ICON
		if (boardTimer >= boardEvents[4] && !boardBools[4]) {
			storyIconsScript.ShowNextButton();
			boardBools[4] = true;
		}
		// TAP EVENT
		if (boardTimer >= boardEvents[5] && !boardBools[5] && nextBtnPressed || boardTimer >= boardEvents[5] && !boardBools[5] && inputDetScript.Tapped) {
			storyIconsScript.HideNextButton();
			tapTime = boardTimer;
			exitingBoard = true;
			nextBtnPressed = false;
			boardBools[5] = true;
			audioManHubMenuScript.ButtonSound(); //clicking sound
		}
		if (exitingBoard) {
			if (boardTimer >= boardEvents[6] + tapTime && !boardBools[6]) {
				// Tap (anywhere or on it)
				// Text fades out, time fades out,
				storyTextScript.FadeOutText();
				storyTimeMoScript.FadeOutGlidingTime();
				storyOneEggScript.theOneEggFadeScript.FadeOut();
				// Regular hub gets activated
				hubScript.startHubActive = true;
				// Relative to the Hub script's hubActiveWait float amount. hubActiveWait - hubActiveFaster = delay after click.
				hubScript.hubActiveFaster = 2.5f;
				boardBools[6] = true;
			}
			if (boardBools[6]) {
				introStates = IntroStates.TitleScreen;
				boardTimer = 0f;
				boardEvents.Clear();
				boardEvents = onceUponATimeEvents;
				boardBools.Clear();
				for(int i = 0; i < onceUponATimeEvents.Count; i++)
				{
					boardBools.Add(false);
				}
				exitingBoard = false;
				tapTime = 99999f;
				inStoryIntro = false;
				mainMenuScript.fullIntro = false;
			}
		}
	}
	
	void ResetStory() {
		menuFaded = false;
		storyTimeMoScript.ResetNormalTime();
		storyGustScript.gust.transform.position = storyGustScript.startTrans.position;
		storyGustScript.gustFadeScript.FadeIn();
		
	}
}