using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HatShopButton : MonoBehaviour {
	public HatShopCell TopLeftCell, TopRightCell, BottomLeftCell, BottomRightCell;
	public void RotateItems(){
		TopLeftCell.MoveItemRight();
		TopRightCell.MoveItemDown();
		BottomLeftCell.MoveItemUp();
		BottomRightCell.MoveItemLeft();
	}
}
