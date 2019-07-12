using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HousePuzzle : MainPuzzleEngine {
	public List<SinkPiece> myPieces;
	public SinkPiece currentPiece;
	public float cellRadius;
	private int currentHashCode;
	public bool soapSelected, canSelect, firstTouch;
	public Vector3 currentCellPos;
	// Use this for initialization
	void Start () {
		currentHashCode = 0;
		soapSelected= false;
		canSelect = true;
		firstTouch = true;
	}
	
	// Update is called once per frame
	void Update () {
		if(myInput.Tapped){
			UpdateMousePos(myInput.TapPosition);
			hit = Physics2D.Raycast(mousePos2D, Vector3.forward, 50f);
			if (hit) {
					Debug.Log(hit.collider.gameObject.name);
					if (hit.collider.CompareTag("Puzzle")) {
						currentPiece = hit.collider.gameObject.GetComponent<SinkPiece>();
						if(currentPiece.pieceType == SinkPiece.pieceTypes.bubble){
							currentPiece.matched = true;
							ResetPieces();
						}
						else{
							ResetPieces();
						}
					}
				}
		}
		else if(myInput.isDragging){
			if(firstTouch){
				UpdateMousePos(myInput.startDragTouch);
				firstTouch = false;
			}else{
				UpdateMousePos(myInput.draggingPosition);
			}
			hit = Physics2D.Raycast(mousePos2D, Vector3.forward, 50f);
			if (hit) {
					Debug.Log(hit.collider.gameObject.name);
					if (hit.collider.CompareTag("Puzzle") && canSelect) {
						if(currentHashCode != hit.collider.gameObject.GetHashCode()){
							currentHashCode = hit.collider.gameObject.GetHashCode();
							if(myPieces.Count > 0){
								CheckConnection(hit.collider.gameObject.GetComponent<SinkPiece>());
							}							
							currentPiece = hit.collider.gameObject.GetComponent<SinkPiece>();
							if(currentPiece.pieceType == SinkPiece.pieceTypes.bubble){
								currentPiece.matched = true;
								ResetPieces();
							}
							currentCellPos =  hit.collider.gameObject.transform.position;
							myPieces.Add(hit.collider.gameObject.GetComponent<SinkPiece>());
							if(myPieces.Count == 1){
								currentPiece.selected = true;
								if(currentPiece.pieceType == SinkPiece.pieceTypes.sponge || currentPiece.pieceType == SinkPiece.pieceTypes.soap){
									if(currentPiece.pieceType == SinkPiece.pieceTypes.soap){
										soapSelected = true;
									}								
								}
								else{
									ResetPieces();
								}
							}else if(myPieces.Count < 5){								
								if(currentPiece.pieceType == SinkPiece.pieceTypes.dish){
									currentPiece.selected = true;
								}
								else{
									ResetPieces();
								}
							}else if(myPieces.Count == 5){
								if(currentPiece.pieceType == SinkPiece.pieceTypes.soap){
									if(soapSelected){
										ResetPieces();
									}
									else{
										MatchPieces();
									}
								}else if(currentPiece.pieceType == SinkPiece.pieceTypes.sponge){
									if(soapSelected){
										MatchPieces();
									}
									else{
										ResetPieces();
									}
								}else{
									ResetPieces();
								}
							}
						}
					}
					
				}else{
					Debug.Log(Vector2.Distance(currentCellPos,Camera.main.ScreenToWorldPoint(myInput.draggingPosition)));
					if(Vector2.Distance(currentCellPos,Camera.main.ScreenToWorldPoint(myInput.draggingPosition)) > cellRadius && myPieces.Count > 0){
						ResetPieces();
					}
				}
		}
		else{			
			ResetPieces();
			canSelect = true;
			firstTouch = true;
		}
	}
	public void UpdateMousePos(Vector3 Pos)
	{
		mousePos = Camera.main.ScreenToWorldPoint(Pos);
		mousePos2D = new Vector2 (mousePos.x, mousePos.y);
	}
	public void ResetPieces(){
		foreach (SinkPiece item in myPieces)
		{
			item.selected = false;
		}
		myPieces.Clear();
		canSelect = false;
		currentCellPos = Vector3.zero;
		currentHashCode = 0;
		soapSelected = false;
	}
	public void MatchPieces(){
		foreach (SinkPiece item in myPieces)
		{
			item.selected = false;
			item.matched = true;
		}
		myPieces.Clear();
		canSelect = false;
		currentCellPos = Vector3.zero;
		currentHashCode = 0;
		soapSelected = false;
	}
	public void CheckConnection(SinkPiece newPiece){
		bool badConnection = true;
		if(newPiece.currentCell.cellDown == currentPiece.currentCell){
			badConnection = false;
		}else if(newPiece.currentCell.cellUp == currentPiece.currentCell){
			badConnection = false;
		}
		else if(newPiece.currentCell.cellLeft == currentPiece.currentCell){
			badConnection = false;
		}
		else if(newPiece.currentCell.cellRight == currentPiece.currentCell){
			badConnection = false;
		}
		if(badConnection){
			ResetPieces();
		}

	}
}
