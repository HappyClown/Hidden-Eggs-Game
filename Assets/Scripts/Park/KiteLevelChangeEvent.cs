using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KiteLevelChangeEvent : MonoBehaviour {
	[Header("Sequence Values")]
	private float eventTimer;
	public bool endEventOn;
	public float stuffFadeOutF, animStartF, activateEggsF, finishedF;
	private bool stuffFadeOutB, activateEggsB, finishedB;
	public bool animStartB;
	[Header("Scripts")]
	public KitePuzzEngine kitePuzzleEngine;
	public SilverEggsManager silverEggManScript;
	
	void Update () {
		if (endEventOn) {
			eventTimer += Time.deltaTime;
			if (eventTimer >= stuffFadeOutF && !stuffFadeOutB) {
				stuffFadeOutB = true;
				kitePuzzleEngine.LvlStuffFadeOut();
			}
			if (eventTimer >= animStartF && !animStartB) {
				animStartB = true;
				//kitePuzzleEngine.bgScleScript.ScaleBG();
			}
			if (eventTimer >= activateEggsF && !activateEggsB) {
				activateEggsB = true;
				foreach(GameObject silEgg in silverEggManScript.activeSilverEggs)
				{
					silEgg.GetComponent<SilverEggSequence>().StartSequence();
				}
				kitePuzzleEngine.scrnDarkImgScript.FadeIn();
			}
			if (eventTimer >= finishedF && !finishedB) {
				endEventOn = false;
				eventTimer = 0f;
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