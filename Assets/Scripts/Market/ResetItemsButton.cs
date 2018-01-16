using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ResetItemsButton : MonoBehaviour 
{
	public Button resetButton;
	public GameObject[] items; 

	void Start () 
	{
		resetButton = this.GetComponent<Button>();
		resetButton.onClick.AddListener(ResetItemsToTable);
		items = GameObject.FindGameObjectsWithTag("Item");
	}
	
	public void ResetItemsToTable () 
	{
		//Debug.Log("Reseting Items to table blip bloop.");
		
		for (int i = 0; i < items.Length; i ++)
		{
			items[i].GetComponent<Items>().BackToInitialPos();
		}
	}
}
