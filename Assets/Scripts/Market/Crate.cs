using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class Crate : MonoBehaviour 
{
	public GameObject crateSnapPos;

	public TextMeshProUGUI pounds;
	public TextMeshProUGUI amntOfItems;

	public float reqPoundsLvl1;
	public float reqItemsLvl1;

	public float reqPoundsLvl2;
	public float reqItemsLvl2;

	public float reqPoundsLvl3;
	public float reqItemsLvl3;

	public float reqPounds;
	public float reqItems;

	public int curntLvl;

	public void UpdateRequirements()
	{
		if (curntLvl == 1) { reqPounds = reqPoundsLvl1; reqItems = reqItemsLvl1; }

		if (curntLvl == 2) { reqPounds = reqPoundsLvl2; reqItems = reqItemsLvl2; }

		if (curntLvl == 3) { reqPounds = reqPoundsLvl3; reqItems = reqItemsLvl3; }

		pounds.text = reqPounds + " pounds";
		amntOfItems.text = reqItems + " items";
	}
}
