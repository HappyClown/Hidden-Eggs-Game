using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleUnlock : MonoBehaviour {
	
	public GameObject puzzPiece, puzzParentObj, endPosObj;
	//public float moveDuration;
	//public AnimationCurve animCurve;
	private Vector3 /* iniPos, */ endPos, curPos;
	//private float lerpValue, curveValue;
	private bool movePuzzPiece;
	public Animator anim;
	public PuzzPieceAnimEvents PuzzPieceFXsScript;
	public FadeInOutSprite pointerFadeScript;

	[Header("Unlocked Area")]
	public bool puzzUnlocked;
	public GameObject puzzClickArea;
	public ParticleSystem puzzShimFX, puzzDustFX, puzzFireworkFX;
	public int puzzUnlockAmnt;
	public Animation puzzAnim;
	//public ClickOnEggs clickOnEggsScript;
	public AudioSceneGeneral audioSceneGenScript;

	void Start() {
		if (!audioSceneGenScript) {
			audioSceneGenScript = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioSceneGeneral>();
		}
	}

	void Update () {
		 if (movePuzzPiece) {
		// 	lerpValue += Time.deltaTime / moveDuration;
		// 	curveValue = animCurve.Evaluate(lerpValue);
		// 	puzzPiece.transform.position = Vector3.Lerp(iniPos, endPos, curveValue);
		 	curPos = puzzPiece.transform.position;
			if (Vector2.Distance(curPos, endPos) <= 0.01f) {
				puzzPiece.transform.position = endPos;
				movePuzzPiece = false;
				PuzzPieceFXsScript.PieceShimFX();
				PuzzPieceFXsScript.PieceRaysFX();
				puzzFireworkFX.Play(true);
				ActivatePuzzle();
				//Debug.Log("activated puzz");
			}
		}
	}
	
	public void UnlockPuzzle() {
		movePuzzPiece = true;
		endPos = endPosObj.transform.position;
		pointerFadeScript.FadeOut();
		anim.SetTrigger("PuzzPiecePop");
		//splineWalkerScript.IsPlaying = true;
		//Play FX's through animation events
		puzzPiece.transform.parent = puzzParentObj.transform.parent;


		audioSceneGenScript.puzzlePieceAnimation();
		Debug.Log("sound -  Puzzle piece");
	}

	public void PuzzleUnlockCheck(int eggsInPanel) {
		if (eggsInPanel == puzzUnlockAmnt) {
			UnlockPuzzle();
		}	
	}

	public void ActivatePuzzle() {
		//audioSceneGenScript.puzzleAnimation();
		audioSceneGenScript.puzzleAnimationStart(puzzClickArea);
		Debug.Log("sound - Puzzle unlocked");
		puzzClickArea.SetActive(true);
		puzzPiece.SetActive(false);
		puzzShimFX.Play(true);
		puzzDustFX.Play(true);
		if (puzzAnim) { puzzAnim.Play(); }
		if (pointerFadeScript.sprite.color.a > 0) { 
			pointerFadeScript.FadeOut(); 
		}
		puzzUnlocked = true;
	}
}
