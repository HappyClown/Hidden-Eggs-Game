using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridBuilderScript : MonoBehaviour {

	public GameObject Cell;
	public Vector3 spawnPoint = new Vector3(0,0,-3.0f);
	public float cellSize = 1.0f;
	
	public PuzzleCell currentCell;
	public PuzzleCell prevCell;
	public int row, col;

	public void ResetValues(){
		spawnPoint = new Vector3(0,0,-3.0f);
		cellSize = 1.0f;
		col = 0;
		row = 0;
	}
	public void DeleteGrid(){
		PuzzleCell[] cells = this.GetComponentsInChildren<PuzzleCell>();
		foreach (PuzzleCell cell in cells)
		{
			DestroyImmediate(cell.gameObject);
		}
		ResetValues();
	}
	public void CreateCell(){
		GameObject newCell =  Instantiate(Cell,spawnPoint,Quaternion.identity);
		newCell.transform.localScale *= cellSize;
		newCell.transform.parent = this.gameObject.transform;
		newCell.transform.localPosition = spawnPoint;
		currentCell = newCell.GetComponent<PuzzleCell>();
		currentCell.edgeUp = true;
		currentCell.edgeDown = true;
		currentCell.edgeLeft = true;
		currentCell.edgeRight = true;
		row = 0;
		col = 0;
		newCell.gameObject.name = col.ToString()+" : "+ row.ToString();
	}
	public void CreateLeft(){		
		spawnPoint += new Vector3(-cellSize,0,0);
		col--;
		if(currentCell.edgeLeft){
			GameObject newCell =  Instantiate(Cell,spawnPoint,Quaternion.identity);
			newCell.transform.localScale *= cellSize;
			newCell.transform.parent = this.gameObject.transform;
			newCell.transform.localPosition = spawnPoint;
			prevCell = currentCell;
			currentCell = newCell.GetComponent<PuzzleCell>();
			PopulateCell(currentCell);
			newCell.gameObject.name = col.ToString()+" : "+ row.ToString();
		}
		else{
			Debug.Log("Cell Already at left");
			currentCell = currentCell.cellLeft;
		}
	}
	public void CreateRight(){			
		spawnPoint += new Vector3(cellSize,0,0);		
		col++;
		if(currentCell.edgeRight){
			GameObject newCell =  Instantiate(Cell,spawnPoint,Quaternion.identity);
			newCell.transform.localScale *= cellSize;
			newCell.transform.parent = this.gameObject.transform;
			newCell.transform.localPosition = spawnPoint;
			prevCell = currentCell;
			currentCell = newCell.GetComponent<PuzzleCell>();
			PopulateCell(currentCell);
			newCell.gameObject.name = col.ToString()+" : "+ row.ToString();
		}else{
			Debug.Log("Cell Already at Right");
			currentCell = currentCell.cellRight;
		}
	}
	public void CreateUp(){
		spawnPoint += new Vector3(0,cellSize,0);
		row++;
		if(currentCell.edgeUp){
			GameObject newCell =  Instantiate(Cell,spawnPoint,Quaternion.identity);
			newCell.transform.localScale *= cellSize;
			newCell.transform.parent = this.gameObject.transform;
			newCell.transform.localPosition = spawnPoint;
			prevCell = currentCell;
			currentCell = newCell.GetComponent<PuzzleCell>();
			PopulateCell(currentCell);
			newCell.gameObject.name = col.ToString()+" : "+ row.ToString();
		}else{
			Debug.Log("Cell Already at Up");
			currentCell = currentCell.cellUp;
		}
	}
	public void CreateDown(){
		spawnPoint += new Vector3(0,-cellSize,0);
		row--;
		if(currentCell.edgeDown){
			GameObject newCell =  Instantiate(Cell,spawnPoint,Quaternion.identity);
			newCell.transform.localScale *= cellSize;
			newCell.transform.parent = this.gameObject.transform;
			newCell.transform.localPosition = spawnPoint;
			prevCell = currentCell;
			currentCell = newCell.GetComponent<PuzzleCell>();
			PopulateCell(currentCell);
			newCell.gameObject.name = col.ToString()+" : "+ row.ToString();
		}else{
			Debug.Log("Cell Already at Down");
			currentCell = currentCell.cellDown;
		}
	}
	void PopulateCell(PuzzleCell mycell){
		Vector3 rightPos = mycell.gameObject.transform.position + new Vector3(+cellSize,0,0);		
		Vector3 leftPos = mycell.gameObject.transform.position + new Vector3(-cellSize,0,0);	
		Vector3 upPos = mycell.gameObject.transform.position + new Vector3(0,cellSize,0);	
		Vector3 downPos = mycell.gameObject.transform.position + new Vector3(0,-cellSize,0);
		mycell.edgeDown = mycell.edgeLeft = mycell.edgeRight = mycell.edgeUp = true;
		PuzzleCell[] cells = this.GetComponentsInChildren<PuzzleCell>();
		foreach (PuzzleCell cell in cells)
		{
			if(cell.gameObject.transform.position == rightPos){
				mycell.cellRight = cell;
				mycell.edgeRight = false;
				cell.cellLeft = mycell;
				cell.edgeLeft = false;
			}else if(cell.gameObject.transform.position == leftPos){
				mycell.cellLeft = cell;
				mycell.edgeLeft = false;
				cell.cellRight = mycell;
				cell.edgeRight = false;
			}else if(cell.gameObject.transform.position == upPos){
				mycell.cellUp = cell;
				mycell.edgeUp = false;
				cell.cellDown = mycell;
				cell.edgeDown = false;
			}else if(cell.gameObject.transform.position == downPos){
				mycell.cellDown = cell;
				mycell.edgeDown = false;
				cell.cellUp = mycell;
				cell.edgeUp = false;
			}
		}
	}
	public void RemoveSprites(){
		PuzzleCell[] cells = this.GetComponentsInChildren<PuzzleCell>();
		foreach (PuzzleCell cell in cells){
			cell.gameObject.GetComponent<SpriteRenderer>().enabled = false;
		}
	}
	public void EnableSprites(){
		PuzzleCell[] cells = this.GetComponentsInChildren<PuzzleCell>();
		foreach (PuzzleCell cell in cells){
			cell.gameObject.GetComponent<SpriteRenderer>().enabled = true;
		}
	}
	public void RemoveColliders(){
		PuzzleCell[] cells = this.GetComponentsInChildren<PuzzleCell>();
		foreach (PuzzleCell cell in cells){
			cell.gameObject.GetComponent<BoxCollider2D>().enabled = false;
		}
	}
	public void EnableColliders(){
		PuzzleCell[] cells = this.GetComponentsInChildren<PuzzleCell>();
		foreach (PuzzleCell cell in cells){
			cell.gameObject.GetComponent<BoxCollider2D>().enabled = true;
		}
	}
}
