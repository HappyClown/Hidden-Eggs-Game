using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainStartLoadOrder : MonoBehaviour {

	public GameObject[] mainObjects;
	//public string mainObjectStartString;
	public bool goNext = false;
	private int frameCount;
	private bool countFrames;

	void Start() {
		countFrames = true;
		StartCoroutine(TempFrameCount());
		StartCoroutine(StartObjectActivation());
	}

	public IEnumerator StartObjectActivation() {
		int counter = mainObjects.Length;
        for (int i = 0; i < counter; i++) {
			goNext = false;
            mainObjects[i].SetActive(true);
			mainObjects[i].SendMessage("ParentObjectStart", this, SendMessageOptions.RequireReceiver);
			while (!goNext) {
            	yield return null;
			}
        }
		//print("Main Start Load Order complete. Frame Count: " + frameCount);
		countFrames = false;
	}

	IEnumerator TempFrameCount() {
		while(countFrames) {
			frameCount++;
			yield return null;
		}
	}
}