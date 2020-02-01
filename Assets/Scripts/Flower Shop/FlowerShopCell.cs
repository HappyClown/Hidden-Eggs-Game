using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerShopCell : PuzzleCell {
	public FlowerShopItem myItem;
	// Use this for initialization
	public void ResetFlowerCell(){
		occupied = false;
		myItem = null;
	}
}
