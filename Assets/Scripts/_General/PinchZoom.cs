using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinchZoom : MonoBehaviour 
{
	public float perspectiveZoomSpeed = 0.5f;
	public float orthoZoomSpeed = 0.5f;
	public Camera cam;
	public float camZoomMoveSpeed;
	public Vector2 centerPoint;
	public Vector2 touchZeroPos;
	public Vector2 touchOnePos;

	public Vector3 lastPanPosition;
	public float PanSpeed;
	// private static readonly float[] BoundsX = new float[]{-10f, 5f};
	// private static readonly float[] BoundsZ = new float[]{-18f, -4f};
	// private static readonly float[] ZoomBounds = new float[]{10f, 85f};	



	void Update ()
	{
		if (Input.touchCount == 1) { touchZeroPos = cam.ScreenToWorldPoint(Input.GetTouch(0).position); }
		if (Input.touchCount == 2) { touchOnePos = cam.ScreenToWorldPoint(Input.GetTouch(1).position); }
		// if(Input.GetMouseButtonDown(0))
		// {
		// 	lastPanPosition = Input.mousePosition;
		// }
		// else if (Input.GetMouseButton(0))
		// {
		// 	PanCam(Input.mousePosition);
		// }

		PinchCamZoom();
		//PanCam();
	}


	// void PanCam(Vector3 newPanPosition)
	// {
	// 	// Determine how much to move the camera
	// 	Vector3 offset = cam.ScreenToViewportPoint(lastPanPosition - newPanPosition);
	// 	Vector3 move = new Vector3(offset.x * PanSpeed, 0, offset.y * PanSpeed);
		
	// 	// Perform the movement
	// 	cam.transform.Translate(move, Space.World);  
		
	// 	// Ensure the camera remains within bounds.
	// 	Vector3 pos = transform.position;
	// 	// pos.x = Mathf.Clamp(transform.position.x, BoundsX[0], BoundsX[1]);
	// 	// pos.z = Mathf.Clamp(transform.position.z, BoundsZ[0], BoundsZ[1]);
	// 	cam.transform.position = pos;

	// 	// Cache the position
	// 	lastPanPosition = newPanPosition;
	// }



	void PinchCamZoom()
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

			centerPoint = (touchZeroPos + touchOnePos) * 0.5f;

			if (cam.orthographic)
			{
				cam.transform.Translate((centerPoint - new Vector2(cam.transform.position.x, cam.transform.position.y)).normalized * camZoomMoveSpeed, Space.World);
				cam.orthographicSize += deltaMagnitudeDiff * orthoZoomSpeed;
				cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, 1f, 10.789f);
			}
			// else
			// {
			// 	cam.fieldOfView += deltaMagnitudeDiff * perspectiveZoomSpeed;
			// 	cam.fieldOfView = Mathf.Clamp(cam.fieldOfView, 0.1f, 179.9f);
			// }
		}
	}
	
}
