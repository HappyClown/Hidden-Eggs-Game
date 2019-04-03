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

	void Update () {
		if (splineWalkerScript.isPlaying) {
			puzzNewScale = animScale.x * (animCurve.Evaluate(splineWalkerScript.Progress) * scaleMult + 1);
			Vector3 newScaleVect3 = new Vector3(puzzNewScale, puzzNewScale, puzzNewScale);
			this.transform.localScale = newScaleVect3;

			fxNewScale = 1 * (animCurve.Evaluate(splineWalkerScript.Progress) * scaleMult + 1);
			Vector3 newFXScale = new Vector3(fxNewScale, fxNewScale, fxNewScale);
			pieceShimFX.transform.localScale = newFXScale;
			pieceRaysFX.transform.localScale = newFXScale;
		}
	}

	public void PieceTrailFX() {
		if (pieceTrailFX.isPlaying) {
			pieceTrailFX.Play(false);
		}
		else {
			pieceTrailFX.Play(true);
		}
	}

	public void PieceShimFX() {
		if (pieceShimFX.isPlaying) {
			pieceShimFX.Play(false);
		}
		else {
			pieceShimFX.Play(true);
		}
	}

	public void PieceRaysFX() {
		if (pieceRaysFX.isPlaying) {
			pieceRaysFX.Play(false);
		}
		else {
			pieceRaysFX.Play(true);
		}
	}

	public void PuzzPieceSplineMove() {
		splineWalkerScript.IsPlaying = true;
		puzzPiece.transform.parent = puzzParentObj.transform.parent;
	}

	public void TakeAnimScale () {
		anim.enabled = false;
		this.transform.localScale = animScale;
	}
}
