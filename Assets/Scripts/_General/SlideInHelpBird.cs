using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideInHelpBird : MonoBehaviour 
{
	public Transform hiddenHelpBirdPos;
	public Transform shownHelpBirdPos;

	public bool moveUp = false;

	public float speed;
	

	void Update ()
	{
		if (moveUp == true)
		{
			this.transform.position = Vector3.MoveTowards(this.transform.position, shownHelpBirdPos.position, speed * Time.deltaTime);
		}

		if (moveUp == false)
		{
			this.transform.position = Vector3.MoveTowards(this.transform.position, hiddenHelpBirdPos.position, speed * Time.deltaTime);
		}

	}


	public void MoveBirdUp () 
	{
		if (moveUp == false)
		{moveUp = true;}
		else if (moveUp == true)
		{moveUp = false;}
	}
}
