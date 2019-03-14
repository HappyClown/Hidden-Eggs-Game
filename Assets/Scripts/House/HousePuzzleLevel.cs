using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HousePuzzleLevel : MonoBehaviour {

	public SinkPiece[] mySinkPieces;
	public bool levelLoading, levelComplete;
	public float dishesNeeded;
	// Use this for initialization
	void Update(){
		if(levelLoading){
			StartLevel();
			levelLoading = false;
		}
	}
	public void StartLevel(){
		foreach (SinkPiece piece in mySinkPieces)
		{
			piece.active = true;
		}
	}
	public void ResetLevel(){
		foreach (SinkPiece piece in mySinkPieces)
		{
			piece.ResetCell();
		}
	}	
	public void CheckLevelComplete(){
		foreach (SinkPiece piece in mySinkPieces)
		{
			bool check = false;
			if(!piece.matched & piece.pieceType != SinkPiece.pieceTypes.bubble){
				check = true;
			}
			levelComplete = check;
		}
	}
}
