using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzPieceAnimEvents : MonoBehaviour {
	[Header("Animation")]
	public Animator anim;
	public Vector3 animScale;
	public ParticleSystem pieceTrailFX, pieceShimFX, pieceRaysFX;
	[Header("Movement")]
	public SplineWalker splineWalkerScript;
	public AnimationCurve animCurve;
	public float scaleMult;
	private float puzzNewScale, fxNewScale;
	public GameObject puzzPiece, puzzParentObj;

	IEnumerator ScalePuzzlePiece() {
		while (splineWalkerScript.isPlaying) {
			puzzNewScale = animScale.x * (animCurve.Evaluate(splineWalkerScript.Progress) * scaleMult + 1);
			Vector3 newScaleVect3 = new Vector3(puzzNewScale, puzzNewScale, puzzNewScale);
			this.transform.localScale = newScaleVect3;

			fxNewScale = 1 * (animCurve.Evaluate(splineWalkerScript.Progress) * scaleMult + 1);
			Vector3 newFXScale = new Vector3(fxNewScale, fxNewScale, fxNewScale);
			pieceShimFX.transform.localScale = newFXScale;
			pieceRaysFX.transform.localScale = newFXScale;
			yield return null;
		}
	}

	public void PieceTrailFX() {
		if(!pieceTrailFX.gameObject.activeSelf) {
			pieceTrailFX.gameObject.SetActive(true);
		}
		else {
			if (pieceTrailFX.isPlaying) {
				pieceTrailFX.Play(false);
			}
			pieceTrailFX.gameObject.SetActive(false);
			return;
		}

		if (!pieceTrailFX.isPlaying) {
			pieceTrailFX.Play(true);
		}
	}

	public void PieceShimFX() {
		if(!pieceShimFX.gameObject.activeSelf) {
			pieceShimFX.gameObject.SetActive(true);
			if (!pieceShimFX.isPlaying) {
				pieceShimFX.Play(true);
			}
		}
		else {
			if (pieceShimFX.isPlaying) {
				pieceShimFX.Play(false);
			}
			pieceShimFX.gameObject.SetActive(false);
		}
	}

	public void PieceRaysFX() {
		if (!pieceRaysFX.gameObject.activeSelf) {
			pieceRaysFX.gameObject.SetActive(true);
			if (!pieceRaysFX.isPlaying) {
				pieceRaysFX.Play(true);
			}
		}
		else {
			if (pieceRaysFX.isPlaying) {
				pieceRaysFX.Play(false);
			}
			pieceRaysFX.gameObject.SetActive(false);
		}
	}

	public void PuzzPieceSplineMove() {
		splineWalkerScript.IsPlaying = true;
		StartCoroutine(ScalePuzzlePiece());
		puzzPiece.transform.parent = puzzParentObj.transform.parent;
	}

	public void TakeAnimScale () {
		anim.enabled = false;
		this.transform.localScale = animScale;
	}
}
