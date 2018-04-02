using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlideInHelpBird : MonoBehaviour 
{
	public Transform hiddenHelpBirdPos;
	public Transform shownHelpBirdPos;

	public GameObject shadow;
	public float shadowAlpha;
	public GameObject txtBubble;

	public bool moveUp = false;

	public float speed;

	private float totalDist;
	private float distLeft;
	private float distPercent;
	


	void Start ()
	{
		totalDist = Vector2.Distance(hiddenHelpBirdPos.position, shownHelpBirdPos.position);
	}



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


		distLeft = Vector2.Distance(this.transform.position, shownHelpBirdPos.position);
		distPercent = (totalDist - distLeft) / totalDist;

		if (moveUp == true/* && Vector2.Distance(this.transform.position, shownHelpBirdPos.position) <= 0.1f*/)
		{
			if (shadowAlpha < 1) { shadowAlpha = distPercent; }
			shadow.GetComponent<Image>().color = new Color(1,1,1, shadowAlpha);
		}

		if (moveUp == false)
		{
			if (shadowAlpha > 0) { shadowAlpha = distPercent; }
			shadow.GetComponent<Image>().color = new Color(1,1,1, shadowAlpha);
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
