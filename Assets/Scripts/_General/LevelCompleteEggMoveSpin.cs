using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCompleteEggMoveSpin : MonoBehaviour {
	[Header ("Settings")]
	public float spinSpeed;
	public float moveDuration;
	public bool amIGolden;
	public int myGlowValue;
	public AnimationCurve animCurve;
	public ParticleSystem trailFX, arrivalFX;
	public Transform endTrans;
	[Header ("References")]
	public LevelCompEggCounter levelCompEggCounterScript;
	public LevelCompleteEggBag levelCompleteEggbagScript;
	public FadeInOutSprite myFadeScript;
	public AudioSceneGeneral audioSceneGenScript;
	[Header ("Info")]
	private Vector3 startPos;
	private float lerp, mySpawnDelay, spawnTimer;
	private bool startEggMove, moveEgg;
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
			if (spawnTimer > mySpawnDelay && !moveEgg) {
				myFadeScript.FadeIn();
				moveEgg = true;
				endTrans = levelCompleteEggbagScript.eggBags[levelCompleteEggbagScript.levelsCompleted].gameObject.transform;
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
					myFadeScript.FadeOut();
					arrivalFX.Play(true);
					trailFX.Stop(true);
					moveEgg = false;
					startEggMove = false;
					//levelCompleteBagGlowScript.CalculateNewAlpha(myGlowValue);
					// if (amIGolden) {
					// 	levelCompleteEggbagScript.MakeNewBagFadeIn();
					// }
				}
			}
		}
	}

	public void StartEggMovement(float spawnDelay) {
		spinDir = Random.Range(0, 2) * 2 - 1;
		startEggMove = true;
		mySpawnDelay = spawnDelay;
	} 

	public void GetReferences() {
		myFadeScript = this.GetComponent<FadeInOutSprite>();
	}
}
