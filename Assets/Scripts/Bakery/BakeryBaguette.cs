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
	public bool active, pushing, canMove, onGoal, selected,directionController, movingToGoal;
	public BakeryBaguette baguetteToPush;
	public Vector3 nextPos, currentPos, startPos, iniPos, BTPcurrentPos, pushingPos;
	public PuzzleCell[] myCells, startCells;
	public PuzzleCell firstCell, lastCell, nextCell;
	public float cellDistance = 1, minDistance = 0.75f, maxDiff = 0.9f;
	private float maxY, maxX, minY, minX;
	private float iniMaxY, iniMaxX, iniMinY, iniMinX;
	public List<Vector3> positionHistory;
	public List<PuzzleCell> cellHistory;

	// Update is called once per frame
	void Update () {

	}
	#region Horizontal
	public void MoveHorizontal(float curPos, float prevPos){
		if(lastCell.CheckRight().goalCell){
			maxX = iniMaxX + cellDistance;
		}else{
			maxX = iniMaxX;
		}
		if(firstCell.CheckLeft().goalCell){
			minX = iniMinX - cellDistance;
		}else{
			minX = iniMinX;
		}
		if(!selected){
			iniPos = this.gameObject.transform.position;
			selected = true;
			nextPos = iniPos;
		}
		float Diff = Mathf.Abs(curPos - prevPos);
		if(Diff > maxDiff){ Diff = maxDiff;}
		if(curPos > prevPos){
			//moving right
			nextPos.x += Diff;
			pushingPos.x += Diff;
			if(nextPos.x > iniPos.x){
				//Is moving right from the starting point
				if(!directionController){
					directionController = true;					
					SetPushedBaguette();
					pushing = false;
					if(movingToGoal){ movingToGoal = false;}
				}
				if(!lastCell.edgeRight){
					//no board edge right
					if(!lastCell.cellRight.occupied){
						//no baguette in the next right cell
						canMove = true;
						if(lastCell.cellRight.goalCell){
							//code for checking goal
							if(lastCell.cellRight.gameObject.GetComponent<BakeryGoalCell>().myColor.ToString() == myColor.ToString()){
								canMove = true;
								movingToGoal = true;
							}else{
								canMove = false;
							}
						}
					}else{
						if(!baguetteToPush){
							baguetteToPush = lastCell.cellRight.gameObject.GetComponent<BakeryCellConn>().mybaguette;
							BTPcurrentPos = baguetteToPush.transform.position;
							float adjustedX = nextPos.x + ((myCells.Length * 0.5f) + 0.5f);
							baguetteToPush.transform.position = new Vector3(adjustedX, baguetteToPush.transform.position.y, baguetteToPush.transform.position.z);
							pushingPos = baguetteToPush.transform.position;
						}
						if(baguetteToPush.CheckAllRight()){							
							//can push right
							pushing = true;
							canMove = true;
						}else{
							// cant push right
							SetPushedBaguette();
							baguetteToPush = null;
							maxX = iniPos.x;
							canMove = false;
							this.gameObject.transform.position = iniPos;
						}
						
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
			if(Mathf.Abs(nextPos.x - iniPos.x) >= minDistance && canMove){
				List<PuzzleCell> tempCells = new List<PuzzleCell>();
				if(baguetteToPush){
					foreach (PuzzleCell cell in baguetteToPush.myCells)
					{
						tempCells.Add(cell.CheckRightAmmount(1));
						cell.occupied = false;
						cell.gameObject.GetComponent<BakeryCellConn>().mybaguette = null;
					}
					for (int i = 0; i < tempCells.Count; i++)
					{
						//occupie (hmmm pie I want pie, give me pie X if you are reading this, I know is written occupy but whatever) new baguette cells and assign baguette
						baguetteToPush.myCells[i] = tempCells[i];
						baguetteToPush.myCells[i].occupied = true;
						baguetteToPush.myCells[i].gameObject.GetComponent<BakeryCellConn>().mybaguette = baguetteToPush;
					}
					baguetteToPush.firstCell = baguetteToPush.firstCell.CheckRightAmmount(1);
					baguetteToPush.lastCell = baguetteToPush.lastCell.CheckRightAmmount(1);
					tempCells.Clear();
					BTPcurrentPos.x += cellDistance;
					baguetteToPush.iniPos = BTPcurrentPos;
					baguetteToPush.currentPos = BTPcurrentPos;
				}

				foreach (PuzzleCell cell in myCells)
				{
					//free current baguette cells and get new cells
					tempCells.Add(cell.CheckRightAmmount(1));
					cell.occupied = false;
					cell.gameObject.GetComponent<BakeryCellConn>().mybaguette = null;
				}
				for (int i = 0; i < tempCells.Count; i++)
				{
					//occupie (hmmm pie I want pie, give me pie X if you are reading this, I know is written occupy but whatever) new baguette cells and assign baguette
					myCells[i] = tempCells[i];
					myCells[i].occupied = true;
					myCells[i].gameObject.GetComponent<BakeryCellConn>().mybaguette = this;
				}
				//update edge cells
				firstCell = firstCell.CheckRightAmmount(1);
				lastCell = lastCell.CheckRightAmmount(1);
				tempCells.Clear();
				currentPos.x += cellDistance;	
				iniPos = currentPos;
				if(movingToGoal){
					foreach (PuzzleCell cell in myCells)
					{
						//free current baguette cells and get new cells
						cell.occupied = false;
						cell.gameObject.GetComponent<BakeryCellConn>().mybaguette = null;
						onGoal = true;
						this.gameObject.SetActive(false);
						canMove = false;
					}
				}
			}
		}else{
			//moving left
			nextPos.x -= Diff;
			pushingPos.x -= Diff;
			if(nextPos.x < iniPos.x){
				//Is moving left from the starting point
				if(directionController){
					directionController = false;					
					SetPushedBaguette();
					pushing = false;
					if(movingToGoal){ movingToGoal = false;}
				}
				if(!firstCell.edgeLeft){
					//no board edge up
					if(!firstCell.cellLeft.occupied){
						//no baguette in the next up cell
						canMove = true;
						if(firstCell.cellLeft.goalCell){
							//code for checking goal
							if(firstCell.cellLeft.gameObject.GetComponent<BakeryGoalCell>().myColor.ToString() == myColor.ToString()){
								canMove = true;
								movingToGoal = true;
							}else{
								canMove = false;
							}
						}
					}else{
						// next left cell is occupied
						if(!baguetteToPush){
							baguetteToPush = firstCell.cellLeft.gameObject.GetComponent<BakeryCellConn>().mybaguette;
							BTPcurrentPos = baguetteToPush.transform.position;
							float adjustedX = nextPos.x - ((myCells.Length * 0.5f) + 0.5f);
							baguetteToPush.transform.position = new Vector3(adjustedX, baguetteToPush.transform.position.y, baguetteToPush.transform.position.z);
							pushingPos = baguetteToPush.transform.position;
						}
						if(baguetteToPush.CheckAllLeft()){							
							//can push left
							pushing = true;
							canMove = true;
						}else{
							// cant push left
							SetPushedBaguette();
							baguetteToPush = null;
							minX = iniPos.x;
							canMove = false;
							this.gameObject.transform.position = iniPos;
						}
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

			if(Mathf.Abs(nextPos.x - iniPos.x) >= minDistance && canMove){
				List<PuzzleCell> tempCells = new List<PuzzleCell>();
				if(baguetteToPush){
					foreach (PuzzleCell cell in baguetteToPush.myCells)
					{
						tempCells.Add(cell.CheckLeftAmmount(1));
						cell.occupied = false;
						cell.gameObject.GetComponent<BakeryCellConn>().mybaguette = null;
					}
					for (int i = 0; i < tempCells.Count; i++)
					{
						//occupie (hmmm pie I want pie, give me pie X if you are reading this, I know is written occupy but whatever) new baguette cells and assign baguette
						baguetteToPush.myCells[i] = tempCells[i];
						baguetteToPush.myCells[i].occupied = true;
						baguetteToPush.myCells[i].gameObject.GetComponent<BakeryCellConn>().mybaguette = baguetteToPush;
					}
					baguetteToPush.firstCell = baguetteToPush.firstCell.CheckLeftAmmount(1);
					baguetteToPush.lastCell = baguetteToPush.lastCell.CheckLeftAmmount(1);
					tempCells.Clear();
					BTPcurrentPos.x -= cellDistance;
					baguetteToPush.iniPos = BTPcurrentPos;
					baguetteToPush.currentPos = BTPcurrentPos;
				}
				
				foreach (PuzzleCell cell in myCells)
				{
					//free current baguette cells and get new cells
					tempCells.Add(cell.CheckLeftAmmount(1));
					cell.occupied = false;
					cell.gameObject.GetComponent<BakeryCellConn>().mybaguette = null;
				}
				for (int i = 0; i < tempCells.Count; i++)
				{
					//occupie (hmmm pie I want pie, give me pie X if you are reading this, I know is written occupy but whatever) new baguette cells and assign baguette
					myCells[i] = tempCells[i];
					myCells[i].occupied = true;
					myCells[i].gameObject.GetComponent<BakeryCellConn>().mybaguette = this;
				}
				//update edge cells
				firstCell = firstCell.CheckLeftAmmount(1);
				lastCell = lastCell.CheckLeftAmmount(1);
				tempCells.Clear();
				currentPos.x -= cellDistance;	
				iniPos = currentPos;
				if(movingToGoal){
					foreach (PuzzleCell cell in myCells)
					{
						//free current baguette cells and get new cells
						cell.occupied = false;
						cell.gameObject.GetComponent<BakeryCellConn>().mybaguette = null;
						onGoal = true;
						this.gameObject.SetActive(false);
						canMove = false;
					}
				}
			}
		}
		
		if(canMove){
			if(nextPos.x > maxX){ nextPos.x = maxX;}
			if(nextPos.x < minX){ nextPos.x = minX;}
			this.gameObject.transform.position = nextPos;
			if(baguetteToPush){
				baguetteToPush.gameObject.transform.position = pushingPos;
			}
		}
		
	}
	#endregion
	#region Vertical
	public void MoveVertical(float curPos, float prevPos){		
		if(lastCell.CheckUp().goalCell){
			maxY = iniMaxY + cellDistance;
		}else{
			maxY = iniMaxY;
		}
		if(firstCell.CheckDown().goalCell){
			minY = iniMinY - cellDistance;
		}else{
			minY = iniMinY;
		}
		
		if(!selected){
			iniPos = this.gameObject.transform.position;
			selected = true;
			nextPos = iniPos;
		}
		float Diff = Mathf.Abs(curPos - prevPos);		
		if(Diff > maxDiff){ Diff = maxDiff;}
		if(curPos > prevPos){
			//moving UP
			nextPos.y += Diff;
			pushingPos.y += Diff;
			if(nextPos.y > iniPos.y){
				//Is moving UP from the starting point
				if(!directionController){
					directionController = true;					
					SetPushedBaguette();
					pushing = false;
					if(movingToGoal){ movingToGoal = false;}
				}
				if(!lastCell.edgeUp){
					//no board edge up
					if(!lastCell.cellUp.occupied){
						//no baguette in the next up cell
						canMove = true;
						if(lastCell.cellUp.goalCell){
							//code for checking goal
							if(lastCell.cellUp.gameObject.GetComponent<BakeryGoalCell>().myColor.ToString() == myColor.ToString()){
								canMove = true;
								movingToGoal = true;
							}else{
								canMove = false;
							}
						}
					}else{
						if(!baguetteToPush){
							baguetteToPush = lastCell.cellUp.gameObject.GetComponent<BakeryCellConn>().mybaguette;
							BTPcurrentPos = baguetteToPush.transform.position;
							float adjustedY = nextPos.y + ((myCells.Length * 0.5f) + 0.5f);
							baguetteToPush.transform.position = new Vector3(baguetteToPush.transform.position.x, adjustedY, baguetteToPush.transform.position.z);
							pushingPos = baguetteToPush.transform.position;
						}
						if(baguetteToPush.CheckAllUp()){							
							//can push UP
							pushing = true;
							canMove = true;
						}else{
							// cant push UP
							SetPushedBaguette();
							baguetteToPush = null;
							canMove = false;
							maxY = iniPos.y;
							this.gameObject.transform.position = iniPos;
						}
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
			if(Mathf.Abs(nextPos.y - iniPos.y) >= minDistance && canMove){
				List<PuzzleCell> tempCells = new List<PuzzleCell>();
				if(baguetteToPush){
					foreach (PuzzleCell cell in baguetteToPush.myCells)
					{
						tempCells.Add(cell.CheckUpAmmount(1));
						cell.occupied = false;
						cell.gameObject.GetComponent<BakeryCellConn>().mybaguette = null;
					}
					for (int i = 0; i < tempCells.Count; i++)
					{
						//occupie (hmmm pie I want pie, give me pie X if you are reading this, I know is written occupy but whatever) new baguette cells and assign baguette
						baguetteToPush.myCells[i] = tempCells[i];
						baguetteToPush.myCells[i].occupied = true;
						baguetteToPush.myCells[i].gameObject.GetComponent<BakeryCellConn>().mybaguette = baguetteToPush;
					}
					baguetteToPush.firstCell = baguetteToPush.firstCell.CheckUpAmmount(1);
					baguetteToPush.lastCell = baguetteToPush.lastCell.CheckUpAmmount(1);
					tempCells.Clear();
					BTPcurrentPos.y += cellDistance;
					baguetteToPush.iniPos = BTPcurrentPos;
					baguetteToPush.currentPos = BTPcurrentPos;
				}

				foreach (PuzzleCell cell in myCells)
				{
					//free current baguette cells and get new cells
					tempCells.Add(cell.CheckUpAmmount(1));
					cell.occupied = false;
					cell.gameObject.GetComponent<BakeryCellConn>().mybaguette = null;
				}
				for (int i = 0; i < tempCells.Count; i++)
				{
					//occupie (hmmm pie I want pie, give me pie X if you are reading this, I know is written occupy but whatever) new baguette cells and assign baguette
					myCells[i] = tempCells[i];
					myCells[i].occupied = true;
					myCells[i].gameObject.GetComponent<BakeryCellConn>().mybaguette = this;
				}
				//update edge cells
				firstCell = firstCell.CheckUpAmmount(1);
				lastCell = lastCell.CheckUpAmmount(1);
				tempCells.Clear();
				currentPos.y += cellDistance;	
				iniPos = currentPos;
				if(movingToGoal){
					foreach (PuzzleCell cell in myCells)
					{
						//free current baguette cells and get new cells
						cell.occupied = false;
						cell.gameObject.GetComponent<BakeryCellConn>().mybaguette = null;
						onGoal = true;
						this.gameObject.SetActive(false);
						canMove = false;
					}
				}
			}
		}else{
			//moving DOWN
			nextPos.y -= Diff;
			pushingPos.y -= Diff;
			if(nextPos.y < iniPos.y){
				//Is moving DOWN from the starting point
				if(directionController){
					directionController = false;					
					SetPushedBaguette();
					pushing = false;
					if(movingToGoal){ movingToGoal = false;}
				}
				if(!firstCell.edgeDown){
					if(!firstCell.cellDown.occupied){
						//no baguette in the next down cell
						canMove = true;
						if(firstCell.cellDown.goalCell){
							//code for checking goal
							if(firstCell.cellDown.gameObject.GetComponent<BakeryGoalCell>().myColor.ToString() == myColor.ToString()){
								canMove = true;
								movingToGoal = true;
							}else{
								canMove = false;
							}
						}
					}else{
						if(!baguetteToPush){
							baguetteToPush = firstCell.cellDown.gameObject.GetComponent<BakeryCellConn>().mybaguette;
							//baguetteToPush.transform.position = new Vector3(baguetteToPush.transform.position.x,(baguetteToPush.transform.position.y - (this.transform.position.y - iniPos.y)),baguetteToPush.transform.position.z);
							BTPcurrentPos = baguetteToPush.transform.position;
							float adjustedY = nextPos.y - ((myCells.Length * 0.5f) + 0.5f);
							baguetteToPush.transform.position = new Vector3(baguetteToPush.transform.position.x, adjustedY, baguetteToPush.transform.position.z);
							pushingPos = baguetteToPush.transform.position;
							//pushingPos.y += (this.transform.position.y - iniPos.y);
						}
						if(baguetteToPush.CheckAllDown()){							
							//can push down
							pushing = true;
							canMove = true;
						}else{
							// cant push down
							SetPushedBaguette();
							baguetteToPush = null;
							minY = iniPos.y;
							canMove = false;
							this.gameObject.transform.position = iniPos;
						}
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
			if(Mathf.Abs(nextPos.y - iniPos.y) >= minDistance && canMove){
				List<PuzzleCell> tempCells = new List<PuzzleCell>();
				if(baguetteToPush){
					foreach (PuzzleCell cell in baguetteToPush.myCells)
					{
						tempCells.Add(cell.CheckDownAmmount(1));
						cell.occupied = false;
						cell.gameObject.GetComponent<BakeryCellConn>().mybaguette = null;
					}
					for (int i = 0; i < tempCells.Count; i++)
					{
						//occupie (hmmm pie I want pie, give me pie X if you are reading this, I know is written occupy but whatever) new baguette cells and assign baguette
						baguetteToPush.myCells[i] = tempCells[i];
						baguetteToPush.myCells[i].occupied = true;
						baguetteToPush.myCells[i].gameObject.GetComponent<BakeryCellConn>().mybaguette = baguetteToPush;
					}
					baguetteToPush.firstCell = baguetteToPush.firstCell.CheckDownAmmount(1);
					baguetteToPush.lastCell = baguetteToPush.lastCell.CheckDownAmmount(1);
					tempCells.Clear();
					BTPcurrentPos.y -= cellDistance;
					baguetteToPush.iniPos = BTPcurrentPos;
					baguetteToPush.currentPos = BTPcurrentPos;
				}

				foreach (PuzzleCell cell in myCells)
				{
					//free current baguette cells and get new cells
					tempCells.Add(cell.CheckDownAmmount(1));
					cell.occupied = false;
					cell.gameObject.GetComponent<BakeryCellConn>().mybaguette = null;
				}
				for (int i = 0; i < tempCells.Count; i++)
				{
					//occupie (hmmm pie I want pie, give me pie X if you are reading this, I know is written occupy but whatever) new baguette cells and assign baguette
					myCells[i] = tempCells[i];
					myCells[i].occupied = true;
					myCells[i].gameObject.GetComponent<BakeryCellConn>().mybaguette = this;
				}
				//update edge cells
				firstCell = firstCell.CheckDownAmmount(1);
				lastCell = lastCell.CheckDownAmmount(1);
				tempCells.Clear();
				currentPos.y -= cellDistance;	
				iniPos = currentPos;
				if(movingToGoal){
					foreach (PuzzleCell cell in myCells)
					{
						//free current baguette cells and get new cells
						cell.occupied = false;
						cell.gameObject.GetComponent<BakeryCellConn>().mybaguette = null;
						onGoal = true;
						this.gameObject.SetActive(false);
						canMove = false;
					}
				}
			}
		}
		
		if(canMove){
			if(nextPos.y > maxY){ nextPos.y = maxY;}
			if(nextPos.y < minY){ nextPos.y = minY;}
			this.gameObject.transform.position = nextPos;
			if(baguetteToPush){
				baguetteToPush.gameObject.transform.position = pushingPos;
			}	
		}
	}
	#endregion
	public bool CheckAllLeft(){
		bool verify = true;
		foreach (PuzzleCell cell in myCells)
		{
			if(cell.edgeLeft){
				verify = false;
			}else if(cell.cellLeft.occupied || cell.cellLeft.goalCell)
			{ verify = false;}
		}
		return verify;
	}
	public bool CheckAllRight(){
		bool verify = true;
		foreach (PuzzleCell cell in myCells)
		{
			if(cell.edgeRight){
				verify = false;
			}else if(cell.cellRight.occupied || cell.cellRight.goalCell)
			{ verify = false;}
		}
		return verify;
	}
	public bool CheckAllUp(){
		bool verify = true;
		foreach (PuzzleCell cell in myCells)
		{
			if(cell.edgeUp){
				verify = false;
			}else if(cell.cellUp.occupied || cell.cellUp.goalCell)
			{ verify = false;}
		}
		return verify;
	}
	public bool CheckAllDown(){
		bool verify = true;
		foreach (PuzzleCell cell in myCells)
		{
			if(cell.edgeDown){
				verify = false;
			}else if(cell.cellDown.occupied || cell.cellDown.goalCell)
			{ verify = false;}
		}
		return verify;
	}
	public void SetUpItem(){
		this.gameObject.SetActive(true);
		for (int i = 0; i < startCells.Length; i++)
		{
			myCells[i] = startCells[i];
		}
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
		onGoal = false;
		movingToGoal = false;
		foreach (PuzzleCell cell in myCells)
		{	
			cell.occupied = true;
			cell.gameObject.GetComponent<BakeryCellConn>().mybaguette = this;
		}
		maxX = iniMaxX = 4 - (myCells.Length * 0.5f);
		minX = iniMinX = (myCells.Length * 0.5f) - 4;
		maxY = iniMaxY = 3 - (myCells.Length * 0.5f);
		minY = iniMinY = (myCells.Length * 0.5f) - 3;
	}
	public void ResetItem(){
		if(startPos == Vector3.zero){
			startPos = this.gameObject.transform.position;
		}		
		if(!startCells[0]){
			for (int i = 0; i < myCells.Length; i++)
			{
				startCells[i] = myCells[i];
			}
		}
		this.transform.position = startPos;
		currentPos = startPos;
		active = false;
		foreach (PuzzleCell cell in myCells)
		{	
			cell.occupied = false;
			cell.gameObject.GetComponent<BakeryCellConn>().mybaguette = null;
		}
		cellHistory.Clear();
		positionHistory.Clear();
	}
	public void SetPosition(){
		this.transform.position = currentPos;
		pushing = false;
		nextPos = currentPos;
		SetPushedBaguette();
	}
	public void SetPushedBaguette(){
		if(baguetteToPush){
			baguetteToPush.gameObject.transform.position = BTPcurrentPos;
			baguetteToPush = null;
		}
	}
	public void SaveStep(int move){
		if(positionHistory.Count > move){
			positionHistory[move] = this.gameObject.transform.position;
			cellHistory[move] = firstCell;
		}else{
			positionHistory.Add(this.gameObject.transform.position);
			cellHistory.Add(firstCell);
		}
	}
	public void StepBack(int move){
		int qty = myCells.Length;
		if(onGoal && positionHistory[move] != this.transform.position){
			onGoal = false;
			this.gameObject.SetActive(true);
		}
		if(horizontal && !onGoal){
			firstCell = cellHistory[move];
			for (int i = 0; i < qty; i++)
			{
				if(i == 0){ 
					myCells[i] = firstCell;
					myCells[i].gameObject.GetComponent<BakeryCellConn>().mybaguette = this;
					myCells[i].occupied = true;
				}
				else{
					myCells[i] = firstCell.CheckRightAmmount(i);
					myCells[i].gameObject.GetComponent<BakeryCellConn>().mybaguette = this;
					myCells[i].occupied = true;
				}
				if(i == (qty -1)){
					lastCell = myCells[i];
				}
			}
		}else if(vertical && !onGoal){
			firstCell = cellHistory[move];
			for (int i = 0; i < qty; i++)
			{
				if(i == 0){ 
					myCells[i] = firstCell;
					myCells[i].gameObject.GetComponent<BakeryCellConn>().mybaguette = this;
					myCells[i].occupied = true;
				}
				else{
					myCells[i] = firstCell.CheckUpAmmount(i);
					myCells[i].gameObject.GetComponent<BakeryCellConn>().mybaguette = this;
					myCells[i].occupied = true;
				}
				if(i == (qty -1)){
					lastCell = myCells[i];
				}
			}	
		}
		nextPos = iniPos = currentPos = positionHistory[move];
		this.transform.position = positionHistory[move];
		movingToGoal = false;
	}
}
