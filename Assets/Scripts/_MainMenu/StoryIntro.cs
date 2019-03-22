using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryIntro : MonoBehaviour {
	[Header("Scripts")]
	public MainMenu mainMenuScript;
	public inputDetector inputDetScript;
	public FadeInOutImage blackScreenFadeScript;
	public StoryScrollingBG storyScrollBGScript;
	public StoryTimeMotions storyTimeMoScript;
	public StoryText storyTextScript;
	public StoryGustMotions storyGustScript;
	[Header("Stuff")]
	public bool inStoryIntro;
	private int storyBoardTextNum;
	private bool menuFaded;
	[Header("StoryBoard Events")]
	public List<float> onceUponATimeEvents;
	public List<float> timeFlyingEvents, gustEvents, theAccidentEvents, gustsMishapEvents, timeConfusedEvents;
	private float boardTimer;
	public List<float> boardEvents;
	public List<bool> boardBools;

	public enum IntroStates {
		TitleScreen, OnceUponATime, TimeFlying, Gust, TheAccident, GustsMishap, TimeConfused, EggsFalling, TimeToTheRescue, TheOneEgg, TheQuest
	}
	public IntroStates introStates; 

	void Start () {
		// boardEvents.Add(0f);
		// boardBools.Add(false);
	}
	
	void Update () {
		Debug.Log(introStates);
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
					break;
				case IntroStates.TimeToTheRescue:
					break;
				case IntroStates.TheOneEgg:
					break;
				case IntroStates.TheQuest:
					break;
			}
		}
	}

	void TitleScreen() {
		if (!menuFaded) {
			mainMenuScript.FadeMainMenu();
			menuFaded = true;
		}
		if (mainMenuScript.titleFade.hidden/*  && mainMenuScript.playBtnFadeScript.hidden */ && mainMenuScript.rstBtnFadeScript.hidden) {
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
			if (!storyScrollBGScript.scroll) {
				storyScrollBGScript.scroll = true;
			}
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
			storyTimeMoScript.timeSpins = true;
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
			blackScreenFadeScript.FadeOut();
			storyTextScript.TurnTextOff();
			storyTimeMoScript.ChangeCurrentTime(storyTimeMoScript.bewilderedTime);
			storyTimeMoScript.timeHovers = false;
			storyTimeMoScript.timeSpins = true;
			storyTimeMoScript.SetPosMid();
		}
		if (boardTimer >= boardEvents[0] && !boardBools[0]) {
			storyTextScript.SetupText(storyBoardTextNum);
			boardBools[0] = true;
		}
		if (boardTimer >= boardEvents[1] && !boardBools[1]) {

		}
	}

	void ResetStory() {
		menuFaded = false;
	}
}
