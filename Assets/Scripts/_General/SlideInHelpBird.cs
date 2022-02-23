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
	[Header("Other Objects")]
	public GameObject dialogueObjectParent;
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
	private Coroutine activeCoroutine;
	[Header("Script References")]
	public SceneTapEnabler sceneTapScript;
	public LevelTapMannager lvlTapManScript;
	public inputDetector inputDetector;
	public AudioHelperBird audioHelperBirdScript;
	public HintManager myHint;
	public HelpIntroText helpIntroText;
	public HelperBirdHint helperBirdHint;
	public HelperBirdRiddle helperBirdRiddle;

	void Start () {
		totalDist = Vector2.Distance(hiddenHelpBirdPos.position, shownHelpBirdPos.position);
		if (!audioHelperBirdScript) {
			audioHelperBirdScript = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioHelperBird>();
		}
	}

	IEnumerator MoveUp () {
		while (lerpValue < 1) {
			// Move bird up to its shown position.
			lerpValue += Time.deltaTime / newDuration;
			helpBirdTrans.position = Vector3.Lerp(curHelpBirdPos, shownHelpBirdPos.position, animCur.Evaluate(lerpValue));
			// Update the percentage of the way the bird has moved.
			distLeft = Vector2.Distance(helpBirdTrans.position, shownHelpBirdPos.position);
			distPercent = (totalDist - distLeft) / totalDist;
			// Fade in the shadow at the bottom of the screen.
			if (shadowAlpha < 1) { shadowAlpha = distPercent; }
			shadow.color = new Color(1,1,1, shadowAlpha);
			// Continue to fade out the text buble even if the bird is going up.
			if (txtBubAlpha > 0) { txtBubAlpha -= Time.deltaTime / txtBubFadeTime; }
			txtBubble.color = new Color(1,1,1, txtBubAlpha);
			yield return null;
		}
		lerpValue = 1;
		isUp = true;
		moveUp = false;
		birdUpTrigger = true;
		helpBirdTrans.position = shownHelpBirdPos.position;
		if (introDone) {
			helperBirdHint.ShowHintButton();
			helperBirdRiddle.ShowRiddleButton();
		}
		while (txtBubAlpha < 1) {
			// Fade in bird parchment
			if (txtBubAlpha < 1) {
				txtBubAlpha += Time.deltaTime / txtBubFadeTime; 
				txtBubble.color = new Color(1,1,1, txtBubAlpha);
			}
			yield return null;
		}
		// Check to see what should appear in the bird dialogue box.
		DialogueBoxChecks();
	}

	IEnumerator MoveDown () {
		distLeft = Vector2.Distance(helpBirdTrans.position, shownHelpBirdPos.position);
		distPercent = (totalDist - distLeft) / totalDist;
		if (closeMenuOnClick) { 
			closeMenuOnClick.SetActive(false);
		}
		if (dontCloseMenu) { 
			dontCloseMenu.SetActive(false);
		}
		
		while (lerpValue < 1) {
			if (!allowClick && lerpValue > 0.05f) {
				allowClick = true;
				if (sceneTapScript) { sceneTapScript.TapLevelStuffTrue(); }
			}
			// Move bird up to its shown position.
			lerpValue += Time.deltaTime / newDuration;
			helpBirdTrans.position = Vector3.Lerp(curHelpBirdPos, hiddenHelpBirdPos.position, animCur.Evaluate(lerpValue));
			// Update the percentage of the way the bird has moved.
			distLeft = Vector2.Distance(helpBirdTrans.position, shownHelpBirdPos.position);
			distPercent = (totalDist - distLeft) / totalDist;
			// Fade in the shadow at the bottom of the screen.
			if (shadowAlpha > 0) { shadowAlpha = distPercent; }
			shadow.color = new Color(1,1,1, shadowAlpha);
			// Fade out text bubble.
			if (txtBubAlpha > 0) { txtBubAlpha -= Time.deltaTime / txtBubFadeTime; }
			txtBubble.color = new Color(1,1,1, txtBubAlpha);
			yield return null;
		}
		moveDown = false;
		isDown = true;
		txtBubble.gameObject.SetActive(false);
		dialogueObjectParent.SetActive(false);
		helpBirdTrans.position = hiddenHelpBirdPos.position;
	}
	// Move the bird up or down depending on its current state.
	public void MoveBirdUpDown() {
		curHelpBirdPos = helpBirdTrans.position;
		if (moveDown || isDown) { 
			if (!isDown) { 
				// 1 is not necessary since 1 is always the lerp's max value, just there to visualize the rule of three. 
				newDuration = duration * lerpValue / 1; 
			} else { 
				newDuration = duration; 
				txtBubble.gameObject.SetActive(true); 
				dialogueObjectParent.SetActive(true);
			} 
			lerpValue = 0;
			moveDown = false;
			isDown = false;
			moveUp = true;
			allowClick = true;
			//sceneTapScript.canTapEggRidPanPuz = false;
			if (lvlTapManScript) { lvlTapManScript.ZoomOutCameraReset(); }
			if (introDone) { closeMenuOnClick.SetActive(true); }
			if (sceneTapScript) { sceneTapScript.TapLevelStuffFalse(); }
			audioHelperBirdScript.birdHelpSound();
			// If the helper bird is already moving up or down stop the coroutine before starting a new one.
			if (activeCoroutine != null) {
				StopCoroutine(activeCoroutine);
			}
			// Because this script is also used in puzzles it first needs to check if the variable exists AKA if the player is currently in a main scene.
			if (myHint) {
				myHint.ResetHint();
			}
			activeCoroutine = StartCoroutine(MoveUp());
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
			if (activeCoroutine != null) {
				StopCoroutine(activeCoroutine);
			}
			inputDetector.cancelDoubleTap = true;
			helperBirdHint.HideHintButton();
			helperBirdRiddle.HideRiddleButton();
			helperBirdRiddle.HideRiddleText();
			activeCoroutine = StartCoroutine(MoveDown());
		}
	}
	void DialogueBoxChecks() {
		// Am I in a scene or a puzzle.
		// Check if the intro has been shown.
		if (!introDone) {
			helpIntroText.StartIntroText();
		}
	}
	public void ShowButtonsAfterIntro() {
		helperBirdHint.ShowHintButton();
		helperBirdRiddle.ShowRiddleButton();
	}
}
