using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeafTurnPooler : MonoBehaviour {
	// This script exists because the Velocity Over Lifetime parameter of the LeafTurnFX
	// uses the objects transform as a reference for what to rotate around. I tried putting
	// in a Custom Simulation Space transform using the same "pooling" method to no avail.
	[Header("Other")]
	public GameObject leafTurnFXGORef;
	public List<GameObject> leafTurnFXGOs;
	private int currentTempTransform;
	[Header("Scripts")]
	public ClickToRotateTile clickToRotScript;

	void Start () {
		for (int i = 0; i < leafTurnFXGOs.Count; i++)
		{
			if (leafTurnFXGOs[i] == null) {
				GameObject go = Instantiate(leafTurnFXGORef);
				leafTurnFXGOs[i] = go;
				go.SetActive(false);
				go.transform.parent = this.gameObject.transform;
			}
		}
	}
	
	public void PlayFXFromPool () {
		GameObject leafTurnFXGO = leafTurnFXGOs[currentTempTransform];
		leafTurnFXGO.SetActive(true);
		leafTurnFXGO.transform.position = clickToRotScript.tileClicked.transform.position;
		leafTurnFXGO.GetComponent<ParticleSystem>().Play(true);

		currentTempTransform++;
		if (currentTempTransform > leafTurnFXGOs.Count - 1) {
			currentTempTransform = 0;
		}
	}
}
