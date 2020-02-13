using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerShopItem : MonoBehaviour {
	//Define the flower types
	public enum FlowerType
	{
		Rose, Daisy, Lily, Peonie
	}
	public FlowerType myFlowerType;
	public FlowerShopCell myCell;
	public FlowerPuzzleLevel mylvl;
	public Vector3 startPosition = Vector3.zero;
	public bool onCell, matched;
	// Use this for initialization
	public void SetUpItem () {
		onCell = matched = false;
		if(startPosition == Vector3.zero){
			startPosition = gameObject.transform.position;
		}
		if(myCell){
			myCell.ResetFlowerCell();
		}else{
			myCell = null;
		}
	}
	 public void ResetItemPosition(){
		 onCell = false;
		 matched = false;
		 this.gameObject.transform.position = startPosition;
		 if(myCell){
			 myCell.myItem = null;
			 myCell.occupied = false;
		 }
		 myCell = null;
	 }
	 public void AsingCell(FlowerShopCell asingCell){
		myCell = asingCell;
		asingCell.myItem = this;
		asingCell.occupied = true;
		onCell = true;
		this.gameObject.transform.position = new Vector3( asingCell.gameObject.transform.position.x,asingCell.gameObject.transform.position.y, this.gameObject.transform.position.z);

	 }
	 public void CheckMatch(){
		 switch (myFlowerType)
		 {
			 case FlowerType.Rose:
			 matched = true;
			 foreach (FlowerShopItem item in mylvl.MyItems)
			 {
				 if(item.name != this.name && item.myFlowerType == FlowerType.Rose && item.onCell){
					 Vector2 pos1 = item.myCell.gameObject.transform.position;
					 Vector2 pos2 = myCell.gameObject.transform.position;
					 if(Mathf.Abs(pos1.x - pos2.x) < 2.1 && Mathf.Abs(pos1.y - pos2.y) < 2.1){
						matched = false;
					 }
				 }
			 }
			 break;
			 case FlowerType.Lily:			 
			 matched = false;
			  foreach (FlowerShopItem item in mylvl.MyItems)
			 {
				 if(item.myFlowerType == FlowerType.Rose && item.onCell){
					 Vector2 pos1 = item.myCell.gameObject.transform.position;
					 Vector2 pos2 = myCell.gameObject.transform.position;
					 if(Mathf.Abs(pos1.x - pos2.x) < 1.1 && Mathf.Abs(pos1.y - pos2.y) < 0.6){
						matched = true;
					 }
					 else if(Mathf.Abs(pos1.x - pos2.x) < 0.6 && Mathf.Abs(pos1.y - pos2.y) < 1.1){
						matched = true;
					 }
				 }
			 }
			 break;
			 case FlowerType.Daisy:
			 matched = true;
			 foreach (FlowerShopItem item in mylvl.MyItems)
			 {
				 if(item.name != this.name && item.myFlowerType == FlowerType.Daisy && item.onCell){
					 Vector2 pos1 = item.myCell.gameObject.transform.position;
					 Vector2 pos2 = this.gameObject.transform.position;
					 if(Mathf.Abs(pos1.x - pos2.x) < 1.1 && Mathf.Abs(pos1.y - pos2.y) < 1.1){
						matched = false;
					 }
				 }
			 }
			 break;
			 case FlowerType.Peonie:
			 matched = false;
			 foreach (FlowerShopItem item in mylvl.MyItems)
			 {
				 if(item.myFlowerType == FlowerType.Daisy && item.onCell){
					 Vector2 pos1 = item.myCell.gameObject.transform.position;
					 Vector2 pos2 = myCell.gameObject.transform.position;
					 if(Mathf.Abs(pos1.x - pos2.x) < 1.1 && Mathf.Abs(pos1.y - pos2.y) < 1.1 && Mathf.Abs(pos1.x - pos2.x) > 0.6 && Mathf.Abs(pos1.y - pos2.y) > 0.6){
						matched = true;
					 }
				 }
			 }
			 break;
		 }
	 }
	// Update is called once per frame
	void Update () {
		
	}
}
