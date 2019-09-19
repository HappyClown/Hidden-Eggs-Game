using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Items : MonoBehaviour 
{
	public Sprite item;
	public Sprite itemWithShadow;
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
	public float zPos;


	void Start () 
	{
		initialPos = this.transform.position;
		zPos = initialPos.z;
		inCrate = false;
		//startTime = Time.time;
		sprite = this.gameObject.GetComponent<SpriteRenderer>();

		sprite.color = new Color(1f, 1f, 1f, 0f);

		//FadeIn ();
	}

	
	void Update () 
	{
		if (fadingOut == true)
		{
			t += Time.deltaTime / fadeDuration;
			sprite.color = new Color(1f, 1f, 1f, Mathf.SmoothStep(1f, 0f, t));
			if (sprite.color.a <= 0f) 
			{
				//this.gameObject.SetActive(false);
				fadingOut = false;
			}
		}

		if (fadingIn == true)
		{
			t += Time.deltaTime / fadeDuration;
			sprite.color = new Color(1f, 1f, 1f, Mathf.SmoothStep(0f, 1f, t));
			if (sprite.color.a >= 1f)
			{
				fadingIn = false;
			}
		}
	}


	public void FadeOut ()
	{
		sprite.color = new Color(1f, 1f, 1f, 1f);
		if (fadingOut == false/*  && sprite.color.a >= 0.01f */)
		{
			fadingOut = true;
			fadingIn = false;
			t = 0f;
			//Debug.Log("Should Fade Out");
		}
	}


	public void FadeIn ()
	{
		if (this.sprite) sprite.color = new Color(1f, 1f, 1f, 0f);
		//if (fadingIn == false/* && sprite.color.a <= 0.01f*/)
		//{
			fadingIn = true;
			fadingOut = false;
			t = 0f;
			//Debug.Log("Should Fade In");
		//}
	}


	public void BackToInitialPos ()
	{
		this.transform.position = initialPos;
		this.inCrate = false;
	}
}
