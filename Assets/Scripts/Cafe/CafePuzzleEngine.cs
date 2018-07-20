using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CafePuzzleEngine : MonoBehaviour {

	// Use this for initialization
	public SwipeDetector mySwipeControls;
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(mySwipeControls.SwipeLeft)
		{
			Debug.Log("left");
		}
		if(mySwipeControls.SwipeRight)
		{
			Debug.Log("right");
		}
		if(mySwipeControls.SwipeUp)
		{
			Debug.Log("up");
		}
		if(mySwipeControls.SwipeDown)
		{
			Debug.Log("down");
		}
	}
}
