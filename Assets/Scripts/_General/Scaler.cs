using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scaler : MonoBehaviour 
{
	public Vector3 iniScale, minScale, maxScale;
	public float lerpTimer, scaleDuration, scaleDelay;
	public bool scaleUp, scaleDown;
	public AnimationCurve animCurve;
	

	void Awake () 
	{
		iniScale = this.transform.localScale;
	}
	

	void Update () 
	{
		if (scaleUp)
		{
			lerpTimer += Time.deltaTime / scaleDuration;
			this.transform.localScale = Vector3.Lerp(iniScale, maxScale, animCurve.Evaluate(lerpTimer));
			if (lerpTimer >= 1f)
			{
				scaleUp = false;
			}
		}

		if (scaleDown)
		{
			lerpTimer += Time.deltaTime / scaleDuration;
			this.transform.localScale = Vector3.Lerp(iniScale, minScale, animCurve.Evaluate(lerpTimer));
			if (lerpTimer >= 1f)
			{
				scaleDown = false;
			}
		}
	}

	public void ScaleUp()
	{
		scaleUp = true;
		scaleDown = false;
		iniScale = this.transform.localScale;
		lerpTimer = 0f - scaleDelay;
	}

	public void ScaleDown()
	{
		scaleUp = false;
		scaleDown = true;
		iniScale = this.transform.localScale;
		lerpTimer = 0f - scaleDelay;
	}
}
