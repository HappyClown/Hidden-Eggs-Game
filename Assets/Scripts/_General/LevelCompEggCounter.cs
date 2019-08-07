using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelCompEggCounter : MonoBehaviour {
	public TextMeshProUGUI tmp;
	public float eggAmnt;
	private float prevEggAmnt;
	
	void Update () {
		if (eggAmnt != prevEggAmnt) {
			// tmp.text = "x" + eggAmnt;
			if (eggAmnt < 10) {
				tmp.text = "0" + eggAmnt;
			}
			else {
				tmp.text = "" + eggAmnt;
			}
		}
		prevEggAmnt = eggAmnt;
	}
}
