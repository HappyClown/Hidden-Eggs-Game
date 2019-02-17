using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelCompEggCounter : MonoBehaviour {
	public TextMeshProUGUI tmp;
	public bool startIncrease;
	public float eggAmnt/* , duration */;
	//private float lerp, startEggs, endEggs;
	//public AnimationCurve animCurve;
	//public ClickOnEggs clickOnEggsScript;

	void Start () {
		
	}
	
	void Update () {
		// if (startIncrease) {
			// lerp += Time.deltaTime / duration;
			// eggAmnt = Mathf.Floor(Mathf.Lerp(startEggs, endEggs, animCurve.Evaluate(lerp)));
			tmp.text = "x" + eggAmnt;
		// }
	}

	public void StartCounterIncrease() {
		startIncrease = true;
	}
}
