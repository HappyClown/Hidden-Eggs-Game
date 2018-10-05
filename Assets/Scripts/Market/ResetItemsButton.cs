﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ResetItemsButton : MonoBehaviour 
{
	public Button resetButton;
	public GameObject[] items; 

	public Scale scaleScript;
	public GrabItem grabItemScript;


	void Start () 
	{
		resetButton = this.GetComponent<Button>();
		resetButton.onClick.AddListener(ResetItemsToTable);
		items = GameObject.FindGameObjectsWithTag("Item");

		scaleScript = GameObject.FindGameObjectWithTag("Scale").GetComponent<Scale>();
	}

	public void FillItemResetArray ()
	{
		items = GameObject.FindGameObjectsWithTag("Item");
	}

	public void ResetItemsToTable () 
	{
		if (grabItemScript.canPlay)
		{
			//Debug.Log("Reseting Items to table blip bloop.");
			scaleScript.itemOnScale = null;
			scaleScript.isAnItemOnScale = false;

			grabItemScript.curntAmnt = 0;
			grabItemScript.curntPounds = 0;
			
			for (int i = 0; i < items.Length; i ++)
			{
				items[i].GetComponent<Items>().BackToInitialPos();
				items[i].transform.parent = grabItemScript.itemHolder.transform;
			}
		}
	}
}
