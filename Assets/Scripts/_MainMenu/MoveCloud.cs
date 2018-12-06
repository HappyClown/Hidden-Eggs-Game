using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCloud : MonoBehaviour 
{
	public float moveSpeed, minSpeed, maxSpeed;
	public bool moveOut;
	public bool moveIn;
	public bool moveLeft;

	public SpriteRenderer cloudSprite;
	public float cloudAlpha;
	public float cloudFadeSpeed;


	void Start ()
	{
		moveSpeed = Random.Range(minSpeed, maxSpeed);
	}


	void Update ()
	{
		if (moveOut)
		{
			// - MOVE IN PROPER DIRECTION - //
			if (moveLeft) { this.transform.Translate(Vector3.left*moveSpeed*Time.deltaTime); }
			else { this.transform.Translate(Vector3.right*moveSpeed*Time.deltaTime); }

			// - FADE - //
			if (cloudAlpha > 0) { cloudAlpha -= cloudFadeSpeed*Time.deltaTime; }
			cloudSprite.color = new Color(1, 1, 1, Mathf.SmoothStep(0f, 1f, cloudAlpha));

			// - STOP MOVING IF FADED OUT - //
			if (cloudAlpha <= 0) { moveOut = false; }
		}

		if (moveIn)
		{
			// - MOVE IN PROPER DIRECTION - // (Reverse of moveOut directions)
			if (moveLeft) { this.transform.Translate(Vector3.right*moveSpeed*Time.deltaTime); }
			else { this.transform.Translate(Vector3.left*moveSpeed*Time.deltaTime); }

			// - FADE IN - //
			if (cloudAlpha < 1) { cloudAlpha += cloudFadeSpeed*Time.deltaTime; }
			cloudSprite.color = new Color(1, 1, 1, Mathf.SmoothStep(0f, 1f, cloudAlpha));

			// - STOP MOVING IF FADED IN - //
			if (cloudAlpha >= 1) { moveIn = false; }
		}

	}

	public void MoveIn()
	{
		moveOut = false;
		moveIn = true;
	}

	public void MoveOut()
	{
		moveOut = true;
		moveIn = false;
	}
	
}
