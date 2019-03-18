using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SilverEggSequence : MonoBehaviour 
{
	public Collider2D myCol;
	public ParticleSystem shimmerFX;
	public SilverEggShimmerFXScaling silEggShimScript;

	[Header("Move & Scale Up")]
	public float moveDuration = 1f;
	public float startMoveDelay;
	public Transform endPos;
	public Vector3 endScale;
	public AnimationCurve moveAnimCurve;
	public float newYMagnitude;
	public AnimationCurve yAnimCurve;
	public ParticleSystem trailFX;
	private bool startSeq;
	private Vector3 startPos;
	private Vector3 startScale;

	[Header("Hover")]
	public float hoverDuration = 1f;
	[Tooltip("Slowly accelerates up to the regular hover cycle speed. Has to be higher then hoverDuration; acceleration time(seconds) = hoverCurDur - hoverDuration.")]
	public float hoverCurDur;
	public float startHoverDelay;
	public float hoverYMult;
	public AnimationCurve hoverAnimCurve;
	public bool hoverSeq;
	public float iniHovCurDur;
	public float iniStartHoverDelay;

	private bool hoverUp = true;
	private float lerpTime;
	
	public AudioScenePuzzleGeneric audioScenePuzzScript;
	public inputDetector inputDetScript;
	public SilverEggsManager silverEggsManScript;

	//public Transform hoverTo; // For hover with lerp.
	
	void Awake() {
		iniHovCurDur = hoverCurDur;
		iniStartHoverDelay = startHoverDelay;
		if (!audioScenePuzzScript) {
			audioScenePuzzScript =  GameObject.FindGameObjectWithTag ("Audio").GetComponent<AudioScenePuzzleGeneric>();
		}
	}

	void Update () {
		if (inputDetScript.Tapped) {
			SkipSequence();
		}

		if (startSeq) {
			if (startMoveDelay > 0f) { startMoveDelay -= Time.deltaTime; }
			else {
				lerpTime += Time.deltaTime / moveDuration;
				float animCurveTime = moveAnimCurve.Evaluate(lerpTime);
				float newY = yAnimCurve.Evaluate(lerpTime) * newYMagnitude;

				this.transform.position = Vector3.Lerp(startPos, endPos.position, animCurveTime);
				this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + newY, this.transform.position.z);
				this.transform.localScale = Vector3.Lerp(startScale, endScale, animCurveTime);

				if (lerpTime >= 1f) {
					//end - play fx, enable collider, etc
					startSeq = false;
					hoverSeq = true;
					hoverUp = true;
					lerpTime = 0f;
					trailFX.Stop();
					startPos = this.transform.position;
					myCol.enabled = true;
				}
			}
		}

		if (hoverSeq) {
			////// HOVER WITH LERP //////
			// if (startHoverDelay > 0f) { startHoverDelay -= Time.deltaTime; }
			// else
			// {
			// 	lerpTime += Time.deltaTime / hoverDuration;
			// 	float animCurveTime = hoverAnimCurve.Evaluate(lerpTime);

			// 	if (hoverUp) { this.transform.position = Vector3.Lerp(endPos.position, hoverTo.position, animCurveTime); }
			// 	else { this.transform.position = Vector3.Lerp(hoverTo.position, endPos.position, animCurveTime); }

			// 	if (lerpTime >= 1f)
			// 	{
			// 		if (hoverUp) { hoverUp = false; } else { hoverUp = true; }
			// 		lerpTime = 0f;
			// 	}
			// }
			////// HOVER WITH ANIMATION CURVE //////
			if (startHoverDelay > 0f) { startHoverDelay -= Time.deltaTime; }
			else {
				if(hoverCurDur > hoverDuration) { hoverCurDur -= Time.deltaTime; } else { hoverCurDur = hoverDuration; } // Gradually increase the hover speed.
				lerpTime += Time.deltaTime / hoverCurDur; // Time in second for one up or down curve.
				float hoverY = hoverAnimCurve.Evaluate(lerpTime) * hoverYMult;

				this.transform.position = new Vector3(startPos.x, startPos.y + hoverY, startPos.z); 
			}
		}
	}

	public void ShimmerFade() { // In SilverEgg Pop anim 
		shimmerFX.gameObject.transform.parent = null;
		var shimmerFXEm = shimmerFX.emission;
		shimmerFXEm.enabled = false;
		silEggShimScript.Refresh();
	}

	public void StartSequence() {
		startSeq = true;
		startPos = this.transform.localPosition;
		startScale = this.transform.localScale;

		audioScenePuzzScript.SilverEggTrailSFX();
		//trailFX.Play();
		//shimmerFX.Play();
	}

	public void ResetHover() {
		hoverCurDur = iniHovCurDur;
		startHoverDelay = iniStartHoverDelay;
		hoverSeq = false;
		lerpTime = 0f;
	}

	public void SkipSequence() {
		if (startSeq) {
			this.transform.position = endPos.position;
			this.transform.localScale = endScale;
			startSeq = false;
			hoverSeq = true;
			hoverUp = true;
			lerpTime = 0f;
			trailFX.Stop();
			startPos = this.transform.position;
			myCol.enabled = true;
			silverEggsManScript.skippedSeq = true;
			//Debug.Log(silverEggsManScript.skippedSeq);
		}
	}
}
