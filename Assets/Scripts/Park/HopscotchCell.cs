using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HopscotchCell : MonoBehaviour {

	private Collider2D myCollider;
	public HopscotchCell nextCell;
	public int myNumber;
	public bool doubleCell, goalCell;
	public HopscotchCell requiredCell;
	public bool tapped = false;

	void Start () {
		myCollider = gameObject.GetComponent<Collider2D>();
	}
	
	void Update () {
		//tapped = false;
	}
	public void checkCell(){
		
		if(doubleCell){
			tapped = true;
			Debug.Log("double cell");
			if(requiredCell.tapped){
				myCollider.enabled = false;
				requiredCell.myCollider.enabled=false;
				nextCell.myCollider.enabled = true;
				Debug.Log("Tapped BOTH tiles succesfully");
				tapped = false;
			}
		}
		else{
			if(myNumber != 1){
				myCollider.enabled = false;
			}
			nextCell.myCollider.enabled = true;
			if(nextCell.doubleCell){
				nextCell.requiredCell.myCollider.enabled = true;
			}
			Debug.Log("Tapped ONE tiles succesfully");
		}
		
	}
	public void ResetCell(){
		if(myNumber != 1){
			myCollider.enabled = false;
			tapped = false;
		}
	}
}
