using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToyStorePuzzlePiece : MonoBehaviour {

	
	public PuzzleCell[] mycells;
	public PuzzleCell mostLeftCell, mostRightCell;
	public int inBetweenCells;
	public Vector3 startPos,outPos, placedPos;
	public SpriteRenderer[] pieceSprites;
	public bool placed, movingBack;
	//reference variables for rotation, hard code the rotation value
	private float currentRotation, rotationValue = -90f, moveTimer, duration;
	public AnimationCurve movingCurve;
	private Quaternion initialRotation;

	void Awake () {
		mycells = this.gameObject.GetComponentsInChildren<PuzzleCell>();
	}
	// Use this for initialization
	void Start () {
		initialRotation = this.gameObject.transform.rotation;
		startPos = this.gameObject.transform.position;
		outPos = startPos;
		SetEdgeCells();
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
		foreach (PuzzleCell cell in mycells)
		{
			PuzzleCell tempcell;
			tempcell = cell.cellLeft;
			cell.cellLeft = cell.cellDown;
			cell.cellDown = cell.cellRight;
			cell.cellRight = cell.cellUp;
			cell.cellUp = tempcell;
			bool tempEdge;
			tempEdge = cell.edgeLeft;
			cell.edgeLeft = cell.edgeDown;
			cell.edgeDown = cell.edgeRight;
			cell.edgeRight = cell.edgeUp;
			cell.edgeUp = tempEdge;			
		}
		SetEdgeCells();
	}
	public void ResetPiece(){
		this.gameObject.transform.rotation = initialRotation;
		this.gameObject.transform.position = startPos;
		this.gameObject.GetComponent<BoxCollider2D>().enabled = true;
		SetEdgeCells();
	}
	public void BackToStart(float backDuration){
		movingBack = true;
		outPos = this.transform.position;
		duration = backDuration;
	}
	public void SetEdgeCells(){
		float minX = 10000;
		float maxX = -10000;
		foreach (PuzzleCell cell in mycells){
			if(cell.gameObject.transform.position.x < minX){
				mostLeftCell = cell;
				minX = cell.gameObject.transform.position.x;
			}
			if(cell.gameObject.transform.position.x > maxX){
				mostRightCell = cell;
				maxX = cell.gameObject.transform.position.x;
			}
		}
		float cellRadious = this.gameObject.GetComponent<GridBuilderScript>().cellSize;
		inBetweenCells = (int)((maxX - minX)/cellRadious)+1;
	}
}
