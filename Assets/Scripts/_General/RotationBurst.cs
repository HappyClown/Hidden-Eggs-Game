using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationBurst : MonoBehaviour {
	[Header("Settings")]
	public bool clockWise;
	public float rotSpeed;
	public AnimationCurve rotCurve;
	public float minCoolDown, maxCoolDown;
	public float minRotDur, maxRotDur;
	public int minRotAmnt, maxRotAmnt;
	public bool firstCoolDown;
	public float firstCoolDownDur;
	[Header("References")]
	public List<ParticleSystem> tipFXs;
	public List<TrailRenderer> tipTrails;
	[Header("Info")]
	public bool rotationOn;
	public bool coolDownOn;
	private float curveLerp, timer;
	private float curRotSpeed, curRot; // curRotDur = spin duration!
	private float direction = 1f;
	private float iniZRot;

	private float angleLerp;
	private float targetRot;
	public int curRotAmnt;
	public float curCoolDown;
	public float curRotDur;

	public AudioRiddleSolvedAnim audioRiddleSolvedAnimScript;
	public bool rotationSoundOn = false;

	void Awake(){
		audioRiddleSolvedAnimScript = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioRiddleSolvedAnim>();
	}
	public void StartRotation() {
		//curRotDur = Random.Range(minRotDur, maxRotDur);
		//curCoolDown = Random.Range(minCoolDown, maxCoolDown);
		//curRotAmnt = Random.Range(minRotAmnt, maxRotAmnt);
		//curRotDur *= curRotAmnt;
		if (clockWise) {
			direction = 1f;
		}
		else {
			direction = -1f;
		}
		iniZRot = this.transform.eulerAngles.z;
		curRot = iniZRot;
		if (clockWise) {
			targetRot = ((curRotAmnt * 360) - iniZRot) * direction;
		}
		else {
			targetRot = ((curRotAmnt * 360) + iniZRot) * direction;
		}
		firstCoolDown = true;

		//sound one shot/rotation
		audioRiddleSolvedAnimScript.spiningStarsSnd(); //sound
	}
	
	void Update () {
		if (rotationOn) {
			if(!rotationSoundOn){
				//sound one shot/rotation
				audioRiddleSolvedAnimScript.spiningStarsSnd(); //sound
				rotationSoundOn = true;
			}
			curveLerp += Time.deltaTime / curRotDur;
			//curRotSpeed = rotCurve.Evaluate(curveLerp) * rotSpeed * direction;
			//curRot += curRotSpeed;
			
			angleLerp = Mathf.Lerp(iniZRot, targetRot, rotCurve.Evaluate(curveLerp));
			// if (curRot >= 360) {
			// 	curRot = curRot - 360;
			// }
			this.transform.eulerAngles = new Vector3(this.transform.eulerAngles.x, this.transform.eulerAngles.y, angleLerp);
			
			timer += Time.deltaTime;
			if (timer >= curRotDur) {
				timer = 0f;
				// curRot = iniZRot;
				curveLerp = 0f;
				//curRotSpeed = 0f;
				rotationOn = false;
				//curCoolDown = Random.Range(minCoolDown, maxCoolDown);
				coolDownOn = true;
				//StopTipFXs();
			}
		}
		if (coolDownOn) {
			timer += Time.deltaTime;
			if (timer >= curCoolDown) {
				timer = 0f;
				angleLerp = iniZRot;
				coolDownOn = false;
				//curRotDur = Random.Range(minRotDur, maxRotDur);
				//curRotAmnt = Random.Range(minRotAmnt, maxRotAmnt);
				//curRotDur *= curRotAmnt;
				if (clockWise) {
					targetRot = (curRotAmnt * 360) + iniZRot;
				}
				else {
					targetRot = ((curRotAmnt * 360) * direction) + iniZRot;
				}
				//StartTipFXs();
				rotationOn = true;

				//reset sound
				rotationSoundOn = false;
			}
		}
		if (firstCoolDown) {
			firstCoolDownDur -= Time.deltaTime;
			if (firstCoolDownDur <= 0f) {
				if (clockWise) {
					targetRot = (curRotAmnt * 360) + iniZRot;
				}
				else {
					targetRot = ((curRotAmnt * 360) * direction) + iniZRot;
				}
				angleLerp = iniZRot;
				//StartTipFXs();
				foreach(TrailRenderer tipTrail in tipTrails)
				{
					tipTrail.enabled = true;
				}
				rotationOn = true;
				firstCoolDown = false;

				//reset sound
				rotationSoundOn = false;
			}
		}
	}

	void StartTipFXs () {
		foreach(ParticleSystem tipFX in tipFXs)
		{
			tipFX.Play();
		}
	}
	void StopTipFXs () {
		foreach(ParticleSystem tipFX in tipFXs)
		{
			tipFX.Stop();
		}
	}
}
