using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CafePuzzleCup : MonoBehaviour {

	public CafePuzzleCell startingCell,curretCell, nextCell, lastCell;
	public CafePuzzleLevel myLevel;
	public bool moving, checkGoal, reverse, active;
	public float animationSpeed, hMovingOrder, vMovingOrder;
	public enum cupColor{
		Red,
		blue,
		green
	}
	public cupColor myColor;
	private float remainingDist, distToPoint;
	private Vector2 nextPos,currentPos;
	public AnimationCurve moveAnimation;
	// Use this for initialization
	public void SetUp() {
		Vector2 startingPos = new Vector3(startingCell.gameObject.transform.position.x, startingCell.transform.position.y);
		transform.position = startingPos;
		startingCell.occupied = true;
		curretCell = startingCell;
		//should be activated after level intro
		active = true;
	}
	
	// Update is called once per frame
	void Update () {
		if(moving){
			if(reverse){
				for (int i = 0; i < myLevel.myCups.Length; i++)
				{
					if(myLevel.myCups[i].myColor == myColor && myLevel.myCups[i].active){
						myLevel.myCups[i].nextCell.occupied = false;
						myLevel.myCups[i].nextCell = myLevel.myCups[i].lastCell;
						myLevel.myCups[i].SetCell();
					}
				}
				reverse = false;
			}
			if(Vector2.Distance(transform.position,nextPos) <  0.05f){
					transform.position = nextPos;
					moving = false;
					curretCell = nextCell;
					if(nextCell.goalCup){
						Debug.Log("here");
						myLevel.cupsLeft --;
						active = false;
						lastCell = curretCell;
					}
			}
			else{
				transform.position = Vector2.MoveTowards(transform.position,nextPos,Time.deltaTime*(moveAnimation.Evaluate(Vector2.Distance(transform.position,nextPos)/distToPoint))*animationSpeed);
			}
		}
	}
	public void MoveCup(string dir){
		if(!moving && active){
			if(dir == "up"){
				//Debug.Log("Moving up!");
				lastCell = curretCell;
				nextCell = curretCell.CheckUp();
				SetCell();					
			}
			else if(dir == "down"){
				//Debug.Log("Moving down!");
				lastCell = curretCell;
				nextCell = curretCell.CheckDown();
				SetCell();			
			}
			else if(dir == "left"){
				//Debug.Log("Moving left!");
				lastCell = curretCell;
				nextCell = curretCell.CheckLeft();
				SetCell();			
			}
			else if(dir == "right"){
				//Debug.Log("Moving right!");
				lastCell = curretCell;
				nextCell = curretCell.CheckRight();
				SetCell();			
			}
		}
	}
	public void SetCell(){
		curretCell.occupied = false;
		nextPos = nextCell.gameObject.transform.position;
		if(nextCell.goalCup){
			if(myLevel.cupsOrder[myLevel.currentCup].ToString() == myColor.ToString()){
				Debug.Log(myLevel.cupsOrder[myLevel.currentCup].ToString());
				Debug.Log(myColor.ToString());
				myLevel.currentCup ++;
				if(myLevel.currentCup == myLevel.cupsOrder.Length){
					myLevel.currentCup = 0;
				}
			}else{
				reverse = true;
			}	
		}else{
			nextCell.occupied = true;
		}
		distToPoint = Vector2.Distance(transform.position,nextPos);
		moving = true;
	}
	public void SetOrder()
	{
		float cupsWithColor = 0;
		int lessX = 0;
		int lessY = 0;
		for (int i = 0; i < myLevel.myCups.Length; i++)
		{
			if(myLevel.myCups[i].myColor.ToString() == myColor.ToString())
			{
				cupsWithColor ++;
				if(myLevel.myCups[i].gameObject.transform.position.x > gameObject.transform.position.x)
				{
					lessX ++;
				}
				else if(myLevel.myCups[i].gameObject.transform.position.x == gameObject.transform.position.x){
					if(myLevel.myCups[i].gameObject.transform.position.y < gameObject.transform.position.y)
					{
						lessX ++;
					}
				}
				if(myLevel.myCups[i].gameObject.transform.position.y > gameObject.transform.position.y)
				{
					lessY ++;
				}
				else if(myLevel.myCups[i].gameObject.transform.position.y == gameObject.transform.position.y){
					
					if(myLevel.myCups[i].gameObject.transform.position.x < gameObject.transform.position.x)
					{
						lessY ++;
					}
				}
			}
		}
		hMovingOrder = cupsWithColor - lessX;
		vMovingOrder = cupsWithColor - lessY;
	}
}
