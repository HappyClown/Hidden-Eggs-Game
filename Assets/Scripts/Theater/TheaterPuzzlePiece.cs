using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheaterPuzzlePiece : MonoBehaviour {
	public PuzzleCell[] mycells;
	public Vector3 startPos,outPos, placedPos;
	public SpriteRenderer[] pieceSprites;
	public bool placed, movingBack;
	//reference variables for rotation, hard code the rotation value
	private float currentRotation, rotationValue = -90f, moveTimer, duration;
	public AnimationCurve movingCurve;
	private Quaternion initialRotation;

	// Use this for initialization
	void Start () {
		initialRotation = this.gameObject.transform.rotation;
		startPos = this.gameObject.transform.position;
		outPos = startPos;
	}
	
	// Update is called once per frame
	void Update () {
		if(movingBack){
			moveTimer += Time.deltaTime * duration;
			this.gameObject.transform.position = Vector3.MoveTowards(outPos,startPos,moveTimer);
			//this.gameObject.transform.position = Vector3.MoveTowards(this.gameObject.transform.position,startPos,0.1f);
			/*if(moveTimer >= 1){
				moveTimer = 0;
				movingBack = false;
			}*/
			if(Vector3.Distance(this.gameObject.transform.position,startPos) <= 0.1f){
				this.gameObject.transform.position = startPos;
				movingBack = false;
				moveTimer = 0;
			}
		}
	}
	public void RotatePiece(){
		this.gameObject.transform.Rotate(0,0,rotationValue);
	}
	public void ResetPiece(){
		this.gameObject.transform.rotation = initialRotation;
		this.gameObject.transform.position = startPos;
	}
	public void BackToStart(float backDuration){
		movingBack = true;
		outPos = this.transform.position;
		duration = backDuration;
	}
}
