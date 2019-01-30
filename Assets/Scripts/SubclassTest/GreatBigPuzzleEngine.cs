using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreatBigPuzzleEngine : MonoBehaviour {
	public int theNumberOne = 1;
	private int thePrivateNumberTwo = 2;

	void Start () {
		Debug.Log("Holy Guacamoly Shmoly Tripotly, also this is in the start function. Doooope");
	}
	
	void Update () {
		Debug.Log("Play. Play. Then Play. Then Play Again. Forever.");
	}

	public void PlayMessage (int whatNumber) {
		Debug.Log("This plays a message not a massege. BIG difference. Also the number you have inpotatoed is: " + whatNumber);
	}
}
