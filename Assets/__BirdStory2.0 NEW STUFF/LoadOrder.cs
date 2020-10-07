using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadOrder : MonoBehaviour {

	public GameObject[] objectsToActivateOnStart;

	void Start() {
		StartCoroutine(StartObjectActivation());
	}

	public IEnumerator StartObjectActivation() {
		int counter = objectsToActivateOnStart.Length;
        for (int i = 0; i < counter; i++) {
            objectsToActivateOnStart[i].SetActive(true);
            yield return null;
        }
	}
}
