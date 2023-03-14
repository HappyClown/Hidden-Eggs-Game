using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToyStorePuzzlePiece : MonoBehaviour {

	
	public PuzzleCell[] mycells;
	public PuzzleCell mostLeftCell, mostRightCell;
	public int inBetweenCells;
	public Vector3 startPos,dropPos, placedPos;
	public SpriteRenderer[] pieceSprites;
	public bool moving;
	//reference variables for rotation, hard code the rotation value
	public float currentRotation, rotationValue = -90f, moveTimer, duration = 1f, cellRadius = 0f;
	public AnimationCurve movingCurve;
	private Quaternion initialRotation;

	void Awake () {
		mycells = this.gameObject.GetComponentsInChildren<PuzzleCell>();
		cellRadius = this.gameObject.GetComponent<GridBuilderScript>().cellSize;
	}
	// Use this for initialization
	void Start () {
		initialRotation = this.gameObject.transform.rotation;
		startPos = this.gameObject.transform.position;
		SetEdgeCells();
	}
	
	// Update is called once per frame
	void Update () {
		if(moving){
			moveTimer += Time.deltaTime;
			if(moveTimer >= duration){
				this.gameObject.transform.position += Vector3.down*cellRadius;
				moveTimer = 0;
			}			
			//this.gameObject.transform.position = Vector3.MoveTowards(this.gameObject.transform.position,startPos,0.1f);
			/*if(moveTimer >= 1){
				moveTimer = 0;
				movingBack = false;
			}*/
			if(Vector3.Distance(this.gameObject.transform.position,placedPos) <= 0.1f){
				this.gameObject.transform.position = placedPos;
				moving = false;
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
		this.gameObject.transform.position = startPos;
		this.gameObject.GetComponent<BoxCollider2D>().enabled = true;
		SetEdgeCells();
	}
	public void SetTargetPos(Vector3 targetPos, Vector3 startCellPos){
		this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x + targetPos.x,this.gameObject.transform.position.y + startCellPos.y,this.gameObject.transform.position.z);
		moving = true;
		dropPos = this.transform.position;
		placedPos = new Vector3(this.gameObject.transform.position.x,this.gameObject.transform.position.y + targetPos.y - startCellPos.y,this.gameObject.transform.position.z);
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
		inBetweenCells = (int)((maxX - minX)/cellRadius)+1;
	}
}
