﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BakeryBaguette : MonoBehaviour {
	public bool horizontal, vertical;
	public enum baguetteColor{
		red,
		yellow,
		Blue
	} 
	public baguetteColor myColor;
	public int timesToMove;
	public bool active, gettingPushed, canMove, onGoal;
	public Vector3 nextPos, currentPos, startPos;
	public PuzzleCell[] myCells;
	public PuzzleCell firstCell, lastCell, nextCell;

	// Use this for initialization
	void Start () {
		startPos = this.gameObject.transform.position;
		currentPos = startPos;
	}
	
	// Update is called once per frame
	void Update () {
		if(canMove){
			this.gameObject.transform.position = currentPos;	
		}
	}
	public void MoveLeft(){
		if(!firstCell.edgeLeft){
			if(!firstCell.cellLeft.occupied){
				canMove = true;
				float provDist = Vector3.Distance(firstCell.cellLeft.gameObject.transform.position,firstCell.gameObject.transform.position);
				nextPos = new Vector3(currentPos.x-provDist,currentPos.y,currentPos.z);
			}else{
				canMove = false;
			}
		}else{
			canMove = false;
		}
		if(Vector3.Distance(currentPos,nextPos)< 1){
			currentPos = nextPos;
			List<PuzzleCell> tempCells = new List<PuzzleCell>();
			foreach (PuzzleCell cell in myCells)
			{
				tempCells.Add(cell.CheckLeftAmmount(1));
				cell.occupied = false;
			}
			for (int i = 0; i < tempCells.Count; i++)
			{
				myCells[i] = tempCells[i];
				myCells[i].occupied = true;
			}
			firstCell = firstCell.CheckLeftAmmount(1);
			lastCell = lastCell.CheckLeftAmmount(1);
			tempCells.Clear();
		}
	}
	public void MoveRight(){
		if(!lastCell.edgeRight){
			if(!lastCell.cellRight.occupied){
				canMove = true;
				float provDist = Vector3.Distance(lastCell.cellRight.gameObject.transform.position,lastCell.gameObject.transform.position);
				nextPos = new Vector3(currentPos.x+provDist,currentPos.y,currentPos.z);
			}else{
				canMove = false;
			}
		}else{
			canMove = false;
		}
		if(Vector3.Distance(currentPos,nextPos)< 1){
			currentPos = nextPos;
			List<PuzzleCell> tempCells = new List<PuzzleCell>();
			foreach (PuzzleCell cell in myCells)
			{
				tempCells.Add(cell.CheckRightAmmount(1));
				cell.occupied = false;
			}
			for (int i = 0; i < tempCells.Count; i++)
			{
				myCells[i] = tempCells[i];
				myCells[i].occupied = true;
			}
			firstCell = firstCell.CheckRightAmmount(1);
			lastCell = lastCell.CheckRightAmmount(1);
			tempCells.Clear();
		}
	}
	public void MoveUp(){
		if(!lastCell.edgeUp){
			if(!lastCell.cellUp.occupied){
				canMove = true;
				float provDist = Vector3.Distance(lastCell.cellUp.gameObject.transform.position,lastCell.gameObject.transform.position);
				nextPos = new Vector3(currentPos.x,currentPos.y+provDist,currentPos.z);
			}else{
				canMove = false;
			}
		}else{
			canMove = false;
		}
		if(Vector3.Distance(currentPos,nextPos)< 1){
			currentPos = nextPos;
			List<PuzzleCell> tempCells = new List<PuzzleCell>();
			foreach (PuzzleCell cell in myCells)
			{
				tempCells.Add(cell.CheckUpAmmount(1));
				cell.occupied = false;
			}
			for (int i = 0; i < tempCells.Count; i++)
			{
				myCells[i] = tempCells[i];
				myCells[i].occupied = true;
			}
			firstCell = firstCell.CheckUpAmmount(1);
			lastCell = lastCell.CheckUpAmmount(1);
			tempCells.Clear();
		}
	}
	public void MoveDown(){
		if(!firstCell.edgeDown){
			if(!firstCell.cellDown.occupied){
				canMove = true;
				float provDist = Vector3.Distance(firstCell.cellDown.gameObject.transform.position,firstCell.gameObject.transform.position);
				nextPos = new Vector3(currentPos.x,currentPos.y-provDist,currentPos.z);
			}else{
				canMove = false;
			}
		}else{
			canMove = false;
		}
		if(Vector3.Distance(currentPos,nextPos)< 1){
			currentPos = nextPos;
			List<PuzzleCell> tempCells = new List<PuzzleCell>();
			foreach (PuzzleCell cell in myCells)
			{
				tempCells.Add(cell.CheckDownAmmount(1));
				cell.occupied = false;
			}
			for (int i = 0; i < tempCells.Count; i++)
			{
				myCells[i] = tempCells[i];
				myCells[i].occupied = true;
			}
			firstCell = firstCell.CheckDownAmmount(1);
			lastCell = lastCell.CheckDownAmmount(1);
			tempCells.Clear();
		}
	}
	public void SetUpItem(){
		if(horizontal){
			float maxX = -1000;
			float minX = 1000;
			foreach (PuzzleCell cell in myCells)
			{
				if(cell.gameObject.transform.position.x > maxX){
					lastCell = cell;
					maxX = cell.gameObject.transform.position.x;
				}
				if(cell.gameObject.transform.position.x < minX){
					firstCell = cell;
					minX = cell.gameObject.transform.position.x;
				}
			}
		}
		else if(vertical){
			float maxY = -1000;
			float minY = 1000;
			foreach (PuzzleCell cell in myCells)
			{
				if(cell.gameObject.transform.position.y > maxY){
					lastCell = cell;
					maxY = cell.gameObject.transform.position.y;
				}
				if(cell.gameObject.transform.position.y < minY){
					firstCell = cell;
					minY = cell.gameObject.transform.position.y;
				}
			}
		}
		this.transform.position = startPos;
		active = true;
		foreach (PuzzleCell cell in myCells)
		{	
			cell.occupied = true;
		}
	}
	public void ResetItem(){
		this.transform.position = startPos;
		active = false;
		foreach (PuzzleCell cell in myCells)
		{	
			cell.occupied = false;
		}
	}
}