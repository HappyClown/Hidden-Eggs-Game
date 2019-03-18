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
	public List<float> timeFlyingEvents, gustEvents, theAccidentEvents;
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
					break;
				case IntroStates.TimeConfused:
					break;
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
		boardTimer += Time.deltaTime;
		if (boardTimer >= boardEvents[0] && !boardBools[0]) {
			if (!storyScrollBGScript.scroll) {
				storyScrollBGScript.scroll = true;
			}
			boardBools[0] = true;
		}
		if (boardTimer >= boardEvents[1] && !boardBools[1]) {
			storyTextScript.SetupText(storyBoardTextNum);
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
			storyBoardTextNum = 1;
			boardTimer = 0f;
			storyTextScript.ChangeTextFade(storyBoardTextNum);
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
			storyBoardTextNum = 2;

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
		if (blackScreenFadeScript.shown) {
			blackScreenFadeScript.FadeOut();
		}
		// Distorted sky instead of normal;
		boardTimer += Time.deltaTime;
		if (boardTimer >= boardEvents[0] && !boardBools[0]) {
			storyTextScript.SetupText(storyBoardTextNum);
			boardBools[0] = true;
		}
		if (boardTimer >= boardEvents[1] && !boardBools[1]) {
			storyGustScript.SetupXMove(storyGustScript.startTrans.position.x, storyGustScript.midTrans.position.x, storyGustScript.moveInDur, storyGustScript.moveInXCurve, false);
			storyGustScript.yHover = true;
			boardBools[1] = true;
		}
		if (boardTimer >= boardEvents[2] && !boardBools[2]) {
			storyGustScript.SetupXMove(storyGustScript.midTrans.position.x, storyGustScript.endTrans.position.x, storyGustScript.moveInDur, storyGustScript.moveOutXCurve, true);
			boardBools[2] = true;
		}
		if (boardBools[2] && inputDetScript.Tapped) {
			blackScreenFadeScript.FadeIn();
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
		if (blackScreenFadeScript.shown) {
			blackScreenFadeScript.FadeOut();
			storyTimeMoScript.normalTime.SetActive(true);
		}
		if (boardTimer >= boardEvents[0] && boardBools[0]) {
			storyGustScript.SetupXMove(storyGustScript.startTrans.position.x, storyGustScript.endTrans.position.x, storyGustScript.moveInDur, storyGustScript.moveInXCurve, true);
			boardBools[0] = true;
		}  

	}

	void ResetStory() {
		menuFaded = false;
	}
}
