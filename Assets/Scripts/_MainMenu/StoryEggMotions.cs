using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryEggMotions : MonoBehaviour {
	[Header("Out of Bag")]
	public Transform eggSpawnTrans;
	public GameObject bewilderedTime;
	public float eggFlyDuration;
	public AnimationCurve yAnimCurve;
	public float yMoveDist, minYMove, maxYMove;
	public AnimationCurve xAnimCurve;
	public float xMoveDist;

	public float minScale, maxScale;
	public float minEggAngle, maxEggAngle;

	private bool spawnInBag;
	private float lerpValue, startY, endY, startX, endX, newX, newY, newEggAngle;
	[Header("Falling")]
	public AnimationCurve toMidAnimCurve;
	public AnimationCurve toBotAnimCurve;
	public float fallDuration, fallEggScale;
	public List<Transform> fallEggSpawnTrans;
	private bool fallFromTop, fallDown, fallOffScreen;
	[Header("Hover")]
	public AnimationCurve hoverAnimCurve;
	public float hoverDuration, hoverUpHeight;
	private bool hover;
	private Vector3 iniHoverPos;
	public int hoverAmnt;
	private int hoverAmntNum;
	public float hoverAfterChangeDur;
	private float timer;
	private bool hoverBeforeFall;
	[Header ("Rotate")]
	public float minRotDur;
	public float maxRotDur;
	private float fullRotDuration;
	private bool rotate;
	[Header ("Scene Egg")]
	[Tooltip ("The time it takes after this egg starts falling for the SceneEgg to start fading in over the PlainEgg.")]
	public float fadeSceneEggTime;
	public float flashEggTime;
	public FadeInOutSprite thisEggFadeScript, sceneEggFadeScript;
	[Tooltip ("Each egg has its own Trail particle system but they all reference the same Burst particle system.")]
	public ParticleSystem trailPartSys, burstPartSys;
	public bool fadeToSceneEgg = false, eggFlashed, eggBurst;
	private float fadeSceneEggTimer;
	public SpriteColorFade spriteColorFadeScript;

	[Header ("Audio Script reference")]
	public AudioIntro audioIntroScript;
	public bool audioEggFallDown = true;
	public bool audioEggOutBag =true;

	void Start(){
		if (!audioIntroScript) {audioIntroScript = GameObject.Find("Audio").GetComponent<AudioIntro>();}
	}
	void Update () {
		if (spawnInBag) {
			OutOfBag();
		}
		if (rotate) {
			Rotate();
		}
		if (fallFromTop) {
			FallFromTop();
		}
		if (hover) {
			Hover();
		}
		if (fallDown) {
			FallDown();
		}
		if (fadeToSceneEgg) {
			FadeToSceneEgg();
		}
		if (hoverBeforeFall) {
			timer += Time.deltaTime;
			if (timer >= hoverAfterChangeDur) {
				fallOffScreen = true;
				timer = 0f;
				hoverBeforeFall = false;
			}
		}
	}

	// For the "TimeConfused" storyboard.
	public void SpawnEggInBag() {
		spawnInBag = true;
		if (bewilderedTime.transform.eulerAngles.y > 90 && bewilderedTime.transform.eulerAngles.y < 270/*  || bewilderedTime.transform.eulerAngles.y < -90 && bewilderedTime.transform.eulerAngles.y >- 180 */) {
			xMoveDist = Mathf.Abs(xMoveDist) * -1;
		}
		else {
			xMoveDist = Mathf.Abs(xMoveDist);
		}
		newEggAngle = Random.Range(minEggAngle, maxEggAngle);
		yMoveDist = Random.Range(minYMove, maxYMove);
		this.transform.localEulerAngles = new Vector3(this.transform.localEulerAngles.x, this.transform.localEulerAngles.y, newEggAngle);
		startX = eggSpawnTrans.position.x;
		endX = eggSpawnTrans.position.x + xMoveDist;
		startY = eggSpawnTrans.position.y;
		endY = eggSpawnTrans.position.y + yMoveDist;

		
		// AUDIO - EGG COMES OUT OF BAG!
		audioIntroScript.introEggDropBasketSFX();
		audioEggOutBag = false;
		
	}

	void OutOfBag() {
		if (lerpValue < 1) {
			lerpValue += Time.deltaTime / eggFlyDuration;
			newX = Mathf.LerpUnclamped(startX, endX, xAnimCurve.Evaluate(lerpValue));
			newY = Mathf.LerpUnclamped(startY, endY, yAnimCurve.Evaluate(lerpValue));

			this.transform.position = new Vector3(newX, newY, this.transform.position.z);
		}
		else {
			spawnInBag = false;
			lerpValue = 0f;
		}
	}
	// For the "EggsFalling" storyboard.
	public void SpawnEggsAtTop(Vector3 startPos, Vector3 endPos) {
		this.transform.localScale = new Vector3(fallEggScale, fallEggScale, fallEggScale);
		this.transform.position = startPos;
		fallFromTop = true;
		startY = this.transform.position.y;
		endY = endPos.y;
		rotate = true;
		fullRotDuration = Random.Range(minRotDur, maxRotDur);
		int newRot = Random.Range(0, 360);
		this.transform.localEulerAngles = new Vector3(this.transform.localEulerAngles.x, this.transform.localEulerAngles.y, newRot);
		lerpValue = 0f;
		fadeSceneEggTimer = 0f;
		// fadeToSceneEgg = true;
	}

	void Rotate() {
		this.transform.RotateAround(this.transform.position, Vector3.back, 360 * (Time.deltaTime / fullRotDuration));
	}

	void FallFromTop() {
		if (lerpValue < 1) {
			lerpValue += Time.deltaTime / fallDuration;
			newY = Mathf.Lerp(startY, endY, toMidAnimCurve.Evaluate(lerpValue));
			this.transform.position = new Vector3(this.transform.position.x, newY, this.transform.position.z);
		}
		else {
			fallFromTop = false;
			lerpValue = 0f;
			hover = true;
			iniHoverPos = this.transform.position;
		}
	}
	// Fade transition from plain egg to scene egg.
	// void FadeToSceneEgg() {
	// 	// At which point after the egg starts falling does the SceneEgg start fading in over the plain egg.
	// 	fadeSceneEggTimer += Time.deltaTime;
	// 	if (fadeSceneEggTimer >= fadeSceneEggTime) {
	// 		sceneEggFadeScript.FadeIn();
	// 		thisEggFadeScript.FadeOut();
	// 		fadeToSceneEgg = false;
	// 	}
	// 	// Start the egg trail FX
	// 	trailPartSys.Play();
	// }

	// White flash transition from plain egg to scene egg.
	void FadeToSceneEgg() {
		// At which point after the egg starts falling does the egg flash white.
		fadeSceneEggTimer += Time.deltaTime;
		if (fadeSceneEggTimer >= flashEggTime && !eggFlashed) {
			spriteColorFadeScript.FlashIn();
			eggFlashed = true;
		}
		if (fadeSceneEggTimer >= spriteColorFadeScript.flashInDur + spriteColorFadeScript.autoFlashOutDelay + flashEggTime && !eggBurst) {
			sceneEggFadeScript.ResetAlpha(1f);
			// Move and play the egg transform FX.
			burstPartSys.transform.position = this.transform.position;
			burstPartSys.Play();
			// AUDIO - PLAIN EGG BECOMES SCENE EGG (Particle effects included)!

			//AUDIO - EGG transforming
			audioIntroScript.introEggPopTransformSFX();

			eggBurst = true;
			// Start the egg trail FX.
			trailPartSys.Play();
		}
		if (fadeSceneEggTimer >= spriteColorFadeScript.flashInDur + spriteColorFadeScript.autoFlashOutDelay + spriteColorFadeScript.flashOutDur + flashEggTime) {
			thisEggFadeScript.ResetAlpha(0f);
			fadeSceneEggTimer = 0f;
			fadeToSceneEgg = false;
			eggFlashed = false;
			//fallOffScreen = true;
			hoverBeforeFall = true;
		}
	}

	void Hover() {
		if (lerpValue < 1) {
			lerpValue += Time.deltaTime / hoverDuration;
			newY = hoverAnimCurve.Evaluate(lerpValue) * hoverUpHeight;
			this.transform.position = new Vector3(this.transform.position.x, iniHoverPos.y + newY, this.transform.position.z);
			// The egg starts going down from its top hover position. Wether this happens at 0.5 of the lerp or otherwise depends on the AnimationCurves peak.
			if (/* lerpValue > 0.5f &&  */fallOffScreen/* hoverAmntNum >= hoverAmnt */) {
				hover = false;
				fallDown = true;
				lerpValue = 0f;
				hoverAmntNum = 0;
				fallOffScreen = false;
				startY = this.transform.position.y;
				endY = startY - 20f; // Height of the screen to make sure all the eggs go off screen.
			}
		}
		else {
			lerpValue = 0f;
			hoverAmntNum++;
		}
	}

	void FallDown() {
		if (lerpValue < 1) {
			lerpValue += Time.deltaTime / fallDuration;
			newY = Mathf.Lerp(startY, endY, toBotAnimCurve.Evaluate(lerpValue));
			this.transform.position = new Vector3(this.transform.position.x, newY, this.transform.position.z);
		}
		else {
			fallDown = false;
			lerpValue = 0f;
			trailPartSys.Stop();
		}
	}

	public void FallOffScreen() {
		fallOffScreen = true;
	}

	public void Reset() {
		spawnInBag = rotate = fallFromTop = hover = fallDown = false;
		this.transform.position = fallEggSpawnTrans[0].position;
		lerpValue = 0f;
		if (trailPartSys.isPlaying) {
			trailPartSys.Clear();
			trailPartSys.Stop();
		}
	}
}
