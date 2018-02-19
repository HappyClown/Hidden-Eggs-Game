using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggGoToCorner : MonoBehaviour 
{
	public Transform cornerPos;

	public Vector3 cornerRot;

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

			this.transform.eulerAngles = Vector3.Lerp(this.transform.eulerAngles, cornerRot, timeToMove * Time.deltaTime);
		}
	}
	

	public void GoToCorner () 
	{
		moveThisEgg = true;
	}
}
