using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeOnEnable : MonoBehaviour 
{
	private bool startFadeIn;
	private float alpha;
	public float fadeSpeed;
	public Image thisImg;

	

	void Update () 
	{
		if (startFadeIn)
		{
			alpha += Time.deltaTime * fadeSpeed;
			
			if (thisImg.color.a < 1)
			{
				thisImg.color = new Color(1,1,1, alpha);
			}
			else
			{
				startFadeIn = false;
			}
		}
	}



	void OnEnable ()
	{
		startFadeIn = true;
	}



	void OnDisable ()
	{
		startFadeIn = false;
		alpha = 0f;
		thisImg.color = new Color(1,1,1, alpha);
	}
}
