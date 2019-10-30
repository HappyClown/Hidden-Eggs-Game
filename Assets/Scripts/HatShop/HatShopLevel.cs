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
			if(!item.moving){ ver = true;}
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

	}

	public void CheckComplete(){
		movingItem = false;
		bool verify = false;
		foreach (HatShopItem item in myItems)
		{
			foreach (HatShopItem.ItemType type in item.currentCell.gameObject.GetComponent<HatShopCell>().myTypes)
			{
				
				if (type.ToString() == item.myType.ToString())
				{
					//verify = true;
					Debug.Log(type.ToString() +"   "+  item.myType.ToString());
				}
			}
		}
		levelComplete = verify;
	}

}
