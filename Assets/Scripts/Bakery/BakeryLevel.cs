using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BakeryLevel : MonoBehaviour {

	public bool levelComplete, movingBaguet;
	public BakeryBaguette[] myBaguettes;
	public BakeryGoalCell [] myGoalCells;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void ResetLevel(){
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
		movingBaguet = false;
		foreach (BakeryBaguette bagtt in myBaguettes)
		{
			bagtt.SetUpItem();
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
}
