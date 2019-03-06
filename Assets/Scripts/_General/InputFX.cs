using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputFX : MonoBehaviour {
	public ParticleSystem touchFX;
	public inputDetector inputDetScript;
	
	void Update () {
		if (inputDetScript.Tapped) {
			Vector3 mousePos = Camera.main.ScreenToWorldPoint(inputDetScript.TapPosition);
			touchFX.transform.position = new Vector3(mousePos.x, mousePos.y, -1);
			touchFX.Play();
		}
	}
}
