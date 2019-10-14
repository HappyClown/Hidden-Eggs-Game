using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryOneEgg : MonoBehaviour {
	[Header ("Rotate")]
	public bool rotate;
	public float halfRotDur;
	[Header ("Fly To Middle")]
	public float flyDur;
	public AnimationCurve flyAnimCurveX, flyAnimCurveY, speedCurve;
	public Transform flyEndTrans;
	private bool flyOutOfTime;
	private float lerpValue, iniX, iniY, maxX, maxY, newX, newY;
	public Vector3 endScale;
	private Vector3 startScale;
	[Header ("Tap Icon")]
	public bool scaleTapIcon;
	public GameObject tapIcon;
	public float scaleDur;
	public Vector3 smallScale, bigScale;
	private float scaleTimer;
	private bool scaleUp = false, scaleDown = true;
	[Header ("General")]
	public GameObject theOneEgg;
	public GameObject behindTheOneEgg;
	public Animator oneEggAnim;
	public ParticleSystem eggClickFX, eggTrailFX, eggToMidTrailFX;
	public Animator eggSealAnim;
	private bool eggClickFXPlayed;
	[Header ("References")]
	public StoryTimeMotions storyTimeMoScript;
	public FadeInOutSprite tapIconFadeScript;
	public FadeInOutSprite oneEggShadowFadeScript;
	public FadeInOutSprite behindTheOneEggFadeScript, theOneEggFadeScript;
	
	void Update () {
		if (scaleTapIcon) {
			ScaleTapIcon();
		}
		if (rotate) {
			Rotate();
		}
		if (flyOutOfTime) {
			FlyOutOfTime();
		}
	}
	// Scale the Tap Icon up and down.
	void ScaleTapIcon() {
		scaleTimer += Time.deltaTime / scaleDur;
		if (scaleDown) {
			tapIcon.transform.localScale = Vector3.Lerp(bigScale, smallScale, scaleTimer);
		}
		if (scaleUp) {
			tapIcon.transform.localScale = Vector3.Lerp(smallScale, bigScale, scaleTimer);
		}
		if (scaleTimer >= 1) {
			if (scaleDown) {
				scaleDown = false;
				scaleUp = true;
			}
			else if (scaleUp) {
				scaleDown = true;
				scaleUp = false;
			}
			scaleTimer = 0f;
		}
	}
	// The OneEgg rotates while floating in the air. (TheOneEgg #010)
	void Rotate() {
		theOneEgg.transform.RotateAround(theOneEgg.transform.position, Vector3.up, 180 * (Time.deltaTime / halfRotDur));
	}
	// Start the egg tap animation.
	public void EggTap() {
		oneEggAnim.SetTrigger("EggPop");
		eggClickFX.Play(true);
		// AUDIO - EGG CLICKED/TAPPED!
	}
	// The OneEgg flies out from Time to the middle of the hub. (TheQuest #011)
	void FlyOutOfTime() {
		lerpValue += Time.deltaTime / flyDur;
		float speedLerpValue = speedCurve.Evaluate(lerpValue);
		newX = Mathf.Lerp(iniX, maxX, flyAnimCurveX.Evaluate(speedLerpValue));
		newY = Mathf.Lerp(iniY, maxY, flyAnimCurveY.Evaluate(speedLerpValue));
		theOneEgg.transform.position = new Vector3(newX, newY, theOneEgg.transform.position.z);
		//theOneEgg.transform.localScale = Vector3.Lerp(startScale, endScale, flyAnimCurveY.Evaluate(speedLerpValue));
		if (speedLerpValue >= 1f) {
			speedLerpValue = 0f;
			flyOutOfTime = false;
			behindTheOneEggFadeScript.FadeOut();
			eggToMidTrailFX.Stop();
			eggSealAnim.SetTrigger("EggSeal");
		}
		if (speedLerpValue >= 0.5f && oneEggShadowFadeScript.shown) {
			oneEggShadowFadeScript.FadeOut();
		}
	}
	// Variable setup for the OneEgg flying out from Time to the middle of the hub. (TheQuest #011)
	public void SetupFlyOutOfTime() {
		theOneEgg.SetActive(true);
		oneEggAnim.enabled = false;
		oneEggAnim.transform.localScale = new Vector3(1,1,1);
		theOneEgg.transform.eulerAngles = Vector3.zero;
		iniX = storyTimeMoScript.currentTime.transform.position.x;
		iniY = storyTimeMoScript.currentTime.transform.position.y;
		theOneEgg.transform.position = new Vector3(iniX, iniY, theOneEgg.transform.position.z);
		maxX = flyEndTrans.position.x;
		maxY = flyEndTrans.position.y;
		startScale = theOneEgg.transform.localScale;
		eggToMidTrailFX.Play();
		flyOutOfTime = true;
	}
}
