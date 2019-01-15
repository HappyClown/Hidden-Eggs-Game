using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleUnlock : MonoBehaviour {
	
	public GameObject puzzPiece, puzzParentObj, endPosObj;
	public float moveDuration;
	public AnimationCurve animCurve;
	private Vector3 iniPos, endPos, curPos;
	private float lerpValue, curveValue;
	private bool movePuzzPiece;

	[Header("Unlocked Area")]
	public bool puzzUnlocked;
	public GameObject puzzClickArea;
	public ParticleSystem puzzShimFX;
	public int puzzUnlockAmnt;
	public Animation puzzAnim;
	public ClickOnEggs clickOnEggsScript;


	void Update () {
		if (movePuzzPiece) {
			lerpValue += Time.deltaTime / moveDuration;
			curveValue = animCurve.Evaluate(lerpValue);
			puzzPiece.transform.position = Vector3.Lerp(iniPos, endPos, curveValue);
			curPos = puzzPiece.transform.position;
			if (Vector3.Distance(curPos, endPos) <= 0.01f) {
				puzzPiece.transform.position = endPos;
				movePuzzPiece = false;
				ActivatePuzzle();
			}
		}
	}
	
	public void UnlockPuzzle() {
		movePuzzPiece = true;
		iniPos = puzzPiece.transform.position;
		endPos = endPosObj.transform.position;
		puzzPiece.transform.parent = puzzParentObj.transform.parent;
	}

	public void PuzzleUnlockCheck(int eggsInPanel) {
		if (eggsInPanel >= puzzUnlockAmnt) {
			UnlockPuzzle();
		}	
	}

	public void ActivatePuzzle() {
		puzzClickArea.SetActive(true);
		puzzPiece.SetActive(false);
		puzzShimFX.Play(true);
		puzzAnim.Play();
		puzzUnlocked = true;
	}


}
