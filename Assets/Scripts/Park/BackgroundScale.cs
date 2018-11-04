using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScale : MonoBehaviour 
{
	public KitePuzzEngine kitePuEnScript;
	public bool scaleBG;
	public float lerpTimer;
	private float startScale, endScale;
	public float scaleValue;
	public float scaleDur;
	public AnimationCurve animCurve;


	void Update()
	{
		if (scaleBG) 
		{ 
			lerpTimer += Time.deltaTime / scaleDur;
			scaleValue = Mathf.Lerp(startScale, endScale, animCurve.Evaluate(lerpTimer));
			this.transform.localScale = new Vector3(scaleValue, scaleValue, scaleValue);
			if (lerpTimer >= 1)
			{
				scaleBG = false;
				this.transform.localScale = new Vector3(endScale, endScale, endScale);
				lerpTimer = 0f;
			}
		}
	}
	
	public void ScaleBG()
	{
		if (!scaleBG) { scaleBG = true; }
		lerpTimer = 0f;
		startScale = this.transform.localScale.x;
		endScale = kitePuEnScript.bGSizes[kitePuEnScript.curntLvl - 1];
	}
}
