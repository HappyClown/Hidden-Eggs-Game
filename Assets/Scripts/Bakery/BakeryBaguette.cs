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
	public int timesToMove;
	public bool active, gettingPushed, canMove, onGoal, selected;
	public Vector3 nextPos, currentPos, startPos, iniPos;
	public PuzzleCell[] myCells;
	public PuzzleCell firstCell, lastCell, nextCell;
	public float cellDistance = 1, minDistance = 0.75f;
	private float maxY = 2.5f, maxX = 3.5f, minY = -2.5f, minX = -3.5f;

	// Use this for initialization
	void Start () {
		startPos = this.gameObject.transform.position;
		currentPos = startPos;
	}
	
	// Update is called once per frame
	void Update () {

	}
	public void MoveHorizontal(float curPos, float prevPos){
		if(!selected){
			iniPos = this.gameObject.transform.position;
			selected = true;
			nextPos = iniPos;
		}
		float Diff = Mathf.Abs(curPos - prevPos);
		if(curPos > prevPos){
			//moving right
			nextPos.x += Diff;
			if(nextPos.x > iniPos.x){
				//Is moving right from the starting point
				if(!lastCell.edgeRight){
					if(!lastCell.cellRight.occupied){
						if(Mathf.Abs(nextPos.x - iniPos.x) >= minDistance && canMove){
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
							currentPos.x += cellDistance;
							iniPos = currentPos;
						}
						canMove = true;
					}else{
						maxX = iniPos.x;
						canMove = false;
						this.gameObject.transform.position = iniPos;
					}
				}else{
					maxX = iniPos.x;
					canMove = false;
					this.gameObject.transform.position = iniPos;
				}
			}else if(nextPos.x > this.gameObject.transform.position.x){
		    	//Is moving right towards the starting point
				canMove = true;
			}
		}else{
			//moving left
			nextPos.x -= Diff;
			if(nextPos.x < iniPos.x){
				//Is moving left from the starting point
				if(!firstCell.edgeLeft){
					if(!firstCell.cellLeft.occupied){
						if(Mathf.Abs(nextPos.x - iniPos.x) >= minDistance && canMove){
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
							currentPos.x -= cellDistance;	
							iniPos = currentPos;
						}
						canMove = true;
					}else{
						minX = iniPos.x;
						canMove = false;
						this.gameObject.transform.position = iniPos;
					}
				}else{
					minX = iniPos.x;
					canMove = false;
					this.gameObject.transform.position = iniPos;
				}
			}else if(nextPos.x < this.gameObject.transform.position.x) {
		    	//Is moving left towards the starting point
				canMove = true;
			}
		}
		
		if(canMove){
			if(nextPos.x > maxX){ nextPos.x = maxX;}
			if(nextPos.x < minX){ nextPos.x = minX;}
			this.gameObject.transform.position = nextPos;	
		}
		
	}
	public void MoveVertical(float curPos, float prevPos){
		if(!selected){
			iniPos = this.gameObject.transform.position;
			selected = true;
			nextPos = iniPos;
		}
		float Diff = Mathf.Abs(curPos - prevPos);
		if(curPos > prevPos){
			//moving UP
			nextPos.y += Diff;
			if(nextPos.y > iniPos.y){
				//Is moving UP from the starting point
				if(!lastCell.edgeUp){
					if(!lastCell.cellUp.occupied){
						if(Mathf.Abs(nextPos.y - iniPos.y) >= minDistance && canMove){
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
							currentPos.y += cellDistance;
							iniPos = currentPos;
						}
						canMove = true;
					}else{
						canMove = false;
						maxY = iniPos.y;
						this.gameObject.transform.position = iniPos;
					}
				}else{
					canMove = false;
					maxY = iniPos.y;
					this.gameObject.transform.position = iniPos;
				}
			}else if(nextPos.y > this.gameObject.transform.position.y){
		    	//Is moving UP towards the starting point
				canMove = true;
			}
		}else{
			//moving DOWN
			nextPos.y -= Diff;
			if(nextPos.y < iniPos.y){
				//Is moving DOWN from the starting point
				if(!firstCell.edgeDown){
					if(!firstCell.cellDown.occupied){
						if(Mathf.Abs(nextPos.y - iniPos.y) >= minDistance && canMove){
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
							currentPos.y -= cellDistance;	
							iniPos = currentPos;
						}
						canMove = true;
					}else{
						minY = iniPos.y;
						canMove = false;
						this.gameObject.transform.position = iniPos;
					}
				}else{
					canMove = false;
					minY=iniPos.y;
					this.gameObject.transform.position = iniPos;
				}
			}else if(nextPos.y < this.gameObject.transform.position.y) {
		    	//Is moving DOWN towards the starting point
				canMove = true;
			}
		}
		
		if(canMove){
			if(nextPos.y > maxY){ nextPos.y = maxY;}
			if(nextPos.y < minY){ nextPos.y = minY;}
			this.gameObject.transform.position = nextPos;	
		}
	}
	/*public void MoveLeft(){
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
	}*/
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
	public void SetPosition(){
		this.transform.position = currentPos;
	}
}
