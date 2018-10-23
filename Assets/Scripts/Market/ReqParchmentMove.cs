using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReqParchmentMove : MonoBehaviour 
{
	public float shownYPos, hiddenYPos, moveDuration;
	public bool moveToShown, moveToHidden;
	public AnimationCurve moveYAnimCurve;
	private float lerpValue;


	void Update()
	{
		if (moveToShown) { MoveToShown(); }

		if (moveToHidden) { MoveToHidden(); }
	}

	void MoveToShown()
	{
		if (lerpValue < 1)
		{
			lerpValue += Time.deltaTime / moveDuration;
			float myY = Mathf.Lerp(hiddenYPos, shownYPos, moveYAnimCurve.Evaluate(lerpValue));
			this.transform.position = new Vector3( this.transform.position.x, myY, this.transform.position.z);
		}
		else
		{
			moveToShown = false;
			lerpValue = 0f;
		}
	}

	void MoveToHidden()
	{
		if (lerpValue < 1)
		{
			lerpValue += Time.deltaTime / moveDuration;
			float myY = Mathf.Lerp(shownYPos, hiddenYPos, lerpValue);
			this.transform.position = new Vector3( this.transform.position.x, myY, this.transform.position.z);
		}
		else
		{
			moveToHidden = false;
			lerpValue = 0f;
		}
	}
}
