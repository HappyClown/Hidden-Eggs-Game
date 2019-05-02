using System.Collections;
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
	[Header("Stuff")]
	public bool inStoryIntro;
	private int storyBoardTextNum;
	private bool menuFaded;
	[Header("StoryBoard Events")]
	public List<float> onceUponATimeEvents;
	public List<float> timeFlyingEvents, gustEvents, theAccidentEvents, gustsMishapEvents, timeConfusedEvents, eggsFallingEvents, timeToTheRescueEvents, theOneEggEvents, theQuestEvents;
	private float boardTimer;
	public List<float> boardEvents;
	public List<bool> boardBools;

	public enum IntroStates {
		TitleScreen, OnceUponATime, TimeFlying, Gust, TheAccident, GustsMishap, TimeConfused, EggsFalling, TimeToTheRescue, TheOneEgg, TheQuest
	}
	public IntroStates introStates; 

	void Start () {
		boardEvents.Add(0f);
		boardBools.Add(false);

		// For testing purposes, should be commented out OR set to the first IntroState.
		boardTimer = 0f;
		//boardEvents.Clear();
		//boardEvents = theQuestEvents;
		//boardBools.Clear();
		//for(int i = 0; i < theQuestEvents.Count; i++)
		//{
		//	boardBools.Add(false);
		//}
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
	}

	void TitleScreen() {
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
			storyScrollBGScript.SetUpClouds(storyScrollBGScript.regularSidewaysBGs, storyScrollBGScript.regSideScrollSpeed, true);
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
		}
	}

	void Gust() {
		if (blackScreenFadeScript.shown && !boardBools[0]) {
			blackScreenFadeScript.FadeOut();
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
		}
		if (boardTimer >= boardEvents[2] && !boardBools[2]) {
			storyGustScript.SetupXMove(storyGustScript.midTrans.position.x, storyGustScript.endTrans.position.x, storyGustScript.moveInDur, storyGustScript.moveOutXCurve);
			boardBools[2] = true;
		}
		if (boardBools[2] && inputDetScript.Tapped) {
			blackScreenFadeScript.FadeIn();
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
			// Back to normal sky scrolling.
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
			boardBools[2] = true;
		}
		if (boardBools[2] && inputDetScript.Tapped) {
			blackScreenFadeScript.FadeIn();
		}
		if (boardBools[2] && blackScreenFadeScript.shown) {
			introStates = IntroStates.GustsMishap;
			boardTimer = 0f;
			boardEvents.Clear();
			boardEvents = gustsMishapEvents;
			boardBools.Clear();
			for(int i = 0; i < gustsMishapEvents.Count; i++)
			{
				boardBools.Add(false);
			}
		}
	}

	void GustsMishap() {
		if (blackScreenFadeScript.shown && !boardBools[0]/*  || Input.GetKeyDown("space") */) {
			blackScreenFadeScript.FadeOut();
			storyTextScript.TurnTextOff();
			storyTimeMoScript.normalTime.SetActive(false);
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
			storyGustScript.SetupXMove(storyGustScript.midTrans.position.x, storyGustScript.topEndTrans.position.x, storyGustScript.moveAcrossDur, storyGustScript.moveOutXCurve);
			storyGustScript.SetupYMove(storyGustScript.gust.transform.position.y, storyGustScript.topEndTrans.position.y, storyGustScript.moveAcrossDur, storyGustScript.moveOutTopYCurve);
			storyGustScript.SetupScaleDown(storyGustScript.gust.transform.localScale.y, storyGustScript.moveOutScale, storyGustScript.moveAcrossDur, storyGustScript.moveOutTopYCurve);
			storyGustScript.gustFadeScript.FadeOut();
			boardBools[2] = true;
		}
		if (boardBools[2] && inputDetScript.Tapped) {
			blackScreenFadeScript.FadeIn();
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
			storyTimeMoScript.timeHovers = false;
			storyTimeMoScript.SetupTimeSpin(storyTimeMoScript.slowSpinDuration);
			storyTimeMoScript.SetTimePos(storyTimeMoScript.bewilderedMidTrans.position, false);
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
		if (boardBools[1] && inputDetScript.Tapped) {
			blackScreenFadeScript.FadeIn();	
		}
		if (boardBools[1] && blackScreenFadeScript.shown) {
			introStates = IntroStates.TheOneEgg;
			boardTimer = 0f;
			boardEvents.Clear();
			boardEvents = theOneEggEvents;
			boardBools.Clear();
			for(int i = 0; i < theOneEggEvents.Count; i++)
			{
				boardBools.Add(false);
			}
		}
	}

	void TheOneEgg() {
		if (blackScreenFadeScript.shown && !boardBools[0]) {
			blackScreenFadeScript.FadeOut();
			storyTextScript.TurnTextOff();
			storyTimeMoScript.SetTimePos(storyTimeMoScript.diveStartTrans.position, true);
			storyOneEggScript.theOneEgg.SetActive(true);
			storyOneEggScript.rotate = true;
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
			storyTimeMoScript.timeDivesThrough = true;
			boardBools[1] = true;
		}
		if (boardTimer >= boardEvents[2] && !boardBools[2]) {
			storyOneEggScript.theOneEgg.SetActive(false);
			storyOneEggScript.rotate = false;
			boardBools[2] = true;
		}
		if (boardBools[2] && inputDetScript.Tapped) {
			blackScreenFadeScript.FadeIn();	
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
			// mainMenuScript.ToHub(); Without the hubScript.startHubActive = true; so that it fades out the main menu but only shows the grey village
			mainMenuScript.ToHub(false);
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
			// Regular hub gets activated
			hubScript.startHubActive = true;
			boardBools[4] = true;
		}
		if (boardBools[4]) {
			introStates = IntroStates.TitleScreen;
			boardTimer = 0f;
			boardEvents.Clear();
			boardBools.Clear();
			inStoryIntro = false;
		}
	}
	
	void ResetStory() {
		menuFaded = false;
	}
}
