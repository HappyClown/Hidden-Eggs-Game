using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheaterPuzzleLevel : MonoBehaviour {

	public bool levelComplete, finished;
	public float snapRadius, backDuration;
	private int resetCount = 0;
	public GameObject gridHolder;
	public PuzzleCell[] gridCells;
	public TheaterPuzzlePiece[] myPieces;
	// Use this for initialization
	void Start () {
		gridCells = gridHolder.GetComponentsInChildren<PuzzleCell>();
	}
	public void CheckComplete(){
		levelComplete = true;
		foreach (PuzzleCell cell in gridCells)
		{
			levelComplete = !cell.occupied ? false : levelComplete;
		}
	}
	public void SetUpLevel(){
		levelComplete = false;
		finished = false;
	}
	public void ResetLevel(){
		if(resetCount >0){
			foreach (TheaterPuzzlePiece piece in myPieces)
			{
				piece.ResetPiece();
			}
			foreach (PuzzleCell cell in gridCells)
			{
				cell.occupied = false;
			}
		}
		resetCount ++;
	}
	public void CheckPiece(TheaterPuzzlePiece piece){
		Vector2 snapPos = Vector2.zero;
		int checker = piece.mycells.Length;
		bool ongrid = false;
		foreach (PuzzleCell pieceCell in piece.mycells)
		{	
			foreach (PuzzleCell gridcell in gridCells)
			{
				if(Vector2.Distance(pieceCell.gameObject.transform.position,gridcell.transform.position) < snapRadius){
					ongrid = true;
					if(!gridcell.occupied){
						pieceCell.occupied = true;
						pieceCell.GetComponent<TheaterPuzzleCellConn>().myGridConn = gridcell;
						snapPos = pieceCell.gameObject.transform.position - gridcell.transform.position;
						checker --;
					}
				}
			}
		}
		if(checker == 0)
		{			
			piece.placed = true;
			foreach (PuzzleCell pieceCell in piece.mycells)
			{	
				pieceCell.GetComponent<TheaterPuzzleCellConn>().myGridConn.occupied = true;
			}
			foreach (SpriteRenderer spRend in piece.pieceSprites)
			{
				spRend.gameObject.SetActive(false);																
			}
			piece.gameObject.transform.position = piece.gameObject.transform.position - new Vector3(snapPos.x, snapPos.y, 0);
			piece.placedPos = piece.gameObject.transform.position;//save placed pos in case is released
		}else{
			if(piece.placed && ongrid){//if piece is released close to the grid but cant be placed
				piece.gameObject.transform.position = piece.placedPos;
				foreach (PuzzleCell pieceCell in piece.mycells)
				{	
					pieceCell.GetComponent<TheaterPuzzleCellConn>().myGridConn.occupied = true;
					pieceCell.occupied = true;
				}
			}else{
				piece.BackToStart(backDuration);
				piece.placed = false;
			}
		}
		CheckComplete();
	}
	public void FreeCells(TheaterPuzzlePiece piece){
		foreach (PuzzleCell pieceCell in piece.mycells)
			{	
				pieceCell.GetComponent<TheaterPuzzleCellConn>().myGridConn.occupied = false;
				pieceCell.occupied = false;
			}
	}
}
