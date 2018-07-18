using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CafePuzzleCup : MonoBehaviour {

	public CafePuzzleCell startingCell,curretCell, nextCell;
	public bool moving;
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
	}
	
	// Update is called once per frame
	void Update () {
		if(!moving){
			if(Input.GetKey(KeyCode.UpArrow)){
				nextCell = curretCell.CheckUp();
				nextCell.occupied = true;
				nextPos = nextCell.gameObject.transform.position;
				distToPoint = Vector2.Distance(transform.position,nextPos);
				moving = true;
			}
			else if(Input.GetKey(KeyCode.DownArrow)){
				nextCell = curretCell.CheckDown();
				nextCell.occupied = true;
				nextPos = nextCell.gameObject.transform.position;
				distToPoint = Vector2.Distance(transform.position,nextPos);
				moving = true;			
			}
			else if(Input.GetKey(KeyCode.LeftArrow)){
				nextCell = curretCell.CheckLeft();
				nextCell.occupied = true;
				nextPos = nextCell.gameObject.transform.position;
				distToPoint = Vector2.Distance(transform.position,nextPos);
				moving = true;			
			}
			else if(Input.GetKey(KeyCode.RightArrow)){
				nextCell = curretCell.CheckRight();
				nextCell.occupied = true;
				nextPos = nextCell.gameObject.transform.position;
				distToPoint = Vector2.Distance(transform.position,nextPos);
				moving = true;			
			}
		}
		else{
			if(Vector2.Distance(transform.position,nextPos) <  0.05f){
				transform.position = nextPos;
				moving = false;
				curretCell.occupied = false;
				curretCell = nextCell;
			}
			else{
				transform.position = Vector2.MoveTowards(transform.position,nextPos,Time.deltaTime*(moveAnimation.Evaluate(Vector2.Distance(transform.position,nextPos)/distToPoint))*animationSpeed);
			}
		}
	}
}
