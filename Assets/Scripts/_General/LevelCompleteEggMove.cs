using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCompleteEggMove : MonoBehaviour {
	public bool amIGolden;
	private bool startEggMove;
	public Transform endTrans;
	private Vector3 startPos;
	private float lerp, mySpawnDelay, spawnTimer;
	public float duration;
	public AnimationCurve animCurve;
	public ParticleSystem trailFX, arrivalFX;
	public LevelCompEggCounter levelCompEggCounterScript;
	public LevelCompleteEggBag levelCompleteEggbagScript;
	public FadeInOutSprite myFadeScript;
	public AudioSceneGeneral audioSceneGenScript;

	void Start () {
		startPos = this.transform.position;
		audioSceneGenScript = GameObject.Find("Audio").GetComponent<AudioSceneGeneral>();
	}

	void Update () {
		if (startEggMove) {
			spawnTimer += Time.deltaTime;
			if (spawnTimer > mySpawnDelay && myFadeScript.hidden) {
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
					audioSceneGenScript.silverEggsPanel(this.gameObject);
					myFadeScript.FadeOut();
					arrivalFX.Play(true);
					trailFX.Stop(true);
					startEggMove = false;
					if (amIGolden) {
						levelCompleteEggbagScript.MakeNewBagAppear();
					}
				}
			}
		}
	}

	public void StartEggMovement(float spawnDelay) {
		startEggMove = true;
		mySpawnDelay = spawnDelay;
	} 
}
