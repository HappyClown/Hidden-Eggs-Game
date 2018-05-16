using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClamFadeInOut : MonoBehaviour 
{
	public bool fadingOut;
	public bool fadingIn;
	public float t;
	public float fadeDuration;

	public bool fadeOnStart = true;
	private SpriteRenderer sprite;



	void Start ()
	{
		sprite = this.gameObject.GetComponent<SpriteRenderer>();

		sprite.color = new Color(1f, 1f, 1f, 0f);

		if (fadeOnStart) { FadeIn(); }
	}



	void Update () 
	{
		if (fadingOut == true)
		{
			t += Time.deltaTime / fadeDuration;
			sprite.color = new Color(1f, 1f, 1f, Mathf.SmoothStep(1f, 0f, t));
			if (t >= 1f)
			{
				//this.gameObject.SetActive(false);
				fadingOut = false;
			}
		}

		if (fadingIn == true)
		{
			t += Time.deltaTime / fadeDuration;
			sprite.color = new Color(1f, 1f, 1f, Mathf.SmoothStep(0f, 1f, t));
			if (t >= 1f)
			{
				fadingIn = false;
			}
		}
	}



	public void FadeOut ()
	{
		if (fadingOut == false && sprite.color.a >= 0.01f)
		{
			fadingOut = true;
			t = 0f;
			//Debug.Log("Should Fade Out");
		}
	}



	public void FadeIn ()
	{
		if (fadingIn == false/* && sprite.color.a <= 0.01f*/)
		{
			fadingIn = true;
			t = 0f;
			//Debug.Log("Should Fade In");
		}
	}
}