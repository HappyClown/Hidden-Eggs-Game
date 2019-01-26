using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzPieceAnimEvents : MonoBehaviour {

	public ParticleSystem pieceTrailFX, pieceShimFX, pieceRaysFX;
	public Vector3 animScale;
	public Animator anim;
	public SplineWalker splineWalkerScript;

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
	}

	public void TakeAnimScale () {
		anim.enabled = false;
		this.transform.localScale = animScale;
	}
}
