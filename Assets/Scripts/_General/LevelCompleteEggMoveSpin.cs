using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCompleteEggMoveSpin : MonoBehaviour {
	[Header ("Settings")]
	public float spinSpeed;
	public float moveDuration, spinTime, becomeWhite, becomePlain;
	public bool amIGolden, amIFirst;
	public int myGlowValue;
	public AnimationCurve animCurve;
	public ParticleSystem trailFX, arrivalFX, spawnFX;
	public Transform endTrans;
	[Header ("References")]
	public LevelCompEggCounter levelCompEggCounterScript;
	public LevelCompleteEggBag levelCompleteEggbagScript;
	public FadeInOutSprite myFadeScript, myGlowFadeScript, plainEggFadeScript;
	public SpriteRenderer mySprite, whiteOverlaySprite;
	public AudioSceneGeneral audioSceneGenScript;
	[Header ("Info")]
	private Vector3 startPos;
	private float lerp, mySpawnDelay, spawnTimer, moveDelay, whiteDelay, plainDelay;
	private bool startEggMove, moveEgg, showEgg, white, plain;
	private int spinDir = 1;

	void Start () {
		startPos = this.transform.position;
		if (!audioSceneGenScript) {
			audioSceneGenScript = GameObject.Find("Audio").GetComponent<AudioSceneGeneral>();
		}
	}

	void Update () {
		if (startEggMove) {
			spawnTimer += Time.deltaTime;
			this.transform.Rotate(Vector3.forward * spinDir * (spinSpeed * Time.deltaTime));
			if (spawnTimer > mySpawnDelay && !showEgg) {
				myFadeScript.FadeIn();
				myGlowFadeScript.FadeIn();
				spawnFX.Play();
				showEgg = true;
				endTrans = levelCompleteEggbagScript.curEggbagFadeScript.gameObject.transform;
			}
			if (showEgg) {
				if (spawnTimer >= whiteDelay && !white) {
					myFadeScript.FadeOut();
					white = true;
				}
				if (spawnTimer >= plainDelay && !plain) {
					plainEggFadeScript.FadeIn();
					plain = true;
				}
				if (spawnTimer >= moveDelay && !moveEgg) {
					moveEgg = true;
				}
				if (moveEgg) {
					if (!trailFX.isPlaying) {
						trailFX.Play(true);
					}
					lerp += Time.deltaTime / moveDuration;
					this.transform.position = Vector3.Lerp(startPos, endTrans.position, animCurve.Evaluate(lerp));
					if (lerp >= 1) {
						levelCompEggCounterScript.eggAmnt++;
						audioSceneGenScript.silverEggsPanel(this.gameObject);
						//myFadeScript.FadeOut();
						plainEggFadeScript.FadeOut();
						myGlowFadeScript.FadeOut();
						arrivalFX.Play(true);
						trailFX.Stop(true);
						showEgg = false;
						startEggMove = false;
						spawnTimer = 0f;
						//levelCompleteBagGlowScript.CalculateNewAlpha(myGlowValue);
						if (amIFirst) {
							levelCompleteEggbagScript.StartCurrentBagGlow();
							levelCompleteEggbagScript.bagAnim.SetTrigger("Rise");
						}
						if (amIGolden) {
							levelCompleteEggbagScript.bagAnim.SetTrigger("Explode");
						}
					}
				}
			}
		}
	}

	public void StartEggMovement(float spawnDelay) {
		spinDir = Random.Range(0, 2) * 2 - 1;
		startEggMove = true;
		mySpawnDelay = spawnDelay;
		whiteDelay = becomeWhite + mySpawnDelay;
		plainDelay = becomePlain + mySpawnDelay;
		moveDelay = spinTime + mySpawnDelay;
	} 

	public void GetReferences() {
		myFadeScript = this.GetComponent<FadeInOutSprite>();
		myGlowFadeScript = this.transform.Find("SmallEggGlow").GetComponent<FadeInOutSprite>();
		SpriteMask sMask = this.GetComponent<SpriteMask>();
		sMask.sprite = this.GetComponent<SpriteRenderer>().sprite;
		whiteOverlaySprite = this.transform.Find("WhiteOverlay").GetComponent<SpriteRenderer>();
		plainEggFadeScript = this.transform.Find("PlainEgg").GetComponent<FadeInOutSprite>();
	}
}