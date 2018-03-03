using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Items : MonoBehaviour 
{
	public int weight;
	public Vector3 initialPos;
	public bool inCrate;
	//public float fadeSpeed;
	public bool fadingOut;
	public bool fadingIn;
	public float t;
	//private float startTime;
	public float fadeDuration;
	private SpriteRenderer sprite;

	void Start () 
	{
		initialPos = this.transform.position;
		inCrate = false;
		//startTime = Time.time;
		sprite = this.gameObject.GetComponent<SpriteRenderer>();

		sprite.color = new Color(1f, 1f, 1f, 0f);

		FadeIn ();
	}
	
	void Update () 
	{
		// if (Input.GetMouseButtonDown(0))
		// {
		// 	FadeOut();
		// }

		// if (Input.GetMouseButtonDown(1))
		// {
		// 	FadeIn();
		// }

		if (fadingOut == true)
		{
			t += Time.deltaTime / fadeDuration;
			sprite.color = new Color(1f, 1f, 1f, Mathf.SmoothStep(1f, 0f, t));
			if (t >= 1f)
			{
				this.gameObject.SetActive(false);
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



	public void BackToInitialPos ()
	{
		this.transform.position = initialPos;
		this.inCrate = false;
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
			Debug.Log("Should Fade In");
		}
	}
}
