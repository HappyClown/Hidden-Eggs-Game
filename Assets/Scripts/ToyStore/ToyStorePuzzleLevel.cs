using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToyStorePuzzleLevel : MonoBehaviour {
	public bool levelComplete, finished;
	public float snapRadius, backDuration;
	public ToyStorePuzzlePiece[] myPieces;
	public PuzzleCell[] goalCells;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void SetUpLevel(){
		levelComplete = false;
		finished = false;
	}
	public void ResetLevel(){

	}
	public void FreeCells(ToyStorePuzzlePiece piece){
		foreach (PuzzleCell pieceCell in piece.mycells)
			{	
				//set logic here
			}
	}
}
