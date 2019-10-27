using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BakeryLevel : MonoBehaviour {

	public bool levelComplete, movingBaguet;
	public BakeryBaguette[] myBaguettes;
	public BakeryGoalCell [] myGoalCells;
	public int moveCount;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void ResetLevel(){
		moveCount = 0;
		levelComplete = false;
		foreach (BakeryBaguette bagtt in myBaguettes)
		{
			bagtt.ResetItem();
		}
		foreach (BakeryGoalCell goals in myGoalCells)
		{
			goals.ResetExit();
		}
	}
	public void SetUpLevel(){
		moveCount = 0;
		movingBaguet = false;
		foreach (BakeryBaguette bagtt in myBaguettes)
		{
			bagtt.SetUpItem();
			bagtt.SaveStep(moveCount);
		}
		foreach (BakeryGoalCell goals in myGoalCells)
		{
			goals.SetUpExit();
		}
	}
	public void CheckBaguettes(){
		bool check = true;
		foreach (BakeryBaguette bagtt in myBaguettes)
		{
			if(!bagtt.onGoal){
				check = false;
			}
		}
		levelComplete = check;
	}
	public void SaveStep(){
		bool ver = false;
		foreach (BakeryBaguette bagtt in myBaguettes)
		{
			if(bagtt.firstCell != bagtt.cellHistory[moveCount]){
				ver = true;
			}
		}
		if(ver){
			moveCount += 1;
			foreach (BakeryBaguette bagtt in myBaguettes)
			{
				bagtt.SaveStep(moveCount);
			}
		}
	}
	public void StepBack(){	
		if(moveCount > 0){
			moveCount -= 1;	
			foreach (BakeryBaguette bagtt in myBaguettes)
			{
				foreach (PuzzleCell cell in bagtt.myCells )
				{
					cell.occupied = false;
					cell.gameObject.GetComponent<BakeryCellConn>().mybaguette = null;
				}
			}
			foreach (BakeryBaguette bagtt in myBaguettes)
			{
				bagtt.StepBack(moveCount);
			}
		}
	}
}
