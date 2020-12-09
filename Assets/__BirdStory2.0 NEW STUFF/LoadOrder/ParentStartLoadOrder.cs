using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentStartLoadOrder : MonoBehaviour {
	//[Header("Scripts")]
	//public ClickOnEggs clickOnEggsScript;
	[Header("Load Order Stuff")]
	public GameObject[] parentObjects;
	public bool goNext = false;
	private MainStartLoadOrder mainStartLoadOrder;

	void ParentObjectStart (MainStartLoadOrder mainStartScript) {
		mainStartLoadOrder = mainStartScript;
		StartCoroutine(StartObjectActivation());
	}

	public IEnumerator StartObjectActivation() {
		int counter = parentObjects.Length;
        for (int i = 0; i < counter; i++) {
			goNext = false;
            parentObjects[i].SetActive(true);
			parentObjects[i].SendMessage("ChildObjectStart", this, SendMessageOptions.RequireReceiver);
			while (!goNext) {
            	yield return null;
			}
        }
		mainStartLoadOrder.goNext = true;
	}
}
