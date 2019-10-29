using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzTutorial : MonoBehaviour {
	public SlideInHelpBird slideInHelpScript;
	public List<FadeInOutImage> tutFadeScripts;
	private FadeInOutImage currentTutFadeScript, lastTutFadeScript;
	private int currentTutInList = 0;
	public inputDetector inputDetScript;
	public LevelSelectionButtons levelSelectScript;
	public MainPuzzleEngine mainPuzzScript;
	public SceneTapEnabler sceneTapScript;
	public FadeInOutImage darkScreenFadeScript;
	public AudioHelperBird audioHelperBirdScript;
	private bool showTut;
	private bool darkenScreen, tutOpen;

	void Start () {
		currentTutFadeScript = tutFadeScripts[0];
		lastTutFadeScript = tutFadeScripts[tutFadeScripts.Count - 1];
		if (!audioHelperBirdScript) {
			audioHelperBirdScript = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioHelperBird>();
		}
	}

	void Update () {
		// Set the tutorial as "open", used to see if the player can play the puzzle or not.
		if (slideInHelpScript.moveUp && !tutOpen) {
			tutOpen = true;
		}
		// Set the tutorial as "closed", used to allow the player to interact with the puzzle.
		if (slideInHelpScript.moveDown && slideInHelpScript.lerpValue > 0.5f && tutOpen) {
			tutOpen = false;
			mainPuzzScript.canPlay = true;
		}
		// If the tutorial is "open" make sure the player cannot interact with the puzzle.
		if (tutOpen) {
			mainPuzzScript.canPlay = false;
		}
		// If the bird is moving up, fade in the darkened screen.
		if (slideInHelpScript.moveUp && !darkenScreen) {
			darkScreenFadeScript.FadeIn();
			// mainPuzzScript.canPlay = false;
			darkenScreen = true;
		}
		// If the bird is fully up, fade in the first tutorial icons.
		if (slideInHelpScript.isUp && !showTut) {
			tutFadeScripts[0].FadeIn();
			mainPuzzScript.canPlay = false;
			showTut = true;
		}
		// If the bird goes down fade out the last tutorial icons.
		if ((slideInHelpScript.moveDown || slideInHelpScript.isDown) && showTut) {
			currentTutFadeScript.FadeOut();
			darkScreenFadeScript.FadeOut();
			showTut = false;
			darkenScreen = false;
		}
		// To go to the next tutorial icons or close the tutorial.
		if (inputDetScript.Tapped) {
			// If there is more then one set of tutorial icons.
			if (currentTutFadeScript != lastTutFadeScript && currentTutFadeScript.shown) {
				currentTutFadeScript.FadeOut();
				currentTutInList++;
				currentTutFadeScript = tutFadeScripts[currentTutInList];
				currentTutFadeScript.FadeIn();
				audioHelperBirdScript.birdHelpSound();
			}
			else if (lastTutFadeScript.shown) {
				slideInHelpScript.MoveBirdUpDown();
				mainPuzzScript.canPlay = true;
				levelSelectScript.InteractableThreeDots(mainPuzzScript.maxLvl, mainPuzzScript.curntLvl);
				sceneTapScript.canTapPauseBtn = true;
				SaveIntroDone();
				currentTutFadeScript.FadeOut();
				currentTutFadeScript = tutFadeScripts[0];
				currentTutInList = 0;
				audioHelperBirdScript.birdHelpSound();
			}
		}
	}

	void SaveIntroDone () {
		GlobalVariables.globVarScript.puzzIntroDone = true;
		GlobalVariables.globVarScript.SaveEggState();
	}
}
