using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeafTurnTransformPooler : MonoBehaviour {
	[Header("Other")]
	public List<GameObject> tempTransforms;
	public ParticleSystem ps;
	private int currentTempTransform;
	[Header("Scripts")]
	public ClickToRotateTile clickToRotScript;

	void Start () {
		for (int i = 0; i < tempTransforms.Count; i++)
		{
			if (tempTransforms[i] == null) {
				GameObject go = new GameObject("LeafTurnTempTransform");
				tempTransforms[i] = go;
				go.SetActive(false);
				go.transform.parent = this.gameObject.transform;
			}
		}
	}
	
	public void SetNewTransform () {
		tempTransforms[currentTempTransform].SetActive(true);
		tempTransforms[currentTempTransform].transform.position = clickToRotScript.tileClicked.transform.position;

		var main = ps.main;
		main.customSimulationSpace = tempTransforms[currentTempTransform].transform;

		currentTempTransform++;
		if (currentTempTransform > tempTransforms.Count - 1) {
			currentTempTransform = 0;
		}
	}
}
