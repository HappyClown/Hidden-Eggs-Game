using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToyStorePuzzleLevel : MonoBehaviour {
	public bool levelComplete, finished;
	public float snapRadius, backDuration;
	public List<PuzzleCell> goalCells = new List<PuzzleCell>();
	public List<ToyStorePieceData> pieces = new List<ToyStorePieceData>();
	

	// Use this for initialization
	void Start () {
		Instantiate(pieces[0].piecePrefab, Vector3.zero, Quaternion.identity);
		pieces[0].inGame = true;
	}
	
	// Update is called once per frame
	void Update () {
		Debug.Log(pieces[0].inGame);
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
