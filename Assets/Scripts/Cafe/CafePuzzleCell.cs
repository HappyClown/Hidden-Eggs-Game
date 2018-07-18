using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CafePuzzleCell : MonoBehaviour {

	//We have to asing by hand the cells next to it, if there is no cell we leave it null
	public CafePuzzleCell cellUp;
	public CafePuzzleCell cellDown;
	public CafePuzzleCell cellLeft;
	public CafePuzzleCell cellRight;
	//the bools define if the cell is next to an edge of the board
	public bool edgeUp, edgeDown, edgeLeft, edgeRight;
	//the ovvupied bool defines if the cell has something on it
	public bool occupied;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	//when the player moves a cup, the cells will check by functions if its cells next to it are occupied
	public CafePuzzleCell CheckUp(){
		//The cell returns itself if the cell next to it in the selected direction is occupied or if there is an edge 
		if(edgeUp){
			return this;
		}
		else if(cellUp.occupied){
			return this;
		}
		else{
		//if not, the next cell will repeat the process. the same script applies for each direction.
			return cellUp.CheckUp();
		}
	}
	public CafePuzzleCell CheckDown(){
		if(edgeDown){
			return this;
		}
		else if(cellDown.occupied){
			return this;
		}
		else{
			return cellDown.CheckDown();
		}
	}
	public CafePuzzleCell CheckLeft(){
		if(edgeLeft){
			return this;
		}
		else if(cellLeft.occupied){
			return this;
		}
		else{
			return cellLeft.CheckLeft();
		}
	}
	public CafePuzzleCell CheckRight(){
		if(edgeRight){
			return this;
		}
		else if(cellRight.occupied){
			return this;
		}
		else{
			return cellRight.CheckRight();
		}
	}
}
