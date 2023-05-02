using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToyStorePuzzleLevel : MonoBehaviour {
	public bool levelComplete, finished, pieceBadPlaced;
	public float snapRadius, backDuration;
	public List<PuzzleCell> goalCells = new List<PuzzleCell>();
	public List<ToyStorePieceData> pieces = new List<ToyStorePieceData>();
	public GameObject[] spawnSpots;
	public GameObject pieceHolder;
		
	// Update is called once per frame
	void Update () {
	}
	public void SetUpLevel(){
		pieceBadPlaced = false;
		ResetLevel();
		foreach (GameObject spawnSpot in spawnSpots)
		{
			SpawnPiece(spawnSpot.transform.position,0,0);
		}
		levelComplete = false;
		finished = false;
	}
	public void ResetLevel(){
		foreach (PuzzleCell goal in goalCells)
		{
			goal.occupied = false;
		}
		ResetPieces();
	}
	private void ResetPieces(){
		foreach (ToyStorePieceData piece in pieces)
		{
			piece.inGame = false;
			piece.spotPos = Vector3.zero;
		}
		 foreach (Transform child in pieceHolder.transform) {
			Destroy(child.gameObject);
		}	
	}
	public void SpawnPiece(Vector3 pos, int type, int version){
		float val = 0;
		Dictionary<int,float> typesInGame = new Dictionary<int,float>();
		if(type > 0){
			typesInGame.Add(type,1);
		}		
		for (int i = 0; i < pieces.Count; i++)
		{			
			if(pieces[i].inGame){
				if(typesInGame.ContainsKey(pieces[i].type)){
					typesInGame[pieces[i].type] += 1;
				}else{
					typesInGame.Add(pieces[i].type,1);
				}
			}			
		}
		for (int i = 0; i < pieces.Count; i++)
		{
			bool canBePlaced = true;
			if(typesInGame.ContainsKey(pieces[i].type)){
				if(typesInGame[pieces[i].type] > 3){
					canBePlaced = false;
				}
			}
			if(pieces[i].type == type && pieces[i].version == version){
				canBePlaced = false;
			}
			if(pieces[i].inGame){
				canBePlaced = false;
			}
			if(canBePlaced){
				val += pieces[i].pieceWeight;
			}			
		}
		float acumulated = 0, selectedVal = 0;
		selectedVal = Random.Range(0,val);
		for (int i = 0; i < pieces.Count; i++)
		{
			bool canBePlaced = true;
			if(typesInGame.ContainsKey(pieces[i].type)){
				if(typesInGame[pieces[i].type] > 3){
					canBePlaced = false;
				}
			}
			if(pieces[i].type == type && pieces[i].version == version){
				canBePlaced = false;
			}
			if(pieces[i].inGame){
				canBePlaced = false;
			}
			if(canBePlaced){
				acumulated += pieces[i].pieceWeight;
			}
			if(acumulated >= selectedVal){
				Instantiate(pieces[i].piecePrefab,pos,Quaternion.identity,pieceHolder.transform);
				pieces[i].inGame = true;
				pieces[i].spotPos = pos;
				i = pieces.Count;
			}			
		}
		typesInGame.Clear();
	}
	public void SetSpawn(int type, int version){
		for (int i = 0; i < pieces.Count; i++)
		{
			if(pieces[i].type == type && pieces[i].version == version){
				SpawnPiece(pieces[i].spotPos, type, version);
				pieces[i].inGame = false;
			}
		}
	}
}
