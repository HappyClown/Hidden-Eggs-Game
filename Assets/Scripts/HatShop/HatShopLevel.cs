using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HatShopLevel : MonoBehaviour {

	public HatShopItem[] myItems;
	public HatShopButton[] myButtons;
	public bool levelComplete, movingItem;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		bool ver = false;
		foreach (HatShopItem item in myItems)
		{
			if(item.verify && !item.moving){ 
				ver = true;
				item.verify = false;
			}
		}
		if(ver){ 
			CheckComplete();
		}
	}

	public void SetUpLevel(){
		foreach (HatShopItem item in myItems)
		{
			item.SetUpItem();
		}
	}
	public void ResetLevel(){
		levelComplete = false;
		foreach (HatShopItem item in myItems)
		{
			item.ResetItem();
		}
	}

	public void CheckComplete(){
		movingItem = false;
		bool verify = true;
		foreach (HatShopItem item in myItems)
		{
			if(!item.ongoal){
				verify = false;
			}
		}
		levelComplete = verify;
	}

}
