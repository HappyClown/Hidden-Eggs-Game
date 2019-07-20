using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HousePuzzleLevel : MonoBehaviour {

	public SinkPiece[] mySinkPieces;
	public bool levelReady, levelComplete, active;
	public float dishesNeeded;
	// Use this for initialization
	void Awake(){
		mySinkPieces = this.gameObject.GetComponentsInChildren<SinkPiece>();
	}
	void Update(){
		if(levelReady){
			StartLevel();
			levelReady = false;
		}
		if(active){
			CheckLevelComplete();
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
		active = false;
		this.gameObject.SetActive(active);
		levelComplete = false;
	}	
	public void CheckLevelComplete(){
		bool check = true;
		foreach (SinkPiece piece in mySinkPieces)
		{
			if(!piece.matched && piece.pieceType != SinkPiece.pieceTypes.bubble){
				check = false;
			}
		}
		levelComplete = check;
	}
	public void SetUpLevel(){
		active = true;
		this.gameObject.SetActive(active);
		foreach (SinkPiece piece in mySinkPieces)
		{
			piece.SetUp();
		}
		levelReady = true;
	}
}
