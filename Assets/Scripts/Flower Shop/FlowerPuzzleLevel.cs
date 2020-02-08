using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerPuzzleLevel : MonoBehaviour {

	public FlowerShopItem[] MyItems;
	public FlowerShopCell[] MyCells;
	public bool levelReady, levelComplete, active;
	// Use this for initialization
	void Awake(){
		
	}
	void Update(){
		
	}
	public void StartLevel(){
		
	}
	public void ResetLevel(){
		foreach (FlowerShopItem piece in MyItems)
		{
			piece.SetUpItem();
		}
		active = false;
		this.gameObject.SetActive(active);
		levelComplete = false;
	}	
	public void CheckLevelComplete(){	
		bool check = true;
		foreach (FlowerShopCell cell in MyCells)
		{
			if(!cell.occupied){
				check = false;
			}
		}
		if(check){
			foreach (FlowerShopItem piece in MyItems)
			{
				piece.CheckMatch();
				if(!piece.matched){
					check = false;
				}
			}
		}
		levelComplete = check;
	}
	public void SetUpLevel(){
		active = true;
		this.gameObject.SetActive(active);
		foreach (FlowerShopItem piece in MyItems)
		{
			piece.SetUpItem();
		}
		levelReady = true;
	}
}
