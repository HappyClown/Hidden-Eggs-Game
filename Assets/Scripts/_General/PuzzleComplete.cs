using System.Collections;
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
	public bool reveal, reset, masksFromMid;
	public string sceneName;
	private float revTimer;
	[Header ("References")]
	public AudioScenePuzzleGeneric audioScenePuzzGeneScript;
	public FadeInOutSprite leftPieceFadeScript, rightPieceFadeScript;
	public TMPTextColorFade textColorScript;
	public TMPTextFall textFallScript;

	public AudioScenePuzzleGeneric audioScenePuzGenScript;
	bool jingle = true;

	void Awake(){
		audioScenePuzGenScript = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioScenePuzzleGeneric>();
	}


	void Update () {
		if (endSeq && !masksFromMid) {
			seqTimer += Time.deltaTime;
			if (seqTimer >= appear && !appearB) {
				//FadeInPieces(); 
				MakeTxtFall();
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
		else if (endSeq && masksFromMid) {
			seqTimer += Time.deltaTime;
			if (seqTimer >= appear && !appearB) {
				//FadeInText();
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
			// STARTS REVEALING HERE! 
			Revealing();

			if(jingle){
				//1 shot sound jingle
				audioScenePuzGenScript.puzPieceJingleSnd();
				jingle=false;
			}

			audioScenePuzGenScript.puzPieceSnd(); //sound pieces
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
		if (audioScenePuzzGeneScript) {
			audioScenePuzzGeneScript.StopSceneMusic();
			audioScenePuzzGeneScript.PlayTransitionMusic();
		}
		GlobalVariables.globVarScript.sceneFadeScript.SwitchScene(sceneName);
	}

	void MakeTxtFall() {
		//textColorScript.startFadeIn = true;
		textFallScript.fallOn = true;
	}

	void Reset() {
		if (masksFromMid) {
			leftPuzzPiece.transform.position = leftStartPos.position;
			rightPuzzPiece.transform.position = rightStartPos.position;
		}
		else {
			textColorScript.Reset();
		}
		seqTimer = 0f;
		appearB = false;
		fxB = false;
		stopTrailB = false;
		endB = false;
		reset = false;
	}
}
