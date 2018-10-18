using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResetItemsButton : MonoBehaviour 
{
	public Button resetButton;
	public List<GameObject> items;

	public Scale scaleScript;
	public GrabItem grabItemScript;


	void Start () 
	{
		resetButton = this.GetComponent<Button>();
		resetButton.onClick.AddListener(ResetItemsToTable);
	}

	public void FillItemResetArray ()
	{
		items.Clear();
		foreach (Transform item in grabItemScript.itemHolder.transform)
		{
			items.Add(item.gameObject);
		}
	}

	public void ResetItemsToTable () // Used for the reset items button
	{
		if (grabItemScript.canPlay)
		{
			//Debug.Log("Reseting Items to table blip bloop.");
			scaleScript.itemOnScale = null;
			scaleScript.isAnItemOnScale = false;

			grabItemScript.curntAmnt = 0;
			grabItemScript.curntPounds = 0;
			
			for (int i = 0; i < items.Count; i ++)
			{
				items[i].GetComponent<Items>().BackToInitialPos();
				items[i].transform.parent = grabItemScript.itemHolder.transform;
			}
		}
	}

	public void EndOfLevelReset() // Used in new level setup
	{
		scaleScript.itemOnScale = null;
		scaleScript.isAnItemOnScale = false;

		for (int i = 0; i < items.Count; i ++)
		{
			//Debug.Log("Should reset items");
			//if (!items[i].gameObject.activeSelf){ items[i].gameObject.SetActive(true); Debug.Log("Should set to true");} 
			items[i].GetComponent<Items>().BackToInitialPos();
			items[i].transform.parent = grabItemScript.itemHolder.transform;
		}
	}
}
