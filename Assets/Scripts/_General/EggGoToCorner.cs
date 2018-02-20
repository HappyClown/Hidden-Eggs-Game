using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggGoToCorner : MonoBehaviour 
{
	public Vector3 cornerPos;

	public ClickOnEggs clickOnEggsScript;

	public Vector3 cornerRot;

	public Vector3 cornerEggScale;

	public float timeToMove;

	public bool moveThisEgg;


	void Start () 
	{
		
	}


	void Update ()
	{
		if (moveThisEgg == true)
		{
			this.transform.position = Vector3.Lerp(this.transform.position, cornerPos, timeToMove * Time.deltaTime);

			this.transform.eulerAngles = Vector3.Lerp(this.transform.eulerAngles, cornerRot, timeToMove * Time.deltaTime);

			this.transform.localScale = Vector3.Lerp(this.transform.localScale, cornerEggScale, timeToMove * Time.deltaTime);
		}
	}
	

	public void GoToCorner () 
	{
		moveThisEgg = true;
		cornerPos = clickOnEggsScript.newCornerPos;
	}
}
