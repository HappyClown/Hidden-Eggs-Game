using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleUnlock : MonoBehaviour {
	public GameObject puzzPiece, puzzParentObj, endPosObj;
	private Vector3 endPos, curPos;
	public Animator anim;
	public PuzzPieceAnimEvents PuzzPieceFXsScript;
	public FadeInOutSprite pointerFadeScript;

	[Header("Unlocked Area")]
	public bool puzzIntroDone;
	public GameObject puzzClickArea;
	public ParticleSystem puzzShimFX, puzzDustFX, puzzFireworkFX;
	public int puzzUnlockAmnt;
	public Animation puzzAnim;
	private bool unlockInQueue = false;

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

	// Check to see if enough eggs have been found to unlock the puzzle.
	public void PuzzleUnlockCheck(int eggsInPanel) {
		if (!puzzIntroDone && eggsInPanel >= puzzUnlockAmnt) {
			if (!unlockInQueue) {
				QueueSequenceManager.AddSequenceToQueue(UnlockPuzzle);
				unlockInQueue = true;
			}
		}	
	}
	// Start moving the puzzle piece.
	public void UnlockPuzzle() {
		clickOnEggsScript.EggMoving(true, false);
		endPos = endPosObj.transform.position;
		pointerFadeScript.FadeOut();
		anim.enabled = true;
		anim.SetTrigger("PuzzPiecePop");
		//Play FX's through animation events
		darkScreenFadeScript.FadeIn();
		sceneTapScript.canTapEggRidPanPuz = false;
		sceneTapScript.canTapHelpBird = false;
		sceneTapScript.canTapPauseBtn = false;
		levelTapScript.ZoomOutCameraReset();
		audioSceneGenScript.puzzlePieceAnimation();
		StartCoroutine(PuzzlePieceMovement());
	}
	// Keep track of where the puzzle piece is to know when it has reached its destination.
	IEnumerator PuzzlePieceMovement() {
		while(Vector2.Distance(curPos, endPos) > 0.01f) {
			curPos = puzzPiece.transform.position;
			yield return null;
		}
		puzzPiece.transform.position = endPos;
		PuzzPieceFXsScript.PieceShimFX();
		PuzzPieceFXsScript.PieceRaysFX();
		puzzFireworkFX.gameObject.SetActive(true);
		darkScreenFadeScript.FadeOut();
		// Adjust this because right now it adds one egg to the eggpanel count.
		clickOnEggsScript.EggMoving(false, false);
		sceneTapScript.canTapEggRidPanPuz = true;
		sceneTapScript.canTapHelpBird = true;
		sceneTapScript.canTapPauseBtn = true;
		ActivatePuzzle();
		QueueSequenceManager.SequenceComplete();
	}
	// Activate the puzzle click area. (after the puzzle piece reaches it)
	public void ActivatePuzzle() {
		//audioSceneGenScript.puzzleAnimation();
		audioSceneGenScript.puzzleAnimationStart(puzzClickArea);
		puzzClickArea.SetActive(true);
		puzzPiece.SetActive(false);
		puzzShimFX.gameObject.SetActive(true);
		puzzShimFX.Play(true);
		puzzDustFX.gameObject.SetActive(true);
		puzzDustFX.Play(true);
		if (puzzAnim) { puzzAnim.Play(); }
		if (pointerFadeScript.gameObject.activeSelf) { 
			pointerFadeScript.gameObject.SetActive(false);
		}
		if (!puzzIntroDone) {
			puzzIntroDone = true;
			SavePuzzleIntro();
		}
	}
	public void LoadPuzzleIntro() {
		puzzIntroDone = GlobalVariables.globVarScript.puzzIntroDone;
		ActivatePuzzle();
	}
	void SavePuzzleIntro() {
		GlobalVariables.globVarScript.puzzIntroDone = puzzIntroDone;
		GlobalVariables.globVarScript.SaveEggState();
	}
}
