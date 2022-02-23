using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCompHelpBird : MonoBehaviour {
	[Header ("Settings")]
	public float moveDur;
	public AnimationCurve moveCurve;
	public float bubAdjustDur;
	public float bubSizeA, bubSizeB, bubSizeC;
	[Header ("References")]
	public GameObject helperBird;
	public Transform hiddenTrans, shownTrans;
	public SpriteRenderer textBubSpriteRend;
	public Animator congratsTextAnim;
	public FadeInOutSprite textBubFadeScript;
	public FadeInOutSprite textBubPointerFadeScript;
	public FadeInOutTMP congratsTextFadeScript;
	public FadeInOutCanvasGroup counterCGFadeScript;
	public FadeInOutCanvasGroup backBtnCGFadeScript;
	public GameObject congratsTextCanvasGO, eggCounterCanvasGO, EndLvlButtCanvasGO;
	public GameObject helpBirdParentGO;
	public AudioHelperBird audioHelperBirdScript;

	[Header ("Info")]
	public bool moveUp;
	private float newBubSize, prevBubSize, curBubSize, bubLerp;

	private bool audioBirdPop = false;

	void Start () {
		if (!audioHelperBirdScript) {audioHelperBirdScript = GameObject.Find("Audio").GetComponent<AudioHelperBird>();}
	}

	public void StartLevelCompBird() {
		this.enabled = true;
		helpBirdParentGO.SetActive(true);
		helperBird.SetActive(true);
		StartCoroutine(MoveBirdUp());
	}

	IEnumerator MoveBirdUp() {
		float timer = 0f;
		audioHelperBirdScript.youDidItSnd();
		audioBirdPop = true;
		// Move the bird up.
		while (timer < 1) {
			timer += Time.deltaTime / moveDur;
			helperBird.transform.position = Vector3.Lerp(hiddenTrans.position, shownTrans.position, moveCurve.Evaluate(timer));
			yield return null;
		}
		helperBird.transform.position = shownTrans.position;
		timer = 0f;
		moveUp = false;
		textBubFadeScript.gameObject.SetActive(true);
		textBubFadeScript.FadeIn();
		textBubPointerFadeScript.FadeIn();
		AdjustBubSize(bubSizeA);
		// Wait for the text bubble to fully fade in.
		while (timer < textBubFadeScript.fadeDuration) {
			timer += Time.deltaTime;
			yield return null;
		}
		// Once the text bubble has appeared, fade in the congrats text.
		congratsTextCanvasGO.SetActive(true);
		congratsTextAnim.SetTrigger("PopIn");
		congratsTextFadeScript.FadeIn();
		//AUDIO BIRD HELP SOUND
		audioHelperBirdScript.birdHelpSound();
	}

	public IEnumerator SwitchTextBubbleContent() {
		float timer = 0f;
		congratsTextFadeScript.FadeOut();
		// Wait for the congratulations text to fully fade out.
		while (timer < congratsTextFadeScript.fadeDuration) {
			timer += Time.deltaTime;
			yield return null;
		}
		timer = 0f;
		congratsTextCanvasGO.SetActive(false);
		StartCoroutine(AdjustBubSize(bubSizeB));
		// Wait for the bubble size to get adjusted.
		while (timer < bubAdjustDur) {
			timer += Time.deltaTime;
			yield return null;
		}
		eggCounterCanvasGO.SetActive(true);
		counterCGFadeScript.FadeIn();
		audioHelperBirdScript.birdHelpSound();
	}

	public IEnumerator SetupEndLevelButton() {
		float timer = 0f;
		counterCGFadeScript.FadeOut();
		while (timer < counterCGFadeScript.fadeDuration) {
			timer += Time.deltaTime;
			yield return null;
		}
		timer = 0f;
		eggCounterCanvasGO.SetActive(false);
		StartCoroutine(AdjustBubSize(bubSizeC));
		while (timer < bubAdjustDur) {
			timer += Time.deltaTime;
			yield return null;
		}
		EndLvlButtCanvasGO.SetActive(true);
		backBtnCGFadeScript.FadeIn();
		audioHelperBirdScript.birdHelpSound();
	}

	IEnumerator AdjustBubSize(float targetBubSize) {
		if (curBubSize == 0) {
			curBubSize = textBubSpriteRend.size.x;
		}
		prevBubSize = curBubSize;
		newBubSize = targetBubSize;
		while (bubLerp < 1f) {
			bubLerp += Time.deltaTime / bubAdjustDur;
			curBubSize = Mathf.Lerp(prevBubSize, newBubSize, bubLerp);
			textBubSpriteRend.size = new Vector2(curBubSize, textBubSpriteRend.size.y);
			yield return null;
		}
		bubLerp = 0f;
		curBubSize = newBubSize;
	}
}
