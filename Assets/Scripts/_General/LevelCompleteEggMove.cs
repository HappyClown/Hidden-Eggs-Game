using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCompleteEggMove : MonoBehaviour {
	private bool startEggMove;
	public Transform endTrans;
	private Vector3 startPos;
	private float lerp, mySpawnDelay, spawnTimer;
	public float duration;
	public AnimationCurve animCurve;
	public ParticleSystem trailFX, arrivalFX;
	public LevelCompEggCounter levelCompEggCounterScript;
	public FadeInOutSprite myFadeScript;

	void Start () {
		startPos = this.transform.position;
	}

	void Update () {
		if (startEggMove) {
			spawnTimer += Time.deltaTime;
			if (spawnTimer > mySpawnDelay) {
				myFadeScript.FadeIn();
			}
			if (myFadeScript.shown) {
				if (!trailFX.isPlaying) {
					trailFX.Play(true);
				}
				lerp += Time.deltaTime / duration;
				this.transform.position = Vector3.Lerp(startPos, endTrans.position, animCurve.Evaluate(lerp));
				if (lerp >= 1) {
					levelCompEggCounterScript.eggAmnt++;
					myFadeScript.FadeOut();
					arrivalFX.Play(true);
					trailFX.Stop(true);
					startEggMove = false;
				}
			}
		}
	}

	public void StartEggMovement(float spawnDelay) {
		startEggMove = true;
		mySpawnDelay = spawnDelay;
	} 
}
