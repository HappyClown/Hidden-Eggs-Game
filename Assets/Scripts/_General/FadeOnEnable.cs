using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeOnEnable : MonoBehaviour 
{
	private bool startFadeIn;
	private float alpha;
	public float fadeSpeed;

	

	void Update () 
	{
		if (startFadeIn)
		{
			alpha += Time.deltaTime * fadeSpeed;
			
			if (this.GetComponent<Image>().color.a < 1)
			{
				this.GetComponent<Image>().color = new Color(1,1,1, alpha);
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
		this.GetComponent<Image>().color = new Color(1,1,1, alpha);
	}
}
