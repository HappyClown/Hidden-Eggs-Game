using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HatShopCell : MonoBehaviour {

	public HatShopItem.ItemType[] myTypes;
	public HatShopItem myItem;
	public PuzzleCell myCell;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void MoveItemLeft(){
		myItem.nextCell = myCell.CheckLeftAmmount(1);
		myItem.moving = true;
	}
	public void MoveItemRight(){
		myItem.nextCell = myCell.CheckRightAmmount(1);
		myItem.moving = true;
	}
	public void MoveItemUp(){
		myItem.nextCell = myCell.CheckUpAmmount(1);
		myItem.moving = true;
	}
	public void MoveItemDown(){
		myItem.nextCell = myCell.CheckDownAmmount(1);
		myItem.moving = true;
	}
}
