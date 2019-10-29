using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlideInHelpBird : MonoBehaviour {
	[Header("Shadow")]
	public Image shadow;
	public float shadowAlpha;
	[Header("Text Bubble")]
	public Image txtBubble;
	private float txtBubAlpha;
	public float txtBubFadeTime;
	public bool introDone, txtBubFadedIn;
	public bool birdUpTrigger;
	[Header("Zones")]
	public GameObject closeMenuOnClick;
	public GameObject dontCloseMenu;
	private float allowClickTimer;
	[Header("Bird Movement")]
	public float duration;
	private float newDuration = 0.001f;
	public bool moveUp, moveDown, isUp, isDown = true, allowClick = true;
	public Transform helpBirdTrans, hiddenHelpBirdPos, shownHelpBirdPos;
	public Vector3 curHelpBirdPos;
	private float totalDist;
	private float distLeft;
	private float distPercent;
	public float lerpValue;
	public AnimationCurve animCur;
	[Header("Script References")]
	public SceneTapEnabler sceneTapScript;
	public LevelTapMannager lvlTapManScript;

	public AudioHelperBird audioHelperBirdScript;
	public HintManager myHint;

	void Start () {
		totalDist = Vector2.Distance(hiddenHelpBirdPos.position, shownHelpBirdPos.position);
		if (!audioHelperBirdScript) {
			audioHelperBirdScript = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioHelperBird>();
		}
	}

	void Update () {
		distLeft = Vector2.Distance(helpBirdTrans.position, shownHelpBirdPos.position);
		distPercent = (totalDist - distLeft) / totalDist;

		#region Up
		if (moveUp) {
			lerpValue += Time.deltaTime / newDuration;
			helpBirdTrans.position = Vector3.Lerp(curHelpBirdPos, shownHelpBirdPos.position, animCur.Evaluate(lerpValue));

			if (shadowAlpha < 1) { shadowAlpha = distPercent; }
			shadow.color = new Color(1,1,1, shadowAlpha);

			// Continue to fade out the text buble even if the bird is going up.
			if (txtBubAlpha > 0) { txtBubAlpha -= Time.deltaTime / txtBubFadeTime; }
			txtBubble.color = new Color(1,1,1, txtBubAlpha);
		}

		// Finished moving up
		if (moveUp && lerpValue >= 1) {
			lerpValue = 1;
			isUp = true;
			moveUp = false;
			birdUpTrigger = true;
			helpBirdTrans.position = shownHelpBirdPos.position;
		}
		// Is all the way up
		if (isUp) {
			 // Fade in bird parchment
			if (txtBubAlpha < 1) {
				txtBubAlpha += Time.deltaTime / txtBubFadeTime; 
				txtBubble.color = new Color(1,1,1, txtBubAlpha);
			}
		}
		#endregion

		#region Down
		if (moveDown) {
			//Debug.Log("Move down was true here. RIP");
			if (closeMenuOnClick) { 
				closeMenuOnClick.SetActive(false);
			}
			if (dontCloseMenu) { 
				dontCloseMenu.SetActive(false);
			}

			lerpValue += Time.deltaTime / newDuration;
			helpBirdTrans.position = Vector3.Lerp(curHelpBirdPos, hiddenHelpBirdPos.position, animCur.Evaluate(lerpValue));

			if (shadowAlpha > 0) { shadowAlpha = distPercent; }
			shadow.color = new Color(1,1,1, shadowAlpha);

			// Fade out text bubble
			if (txtBubAlpha > 0) { txtBubAlpha -= Time.deltaTime / txtBubFadeTime; }
			txtBubble.color = new Color(1,1,1, txtBubAlpha);
		}

		// Finished moving down
		if (moveDown && lerpValue >= 1) {
			moveDown = false;
			isDown = true;
			helpBirdTrans.position = hiddenHelpBirdPos.position;
		}

		// Allow Clicking on eggs, puzzle, etc
		if (moveDown && !allowClick || isDown && !allowClick) {
			if (allowClickTimer > 0.05f) { // Skipping 2 frames to avoid disabling the close help button and tapping on an egg at the same time
				if (sceneTapScript) {
					sceneTapScript.TapLevelStuffTrue();
				}
				allowClickTimer = 0;
				allowClick = true;
			}
			if (!sceneTapScript.canTapEggRidPanPuz) {
				allowClickTimer += Time.deltaTime;
			}
		}
		#endregion
	}

	#region Methods
	public void MoveBirdUpDown() {
		curHelpBirdPos = helpBirdTrans.position;
		if(myHint){
			myHint.resetHint = true;
		}
		if (moveDown || isDown) { // 1 is not necessary since 1 is always the lerp's max value, just there to visualize the rule of three. 
			if (!isDown) { newDuration = duration * lerpValue / 1; } else { newDuration = duration; } 
			lerpValue = 0;
			moveDown = false;
			isDown = false;
			moveUp = true;
			allowClick = true;
			//sceneTapScript.canTapEggRidPanPuz = false;
			if (lvlTapManScript) { lvlTapManScript.ZoomOutCameraReset(); }
			if (introDone) { closeMenuOnClick.SetActive(true); }
			if (sceneTapScript) {
				sceneTapScript.TapLevelStuffFalse();
			}
			audioHelperBirdScript.birdHelpSound();
			return;
		}
		if (moveUp || isUp) {
			if (!isUp) { newDuration = duration * lerpValue / 1; } else { newDuration = duration; }
			lerpValue = 0;
			moveUp = false;
			isUp = false;
			moveDown = true; 
			txtBubFadedIn = false;
			allowClick = false;			
		}
	}
	#endregion
}
