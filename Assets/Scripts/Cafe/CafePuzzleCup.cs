using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CafePuzzleCup : MonoBehaviour {

	public CafePuzzleCell startingCell,curretCell, nextCell, lastCell;
	public CafePuzzleLevel myLevel;
	public bool moving, checkGoal, reverse, active;
	public float animationSpeed;
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
	void Start () {
		Vector2 startingPos = new Vector3(startingCell.gameObject.transform.position.x, startingCell.transform.position.y);
		transform.position = startingPos;
		startingCell.occupied = true;
		curretCell = startingCell;
		checkGoal = false;
		//should be activated after level intro
		active = true;
	}
	
	// Update is called once per frame
	void Update () {
		if(moving){
			if(Vector2.Distance(transform.position,nextPos) <  0.05f){
				if(reverse){
					nextCell = lastCell;
					SetCell();
				}else{
					transform.position = nextPos;
					moving = false;
					curretCell = nextCell;
				}
			}
			else{
				transform.position = Vector2.MoveTowards(transform.position,nextPos,Time.deltaTime*(moveAnimation.Evaluate(Vector2.Distance(transform.position,nextPos)/distToPoint))*animationSpeed);
			}
		}
	}
	public void MoveCup(Vector2 target, string dir){
		if(!moving){
			if(dir == "up"){
				lastCell = curretCell;
				nextCell = curretCell.CheckUp();
				SetCell();					
			}
			else if(dir == "down"){
				lastCell = curretCell;
				nextCell = curretCell.CheckDown();
				SetCell();			
			}
			else if(dir == "left"){
				lastCell = curretCell;
				nextCell = curretCell.CheckLeft();
				SetCell();			
			}
			else if(dir == "right"){
				lastCell = curretCell;
				nextCell = curretCell.CheckRight();
				SetCell();			
			}
		}
	}
	void SetCell(){
		curretCell.occupied = false;
		if(nextCell.goalCup){
			checkGoal = true;
			if(myLevel.cupsOrder[myLevel.currentCup].ToString() == myColor.ToString()){
				myLevel.currentCup ++;
				active = false;
			}else{
				reverse = true;
			}	
		}else{
			nextCell.occupied = true;
			nextPos = nextCell.gameObject.transform.position;
		}
		distToPoint = Vector2.Distance(transform.position,nextPos);
		moving = true;
	}
}
