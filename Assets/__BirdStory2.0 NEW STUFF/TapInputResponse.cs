using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapInputResponse : MonoBehaviour {

	public ClickOnEggs clickOnEggs;


	public void Tapped() {
		// if mySceneState = eggFinding
		clickOnEggs.TapChecks(); // This checks if an egg, panel, puzzle, etc. has been tapped on.
	}
}
