using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CafePuzzleLevel : MonoBehaviour {

	// Use this for initialization
	public CafePuzzleCup[] myCups;
	public CafePuzzleObstacle[] myObstacles;
	public CafePuzzleCell cellGoal;
	public int levelNumber;
	public bool starting, finishing, loaded,finished, movigCups, setOrder, levelComplete;
	public float introTime, exitTime;
	private float currentintroTime, currentExitTime;
	public int currentCup;
	public enum cupColor{
		Red,
		blue,
		green
	}
	public cupColor[] cupsOrder;
	public int requiredCups, cupsLeft;
	public float cupsToMove, order;
	public string colorToMove, direction;
	
	// Update is called once per frame
	void Update () {
		if(starting){
			introAnimation();
		}
		if(cupsLeft == 0){
			//Space for finishing animation
			if(!finishing){
				loaded = false;
				finishing = true;
				levelComplete = true;
			}
		}
		if(finishing){
			exitAnimation();
		}
		if(cupsToMove > 0){
			setOrder = false;
			for (int i = 0; i < myCups.Length; i++)
			{
				if(myCups[i].myColor.ToString() == colorToMove){
					if(direction == "left" || direction == "down")
					{
						order ++;
						if(order == cupsToMove){
							cupsToMove = 0;
						}
					}
					else if(direction == "right" || direction == "up")
					{
						order --;
						if(order == 1){
							cupsToMove = 0;
						}
					}
					if(direction == "left" || direction == "right")
					{	
						for (int j = 0; j < myCups.Length; j++)
						{					
							if(myCups[j].hMovingOrder == order && myCups[j].myColor.ToString() == colorToMove)
							{
								myCups[j].MoveCup(direction);
							}
						}
					}
					else if(direction == "down" || direction == "up")
					{	
						for (int j = 0; j < myCups.Length; j++)
						{
							if(myCups[j].vMovingOrder == order && myCups[j].myColor.ToString() == colorToMove)
							{
								myCups[j].MoveCup(direction);
							}
						}
					}
				}
			}
		}
		movigCups = false;
		for (int i = 0; i < myCups.Length; i++)
		{	
			if(myCups[i].moving){
				 movigCups = true;
			}
		}
		if(!movigCups && !setOrder){
			for (int i = 0; i < myCups.Length; i++)
			{	
				myCups[i].SetOrder();
			}
			setOrder = true;
		}
	}
	public void SetUpLevel(){
		loaded = finishing =  finished = false;
		currentCup = 0;
		cupsToMove = 0;
		setOrder = false;
		movigCups = false;
		starting = true;
		currentintroTime = 0;
		cupsLeft = requiredCups;
		cellGoal.cellLeft.edgeRight = false;
		cellGoal.cellLeft.cellRight = cellGoal;
		for (int i = 0; i < myCups.Length; i++)
		{
			myCups[i].SetUp();
		}
		for (int i = 0; i < myObstacles.Length; i++)
		{
			myObstacles[i].SetUp();
		}

	}
	public void ResetLevel(){
		cellGoal.cellLeft.edgeRight = true;
		cellGoal.cellLeft.cellRight = null;
		levelComplete = false;
		for (int i = 0; i < myCups.Length; i++)
		{
			myCups[i].SetUp();
		}
		for (int i = 0; i < myObstacles.Length; i++)
		{
			myObstacles[i].SetUp();
		}
	}
	public void Swipe(string color, string dir){
		cupsToMove = 0;
		colorToMove = color;
		for (int i = 0; i < myCups.Length; i++)
		{	
			if(myCups[i].myColor.ToString() == color){
				cupsToMove ++;
			}
		}
		if(dir == "left" || dir == "down")
		{
			order = 0;
		}
		else if(dir == "right" || dir == "up")
		{
			order = cupsToMove + 1;
		}
		direction = dir;
	}
	public void introAnimation(){
		currentintroTime += Time.deltaTime;
		if(currentintroTime >= introTime){
			starting = false;
			loaded = true;
		}
	}
	public void exitAnimation(){
		currentExitTime += Time.deltaTime;
		cellGoal.cellLeft.edgeRight = true;
		cellGoal.cellLeft.cellRight = null;
		if (currentExitTime >= exitTime)
		{
			finishing = false;
			finished = true;
		}
	}
}
