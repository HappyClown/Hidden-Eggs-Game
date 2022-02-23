using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCompleteEggMoveSpin : MonoBehaviour {
	[Header ("Settings")]
	public float spinSpeed;
	public float moveDuration, startMove, becomeWhite, becomePlain, startShake;
	public float glowToMax;
	public float bagExplodeDelay;
	public bool amIGolden, amIFirst;
	public int myGlowValue;
	public AnimationCurve animCurve;
	public Transform endTrans;
	[Header ("References")]
	public LevelCompEggCounter levelCompEggCounterScript;
	public LevelCompleteEggBag levelCompleteEggbagScript;
	public LevelCompleteEggSpawner lvlCompEggSpawnScript;
	public IncreasePartSysSimulationSpeed FXSpeedScript;
	public RotateFX rotateFXScript;
	public FadeInOutSprite myFadeScript, myGlowFadeScript, plainEggFadeScript;
	public SpriteRenderer mySprite, whiteOverlaySprite;
	public AudioSceneGeneral audioSceneGenScript;
	public ParticleSystem trailFX, arrivalFX, spawnFX;
	public AudioLevelCompleteAnim audioLevelCompleteScript;
	public Animator eggAnim;
	[Header ("Info")]
	private Vector3 startPos;
	private float lerp, mySpawnDelay, spawnTimer, moveDelay, whiteDelay, plainDelay, shakeDelay;
	private float glowMaxDelay;
	private bool startEggMove, showEgg, white, plain, glowOut, shakeStarted;
	private bool glowMax;
	public bool moveEgg;
	private int spinDir = 1;
	private float explodeTimer;
	private bool bagExplosionWait;
	public bool audioGlow = false;
	public bool audioMoveToBag = false;

	void Start () {
		startPos = this.transform.position;
		if (!audioSceneGenScript) {
			audioSceneGenScript = GameObject.Find("Audio").GetComponent<AudioSceneGeneral>();
		}
		if (!audioLevelCompleteScript) {audioLevelCompleteScript = GameObject.Find("Audio").GetComponent<AudioLevelCompleteAnim>();}
	}

	void Update () {
		if (startEggMove) {
			spawnTimer += Time.deltaTime;
			this.transform.Rotate(Vector3.forward * spinDir * (spinSpeed * Time.deltaTime));
			if (spawnTimer > mySpawnDelay && !showEgg) {
				// AUDIO - EGG APPEARS!
				audioLevelCompleteScript.circleEggsSoloSnd();
				myFadeScript.FadeIn();
				myGlowFadeScript.FadeIn();
				spawnFX.gameObject.SetActive(true);
				spawnFX.Play();
				showEgg = true;
				endTrans = levelCompleteEggbagScript.curEggbagFade.gameObject.transform;
			}
			if (showEgg) {
				// Glow from current alpha value to 1f (maximum).
				if (spawnTimer >= glowMaxDelay && !glowMax) {
					myGlowFadeScript.maxAlpha = 1f;
					myGlowFadeScript.FadeIn(myGlowFadeScript.sprite.color.a);
					glowMax = true;
					// AUDIO - EGG APPEARS!
					audioLevelCompleteScript.circleEggsSoloGoldSnd();
				}
				// Scene egg fades out.
				if (spawnTimer >= whiteDelay && !white) {
					myFadeScript.FadeOut();
					if (amIFirst) {
						rotateFXScript.RotatePlayFX();
					}
					white = true;
					// AUDIO - WHITE EGGS APPEARS!
					audioLevelCompleteScript.circleEggsGlowSnd(); //nicer sound but doesnt help the gitching??
				}
				// Plain egg fades in.
				if (spawnTimer >= plainDelay && !plain) {
					myGlowFadeScript.FadeOut(0.25f);
					plainEggFadeScript.FadeIn();
					plain = true;
					// AUDIO - PLAIN EGGS APPEARS!
					audioLevelCompleteScript.circleEggsSoloPlainSnd();
				}
				// Shake anim.
				if (spawnTimer >= shakeDelay && !shakeStarted) {
					shakeStarted = true;
				}
				// Move egg.
				if (spawnTimer >= moveDelay && !moveEgg) {
					if (amIFirst) {
						rotateFXScript.RotatePlayFX(lvlCompEggSpawnScript.allEggToBagDuration);
					}
					moveEgg = true;
					//AUDIO EGGS  IN BAG MOUVEMENT?
					if(!audioMoveToBag){
						audioLevelCompleteScript.eggsMoveInBagSnd();
						audioMoveToBag = true;
					}
				}
				if (moveEgg) {
					if (!trailFX.isPlaying) {
						trailFX.gameObject.SetActive(true);
						trailFX.Play(true);
					}
					lerp += Time.deltaTime / moveDuration;
					this.transform.position = Vector3.Lerp(startPos, endTrans.position, animCurve.Evaluate(lerp));
					if (lerp >= 1) {
						levelCompEggCounterScript.eggAmnt++;
						// AUDIO - EGG REACHES BAG!						
						audioLevelCompleteScript.eggsCounterSnd();
						plainEggFadeScript.FadeOut();
						//myGlowFadeScript.FadeOut();
						myGlowFadeScript.gameObject.SetActive(false);
						arrivalFX.gameObject.SetActive(true);
						arrivalFX.Play(true);
						trailFX.Stop(true);
						trailFX.gameObject.SetActive(false);
						showEgg = false;
						startEggMove = false;
						spawnTimer = 0f;
						levelCompleteEggbagScript.bagAnim.SetTrigger("Scale");
						if (amIFirst) {
							levelCompleteEggbagScript.StartCurrentBagGlow();
							FXSpeedScript.myPartSys.Play();
						}
						if (amIGolden) {
							bagExplosionWait = true;
							FXSpeedScript.IncreaseSimulationSpeed();
							// //AUDIO particules in bag
							audioLevelCompleteScript.particulesInBagSnd();
						}
					}
				}
			}
		}
		if (bagExplosionWait) {
			bagExplodeDelay -= Time.deltaTime;
			if (bagExplodeDelay <= 0f) {
				levelCompleteEggbagScript.bagAnim.SetTrigger("Explode");
				bagExplosionWait = false;
			}
		}
	}

	public void StartEggMovement(float spawnDelay, float toBagDelay) {
		spinDir = Random.Range(0, 2) * 2 - 1;
		startEggMove = true;
		mySpawnDelay = spawnDelay;
		whiteDelay = becomeWhite + mySpawnDelay;
		glowMaxDelay = glowToMax + mySpawnDelay;
		plainDelay = becomePlain + mySpawnDelay;
		shakeDelay = startShake + toBagDelay;
		moveDelay = startMove + toBagDelay;
		eggAnim.gameObject.SetActive(true);
	} 

	public void GetReferences() {
		myFadeScript = this.GetComponent<FadeInOutSprite>();
		myGlowFadeScript = this.transform.Find("SmallEggGlow").GetComponent<FadeInOutSprite>();
		eggAnim = this.transform.parent.GetComponent<Animator>();
		SpriteMask sMask = this.GetComponent<SpriteMask>();
		sMask.sprite = this.GetComponent<SpriteRenderer>().sprite;
		whiteOverlaySprite = this.transform.Find("WhiteOverlay").GetComponent<SpriteRenderer>();
		plainEggFadeScript = this.transform.Find("PlainEgg").GetComponent<FadeInOutSprite>();
	}
}