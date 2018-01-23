using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggGoToCorner : MonoBehaviour 
{
	public Transform cornerPos;

	public float timeToMove;

	public bool moveThisEgg;


	void Start () 
	{
		
	}


	void Update ()
	{
		if (moveThisEgg == true)
		{
			this.transform.position = Vector3.Lerp(this.transform.position, cornerPos.position, timeToMove * Time.deltaTime);
		}
	}
	

	public void GoToCorner () 
	{
		moveThisEgg = true;
	}
}
