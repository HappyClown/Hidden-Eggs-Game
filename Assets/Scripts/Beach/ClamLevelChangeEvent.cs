using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClamLevelChangeEvent : MonoBehaviour {
	private float endEventTimer;
	public bool endEventOn;
	public float stuffFadeOutF, animStartF, activateEggsF, finishedF;
	private bool stuffFadeOutB, activateEggsB, finishedB;
	public bool animStartB;
	public ClamPuzzle clamPuzzleScript;
	public SilverEggsManager silverEggManScript;

	void Update () {
		if (endEventOn) {
			endEventTimer += Time.deltaTime;
			if (endEventTimer >= stuffFadeOutF && !stuffFadeOutB) {
				stuffFadeOutB = true;
				clamPuzzleScript.LvlStuffFadeOut();
			}
			if (endEventTimer >= animStartF && !animStartB) {
				animStartB = true;
			}
			if (endEventTimer >= activateEggsF && !activateEggsB) {
				activateEggsB = true;
				foreach(GameObject silEgg in silverEggManScript.activeSilverEggs)
				{
					silEgg.GetComponent<SilverEggSequence>().StartSequence();
				}
				clamPuzzleScript.scrnDarkImgScript.FadeIn();
			}
			if (endEventTimer >= finishedF && !finishedB) {
				endEventOn = false;
				endEventTimer = 0f;
				stuffFadeOutB = false;
				animStartB = false;
				activateEggsB = false;
			}
		}
	}

	public void LevelChangeEvent() {
		endEventOn = true;
	}
}
