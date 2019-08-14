using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCompleteEggMove : MonoBehaviour {
	[Header ("Settings")]
	public float duration;
	public bool amIGolden;
	public AnimationCurve animCurve;
	public ParticleSystem trailFX, arrivalFX;
	public Transform endTrans;
	[Header ("References")]
	public Animator eggTypeAnim;
	public Animator eggBagAnim;
	public LevelCompEggCounter levelCompEggCounterScript;
	public LevelCompleteEggBag levelCompleteEggbagScript;
	public FadeInOutSprite myFadeScript;
	public AudioLevelCompleteAnim audioLevelCompleteScript;

	[Header ("Info")]
	private Vector3 startPos;
	private float lerp, mySpawnDelay, spawnTimer;
	private bool startEggMove, moveEgg;

	void Start () {
		startPos = this.transform.position;

	if (!audioLevelCompleteScript) {audioLevelCompleteScript = GameObject.Find("Audio").GetComponent<AudioLevelCompleteAnim>();}
	}

	void Update () {
		if (startEggMove) {
			spawnTimer += Time.deltaTime;
			if (spawnTimer > mySpawnDelay && !moveEgg) {
				myFadeScript.FadeIn();
				eggTypeAnim.SetTrigger("Scale");
				moveEgg = true;
				//endTrans = levelCompleteEggbagScript.eggBags[levelCompleteEggbagScript.levelsCompleted].gameObject.transform;
			}
			if (moveEgg) {
				if (!trailFX.isPlaying) {
					trailFX.Play(true);
				}
				lerp += Time.deltaTime / duration;
				this.transform.position = Vector3.Lerp(startPos, endTrans.position, animCurve.Evaluate(lerp));
				if (lerp >= 1) {
					eggBagAnim.SetTrigger("Scale");
					levelCompEggCounterScript.eggAmnt++;
					if (levelCompEggCounterScript.eggAmnt == 1) {
						levelCompleteEggbagScript.MakeNewBagFadeIn();
					}
					//AUDIO SOUND EGGS COUNTER
					audioLevelCompleteScript.eggsCounterSnd();

					myFadeScript.FadeOut();
					arrivalFX.Play(true);
					trailFX.Stop(true);
					moveEgg = false;
					startEggMove = false;
					// if (amIGolden) {
					// 	levelCompleteEggbagScript.MakeNewBagAppear();
					// }
				}
			}
		}
	}

	public void StartEggMovement(float spawnDelay) {
		startEggMove = true;
		mySpawnDelay = spawnDelay;
	} 
}
