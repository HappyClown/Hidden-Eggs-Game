using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzTutorial : MonoBehaviour {
	public SlideInHelpBird slideInHelpScript;
	public FadeInOutImage tutFadeScript;
	public inputDetector inputDetScript;
	public LevelSelectionButtons levelSelectScript;
	private bool showTut;

	void Update () {
		if (slideInHelpScript.isUp && !showTut) {
			tutFadeScript.FadeIn();
			showTut = true;
		}

		if ((slideInHelpScript.moveDown || slideInHelpScript.isDown) && showTut) {
			tutFadeScript.FadeOut();
			showTut = false;
		}

		if (inputDetScript.Tapped && tutFadeScript.shown) {
			slideInHelpScript.MoveBirdUpDown();
			//engineScript.canPlay = true;
			//levelSelectScript.InteractableThreeDots();
		}
	}
}
