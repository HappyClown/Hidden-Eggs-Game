using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClamLevelChangeEvent : MonoBehaviour {
	private float endEventTimer;
	public SpriteRenderer bootFront;
	public bool endEventOn;
	public float stuffFadeOutF, animStartF, activateEggsF, changeBootOrderF, finished;
	private bool stuffFadeOutB, activateEggsB, changeBootOrderB;
	public bool animStartB;
	public Animator bootAnim;
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
				bootAnim.SetTrigger("BootShake");
				bootFront.sortingLayerName = "SilverEgg";
				animStartB = true;
			}
			if (endEventTimer >= activateEggsF && !activateEggsB) {
				activateEggsB = true;
				foreach(GameObject silEgg in silverEggManScript.activeSilverEggs)
				{
					silEgg.GetComponent<SilverEggSequence>().StartSequence();
				}
			}
			if (endEventTimer >= changeBootOrderF && !changeBootOrderB) {
				bootFront.sortingLayerName = "Default";
				if (bootFront.sortingOrder != 0) {
					bootFront.sortingOrder = 0;
				}
				changeBootOrderB = true;
				clamPuzzleScript.scrnDarkImgScript.FadeIn();
			}
			if (endEventTimer >= finished) {
				endEventOn = false;
				endEventTimer = 0f;
				stuffFadeOutB = false;
				animStartB = false;
				activateEggsB = false;
				changeBootOrderB = false;
			}
		}
	}

	public void LevelChangeEvent() {
		endEventOn = true;
		bootFront.sortingLayerName = "SilverEgg";
		if (bootFront.sortingOrder != 1) {
			bootFront.sortingOrder = 1;
		}
		Debug.Log("Hello, LevelChangeEvent speaking.");
	}
}
