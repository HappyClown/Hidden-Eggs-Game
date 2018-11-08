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
	// Use this for initialization
	void Start () {
		myCollider = gameObject.GetComponent<Collider2D>();
	}
	
	// Update is called once per frame
	void Update () {
		tapped = false;
	}
	public void checkCell(){
		
		if(doubleCell){
			tapped = true;
			if(requiredCell.tapped){
				myCollider.enabled = false;
				requiredCell.myCollider.enabled=false;
				nextCell.myCollider.enabled = true;
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
		}
		
	}
	public void ResetCell(){		
		if(myNumber != 1){
			myCollider.enabled = false;
		}
	}
}
