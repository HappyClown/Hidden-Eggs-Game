using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTapEnabler : MonoBehaviour 
{
	[Header("Scene")]
	public bool canTapEggRidPanPuz;
	public bool canTapHelpBird;
	public bool canTapPauseBtn;
	public bool canTapGoldEgg;
	public bool canTapLvlComp;
	public inputDetector inputDetectorScript;
	// [Header("Puzzle")]
	// public bool canTapPuzzReset;

	void Update () {
		if (inputDetectorScript && inputDetectorScript.detectDoubleTap != canTapEggRidPanPuz) {
			inputDetectorScript.detectDoubleTap = canTapEggRidPanPuz;
			inputDetectorScript.detectDoubleTouch = canTapEggRidPanPuz;
		}
	}

	public void TapLevelStuffFalse() {
		canTapEggRidPanPuz = false;
		canTapHelpBird = false;
		canTapPauseBtn = false;
	}

	public void TapLevelStuffTrue() {
		canTapEggRidPanPuz = true;
		canTapHelpBird = true;
		canTapPauseBtn = true;
	}
}
