using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinchZoom : MonoBehaviour 
{
	public float perspectiveZoomSpeed = 0.5f;
	public float orthoZoomSpeed = 0.5f;
	public Camera cam;

	void Update ()
	{
		if (Input.touchCount == 2)
		{
			Touch touchZero = Input.GetTouch(0);
			Touch touchOne = Input.GetTouch(1);

			Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
			Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

			float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
			float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

			float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

			if (cam.orthographic)
			{
				cam.orthographicSize += deltaMagnitudeDiff * orthoZoomSpeed;
				cam.orthographicSize = Mathf.Max(cam.orthographicSize, 1f);
			}
			else
			{
				cam.fieldOfView += deltaMagnitudeDiff * perspectiveZoomSpeed;
				cam.fieldOfView = Mathf.Clamp(cam.fieldOfView, 0.1f, 179.9f);
			}
		}
	}
	
}
