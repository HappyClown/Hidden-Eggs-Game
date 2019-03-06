using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryIntro : MonoBehaviour {
	[Header("Scripts")]
	public MainMenu mainMenuScript;
	[Header("Stuff")]
	public bool inStoryIntro;
	public GameObject timeNormal, timeConfused, timeDiving, gust;
	private bool menuFaded;

	public enum IntroStates {
		TitleScreen, OnceUponATime, TimeFlying, Gust, TheAccident, GustsMishap, TimeConfused, EggsFalling, TimeToTheRescue, TheOneEgg, TheQuest
	}
	public IntroStates introStates; 

	void Start () {
		
	}
	
	void Update () {
		if (inStoryIntro) {
			switch(introStates) {
				case IntroStates.TitleScreen:
					TitleScreen(); break;
				case IntroStates.OnceUponATime:
					OnceUponATime(); break;
				case IntroStates.TimeFlying:
					break;
				case IntroStates.Gust:
					break;
				case IntroStates.TheAccident:
					break;
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
		Debug.Log(introStates);
		if (!menuFaded) {
			mainMenuScript.FadeMainMenu();
			menuFaded = true;
		}
		if (mainMenuScript.titleFade.hidden/*  && mainMenuScript.playBtnFadeScript.hidden */ && mainMenuScript.rstBtnFadeScript.hidden) {
			introStates = IntroStates.OnceUponATime;
		}
	}

	void OnceUponATime() {
		
		Debug.Log(introStates);
	}

	void ResetStory() {
		menuFaded = false;
	}
}
