using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCloud : MonoBehaviour 
{
	public float moveSpeed;
	public bool doIMove;
	public bool moveLeft;

	public SpriteRenderer cloudSprite;
	public float cloudAlpha;
	public float cloudFadeSpeed;


	void Update ()
	{
		if (doIMove)
		{
			// - MOVE IN PROPER DIRECTION - //
			if (moveLeft) { this.transform.Translate(Vector3.left*moveSpeed); }
			else { this.transform.Translate(Vector3.right*moveSpeed); }

			// - FADE - //
			if (cloudAlpha > 0) { cloudAlpha -= cloudFadeSpeed; }
			cloudSprite.color = new Color(1, 1, 1, Mathf.SmoothStep(0f, 1f, cloudAlpha));
		}

	}
	
}
