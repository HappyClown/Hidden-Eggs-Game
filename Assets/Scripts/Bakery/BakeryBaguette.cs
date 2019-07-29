using System.Collections;
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
	public bool active;
	public Vector3 nextPos, currentPos, startPos;
	public PuzzleCell[] myCells;
	public PuzzleCell firstCell, lastCell;

	// Use this for initialization
	void Start () {
		startPos = this.gameObject.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		
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
