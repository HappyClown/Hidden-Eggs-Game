﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PuzzleComplete : MonoBehaviour {
	[Header ("Left")]
	public GameObject leftPuzzPiece;
	public Transform leftStartPos, leftEndPos;
	public ParticleSystem leftTrailFX;
	[Header ("Right")]
	public GameObject rightPuzzPiece;
	public Transform rightStartPos, rightEndPos;
	public ParticleSystem rightTrailFX;
	[Header ("Sequence")]
	public bool endSeq;
	public float appear, fx, stopTrail, end;
	private bool appearB, fxB, stopTrailB, endB;
	private float seqTimer;
	[Header ("General")]
	public float revealDur;
	public AnimationCurve revealCurve;
	public bool reveal, reset;
	private float revTimer;
	[Header ("References")]
	public AudioSceneMarketPuzzle audioSceneMarketPuzzScript;
	public FadeInOutSprite leftPieceFadeScript, rightPieceFadeScript;
	public TMPTextColorFade textColorScript;
	public TMPTextFall textFallScript;

	void Update () {
		if (endSeq) {
			seqTimer += Time.deltaTime;
			if (seqTimer >= appear && !appearB) {
				//FadeInPieces(); 
				FadeInText();
				appearB = true;
			}
			if (seqTimer >= fx && !fxB) {
				PlayFXs();
				reveal = true;
				fxB = true;
			}
			if (seqTimer >= stopTrail && !stopTrailB) {
				StopTrailFX();
				stopTrailB = true;
			}
			if (seqTimer >= end && !endB) {
				endSeq = false;
				EndPuzzle();
				endB = true;
			}
		}

		if (reveal) {
			Revealing();
		}

		if (reset) {
			Reset();
		}
	}

	void Revealing() {
		revTimer += Time.deltaTime / revealDur;
		leftPuzzPiece.transform.position = Vector3.Lerp(leftStartPos.position, leftEndPos.position, revealCurve.Evaluate(revTimer));
		rightPuzzPiece.transform.position = Vector3.Lerp(rightStartPos.position, rightEndPos.position, revealCurve.Evaluate(revTimer));
		if (revTimer >= 1f) {
			reveal = false;
			revTimer = 0f;
		}
	}

	void FadeInPieces() {
		leftPieceFadeScript.FadeIn();
		rightPieceFadeScript.FadeIn();
	}

	void PlayFXs() {
		leftTrailFX.Play();
		rightTrailFX.Play();
		//reveal = true;
	}

	void StopTrailFX() {
		leftTrailFX.Stop();
		rightTrailFX.Stop();
	}

	void EndPuzzle() {
		//audioSceneMarketPuzzScript.StopSceneMusic();
		//audioSceneMarketPuzzScript.PlayTransitionMusic();
		//SceneFade.SwitchScene(GlobalVariables.globVarScript.parkName);
	}

	void FadeInText() {
		//textColorScript.startFadeIn = true;
		textFallScript.fallOn = true;
	}

	void Reset() {
		textColorScript.Reset();
		seqTimer = 0f;
		appearB = false;
		fxB = false;
		stopTrailB = false;
		endB = false;
		reset = false;
	}
}