using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleCell : MonoBehaviour {

	//We have to asing by hand the cells next to it, if there is no cell we leave it null
	public PuzzleCell cellUp;
	public PuzzleCell cellDown;
	public PuzzleCell cellLeft;
	public PuzzleCell cellRight;
	//the bools define if the cell is next to an edge of the board or if it is the goal;
	public bool edgeUp, edgeDown, edgeLeft, edgeRight;
	//the occupied bool defines if the cell has something on it
	public bool occupied, goalCell;
	//Ammount of times it goes the check
	public int CheckTimes;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	//when the player moves a cup, the cells will check by functions if its cells next to it are occupied
	public PuzzleCell CheckUp( int myNum = 0){
		CheckTimes = myNum;
		//The cell returns itself if the cell next to it in the selected direction is occupied or if there is an edge 
		if(edgeUp || goalCell){
			return this;
		}
		else if(cellUp.occupied){
			return this;
		}
		else{
			myNum ++;
		//if not, the next cell will repeat the process. the same script applies for each direction.
			return cellUp.CheckUp( myNum);
		}
	}
	public PuzzleCell CheckDown( int myNum = 0){
		CheckTimes = myNum;
		if(edgeDown || goalCell){
			return this;
		}
		else if(cellDown.occupied){
			return this;
		}
		else{
			myNum ++;
			return cellDown.CheckDown(myNum);
		}
	}
	public PuzzleCell CheckLeft( int myNum = 0){
		CheckTimes = myNum;
		if(edgeLeft || goalCell){
			return this;
		}
		else if(cellLeft.occupied){
			return this;
		}
		else{
			myNum ++;
			return cellLeft.CheckLeft(myNum);
		}
	}
	public PuzzleCell CheckRight( int myNum = 0){
		CheckTimes = myNum;
		if(edgeRight || goalCell){
			return this;
		}
		else if(cellRight.occupied){
			return this;
		}
		else{
			myNum ++;
			return cellRight.CheckRight(myNum);
		}
	}
	public PuzzleCell CheckUpAmmount(int times){
		//The cell returns itself if the cell next to it in the selected direction is occupied or if there is an edge 
		if(times == 0){
			return this;
		}
		else {
		//if not, the next cell will repeat the process. the same script applies for each direction.
			return cellUp.CheckUpAmmount(times - 1);
		}
	}
	public PuzzleCell CheckDownAmmount(int times){
		if(times == 0){
			return this;
		}
		else {
			return cellDown.CheckDownAmmount(times - 1);
		}
	}
	public PuzzleCell CheckLeftAmmount(int times){
		if(times == 0){
			return this;
		}
		else {
			return cellLeft.CheckLeftAmmount(times - 1);
		}
	}
	public PuzzleCell CheckRightAmmount(int times){
		if(times == 0){
			return this;
		}
		else {
			return cellRight.CheckRightAmmount( times - 1);
		}
	}
}
