using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheaterPuzzlePiece : MonoBehaviour {
	public PuzzleCell[] mycells;
	public Vector3 startPos,outPos, placedPos;
	public bool placed;
	//reference variables for rotation, hard code the rotation value
	private float currentRotation, rotationValue = -90f;
	private Quaternion initialRotation;

	// Use this for initialization
	void Start () {
		initialRotation = this.gameObject.transform.rotation;
		startPos = this.gameObject.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.R)){
			RotatePiece();
		}
		if(Input.GetKeyDown(KeyCode.P)){
			ResetPiece();
		}
	}
	public void RotatePiece(){
		this.gameObject.transform.Rotate(0,0,rotationValue);
	}
	public void ResetPiece(){
		this.gameObject.transform.rotation = initialRotation;
		this.gameObject.transform.position = startPos;
	}
}
