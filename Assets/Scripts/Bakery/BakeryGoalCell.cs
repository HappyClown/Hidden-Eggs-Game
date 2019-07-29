using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BakeryGoalCell : MonoBehaviour {


	public enum exitColor{
		red,
		yellow,
		Blue
	}
	public exitColor myColor;
	public PuzzleCell myCell;
	public void SetUpExit(){
		if(myCell.cellDown){
			myCell.cellDown.cellUp = myCell;
			myCell.cellDown.edgeUp = false;
		}else if(myCell.cellUp){
			myCell.cellUp.cellDown = myCell;
			myCell.cellUp.edgeDown = false;
		}else if(myCell.cellLeft){
			myCell.cellLeft.cellRight = myCell;
			myCell.cellLeft.edgeRight = false;
		}else if(myCell.cellRight){
			myCell.cellRight.cellLeft = myCell;
			myCell.cellRight.edgeLeft = false;
		}

	}
	public void ResetExit(){
		if(myCell.cellDown){
			myCell.cellDown.edgeUp = true;
			myCell.cellDown.cellUp = null;
		}else if(myCell.cellUp){
			myCell.cellUp.edgeDown = true;
			myCell.cellUp.cellDown = null;
		}else if(myCell.cellLeft){
			myCell.cellLeft.edgeRight = true;
			myCell.cellLeft.cellRight = null;
		}else if(myCell.cellRight){
			myCell.cellRight.edgeLeft = true;
			myCell.cellRight.cellLeft = null;
		}
	}
}
