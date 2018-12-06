﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnaDisaButtonOnFade : MonoBehaviour {

	private bool alphaDeltaPos, alphaDeltaNeg;
	private float alphaDelta, curAlphaValue, prevAlphaValue;
	public Image refImg;
	public Button thisBtn;
	public float disableThreshold, enableThreshold;
	


	void Start () {
		
	}
	
	void Update () {
		curAlphaValue = refImg.color.a;

		if (curAlphaValue != prevAlphaValue)
		{
			//Debug.Log(this.gameObject.name);
			alphaDelta = curAlphaValue - prevAlphaValue;

			if (alphaDelta > 0)
			{
				alphaDeltaPos = true;
				alphaDeltaNeg = false;
			}
			else if (alphaDelta < 0)
			{
				alphaDeltaPos = false;
				alphaDeltaNeg = true;
			}

			if (refImg.color.a < disableThreshold && alphaDeltaNeg)
			{
				thisBtn.enabled = false;
			}

			if (refImg.color.a > enableThreshold && alphaDeltaPos)
			{
				thisBtn.enabled = true;
			}
		}

		prevAlphaValue = refImg.color.a;
	}
}
