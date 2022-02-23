using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelCompEggCounter : MonoBehaviour {
	public TextMeshProUGUI tmp;
	public float eggAmnt;
	private float prevEggAmnt;
	
	public void AddEgg() {
		print ("eggamount plus wanonejuan");
		eggAmnt++;
		if (eggAmnt < 10) {
			tmp.text = "0" + eggAmnt;
		}
		else {
			tmp.text = "" + eggAmnt;
		}
	}
}
