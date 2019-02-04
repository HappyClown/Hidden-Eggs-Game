using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResetItemsButton : MonoBehaviour {
	public Button resetButton;
	public List<GameObject> items;
	public Scale scaleScript;
	public MarketPuzzleEngine marketPuzzScript;
	public SceneTapEnabler sceneTapEnaScript;

	void Start () {
		resetButton = this.GetComponent<Button>();
		resetButton.onClick.AddListener(ResetItemsToTable);
	}

	void Update () {
		if (!sceneTapEnaScript.canTapPauseBtn) {
			resetButton.interactable = false;
		}
		else {
			resetButton.interactable = true;
		}
	}

	public void FillItemResetArray () {
		items.Clear();
		foreach (Transform item in marketPuzzScript.itemHolder.transform) {
			items.Add(item.gameObject);
		}
	}

	public void ResetItemsToTable () { // Used for the reset items button 
		if (marketPuzzScript.canPlay) {
			scaleScript.itemOnScale = null;
			scaleScript.isAnItemOnScale = false;

			marketPuzzScript.curntAmnt = 0;
			marketPuzzScript.curntPounds = 0;
			
			for (int i = 0; i < items.Count; i ++) {
				items[i].GetComponent<Items>().BackToInitialPos();
				items[i].transform.parent = marketPuzzScript.itemHolder.transform;
			}
		}
	}

	public void EndOfLevelReset() { // Used in new level setup
		scaleScript.itemOnScale = null;
		scaleScript.isAnItemOnScale = false;

		for (int i = 0; i < items.Count; i ++) {
			items[i].GetComponent<Items>().BackToInitialPos();
			items[i].transform.parent = marketPuzzScript.itemHolder.transform;
		}
	}
}
