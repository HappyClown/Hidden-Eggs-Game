using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HatShopItem : MonoBehaviour {
	public enum ItemType
	{
		feather,
		Bow,
		Butterfly,
		Button,
		Flower,
		thing5,
		thing6,
		thing7,
		thing8,
	}
	public ItemType myType;
	public HatShopLevel myLevel;
	public bool moving = false, verify = false, ongoal = false;
	public PuzzleCell currentCell, nextCell, initialCell;
	public float moveDur;
	private float timer;
	void Update () {
		if(moving){
			myLevel.movingItem = true;
			timer += Time.deltaTime / moveDur;
			this.transform.position = Vector3.Lerp(currentCell.transform.position, nextCell.transform.position, timer);
			if (timer >= 1) {
				timer = 0f;
				currentCell = nextCell;
				moving = false;
				currentCell.gameObject.GetComponent<HatShopCell>().myItem = this;
				verify = true;
				CheckGoal();
			}
		}
		
	}
	public void SetUpItem()	{
		currentCell = initialCell;
		currentCell.gameObject.GetComponent<HatShopCell>().myItem = this;
		this.transform.position = initialCell.transform.position;
		moving = false;
		timer = 0;
		verify = false;
		CheckGoal();
	}
	public void ResetItem()	{
		currentCell = initialCell;
		currentCell.gameObject.GetComponent<HatShopCell>().myItem = this;
		this.transform.position = initialCell.transform.position;
		moving = false;
		timer = 0;
		verify = false;
		CheckGoal();
	}
	public void CheckGoal(){
		ongoal = false;
		for (int i = 0; i < currentCell.gameObject.GetComponent<HatShopCell>().myTypes.Length; i++)
		{
			if(myType.ToString() == currentCell.gameObject.GetComponent<HatShopCell>().myTypes[i].ToString()){
				ongoal = true;
			}
		}
	}
}
