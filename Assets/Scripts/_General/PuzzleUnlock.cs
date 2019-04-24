using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleUnlock : MonoBehaviour {
	
	public GameObject puzzPiece, puzzParentObj, endPosObj;
	//public float moveDuration;
	//public AnimationCurve animCurve;
	private Vector3 /* iniPos, */ endPos, curPos;
	//private float lerpValue, curveValue;
	private bool movePuzzPiece, waitToStartSeq;
	public Animator anim;
	public PuzzPieceAnimEvents PuzzPieceFXsScript;
	public FadeInOutSprite pointerFadeScript;

	[Header("Unlocked Area")]
	public bool puzzUnlocked;
	public GameObject puzzClickArea;
	public ParticleSystem puzzShimFX, puzzDustFX, puzzFireworkFX;
	public int puzzUnlockAmnt;
	public Animation puzzAnim;

	[Header("References")]
	public ClickOnEggs clickOnEggsScript;
	public FadeInOutImage darkScreenFadeScript;
	public SceneTapEnabler sceneTapScript;
	public LevelTapMannager levelTapScript;
	public AudioSceneGeneral audioSceneGenScript;

	void Start() {
		if (!audioSceneGenScript) {
			audioSceneGenScript = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioSceneGeneral>();
		}
	}

	void Update () {
		// Wait until no other sequences are playing to start the Puzzle Unlock sequence.
		if (waitToStartSeq && !ClickOnEggs.inASequence) {
			// In a sequence.
			ClickOnEggs.inASequence = true;
			waitToStartSeq = false;
			UnlockPuzzle();
			//Debug.Log(ClickOnEggs.inASequence);
		}
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
				darkScreenFadeScript.FadeOut();
				clickOnEggsScript.eggMoving -= 1;
				sceneTapScript.canTapEggRidPanPuz = true;
				sceneTapScript.canTapHelpBird = true;
				sceneTapScript.canTapPauseBtn = true;
				// Sequence finished.
				ClickOnEggs.inASequence = false;
			}
		}
	}
	// Check to see if enough eggs have been found to unlock the puzzle.
	public void PuzzleUnlockCheck(int eggsInPanel) {
		if (eggsInPanel == puzzUnlockAmnt) {
			waitToStartSeq = true;
		}	
	}
	// Start moving the puzzle piece.
	public void UnlockPuzzle() {
		movePuzzPiece = true;
		clickOnEggsScript.eggMoving += 1;
		endPos = endPosObj.transform.position;
		pointerFadeScript.FadeOut();
		anim.SetTrigger("PuzzPiecePop");
		//splineWalkerScript.IsPlaying = true;
		//Play FX's through animation events
		//puzzPiece.transform.parent = puzzParentObj.transform.parent;
		darkScreenFadeScript.FadeIn();
		sceneTapScript.canTapEggRidPanPuz = false;
		sceneTapScript.canTapHelpBird = false;
		sceneTapScript.canTapPauseBtn = false;
		levelTapScript.ZoomOutCameraReset();

		audioSceneGenScript.puzzlePieceAnimation();
		//Debug.Log("sound -  Puzzle piece");
	}
	// Activate the puzzle click area. (after the puzzle piece reaches it)
	public void ActivatePuzzle() {
		//audioSceneGenScript.puzzleAnimation();
		audioSceneGenScript.puzzleAnimationStart(puzzClickArea);
		//Debug.Log("sound - Puzzle unlocked");
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
