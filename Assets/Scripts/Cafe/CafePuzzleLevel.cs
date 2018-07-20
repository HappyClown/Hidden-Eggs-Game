using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CafePuzzleLevel : MonoBehaviour {

	// Use this for initialization
	public CafePuzzleCup[] myCups;
	public int levelNumber;
	public bool starting, finishing;
	public int currentCup;
	public enum cupColor{
		Red,
		blue,
		green
	}
	public cupColor[] cupsOrder;
	public int requiredCups;
	void Start () {
		starting = finishing = false;
		currentCup = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if(myCups[0].myColor.ToString() == cupsOrder[0].ToString()){
			
		}
	}
}
