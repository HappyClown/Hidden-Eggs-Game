using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveWithCamera : MonoBehaviour 
{
	public Camera cam;
	public Vector3 topLeftCorner;
	public float newScale;

	public float originalObjScale;
	public float originalCamSize;


	void Start ()
	{
		originalCamSize = cam.orthographicSize;
		originalObjScale = this.transform.localScale.x;
	}


	void LateUpdate ()
	{ 
		topLeftCorner = cam.ScreenToWorldPoint(new Vector3(0, cam.pixelHeight, 0));
		if (topLeftCorner.y > 10.8037f) {
			topLeftCorner.y = 10.8037f;
		}
		this.transform.position = new Vector2(topLeftCorner.x, topLeftCorner.y);

		newScale = (originalObjScale * cam.orthographicSize) / originalCamSize;
		this.transform.localScale = new Vector3(newScale, newScale, this.transform.localScale.z);
	}
}