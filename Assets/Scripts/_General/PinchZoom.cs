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
	private static readonly float[] BoundsX = new float[]{-10f, 5f};
	private static readonly float[] BoundsZ = new float[]{-18f, -4f};
	private static readonly float[] ZoomBounds = new float[]{10f, 85f};	

	public bool touching = false;
	public Touch touchZero;
	public Touch touchOne;
	public float minCameraSize;
	private float maxCameraSize;
	public float currentCameraSize;
	public float onTouchCameraSize;
	public float minDeltaDif;
	public float maxDeltaDif;
	private float initialDeltaDif;
	public float currentDeltaDif = 0;
	public float myLastDelta;
	public Vector2 initialCameraPosition;
	void Start(){
		maxCameraSize = cam.orthographicSize;
		currentCameraSize = cam.orthographicSize;
		initialCameraPosition = new Vector2(cam.transform.position.x, cam.transform.position.y);
	}
	void Update ()
	{
		if (Input.touchCount == 2) { 
			PinchCamZoom(); 
		}else{
			touching = false;
			currentDeltaDif = 0;
		}
		
		// if(Input.GetMouseButtonDown(0))
		// {
		// 	lastPanPosition = Input.mousePosition;
		// }
		// else if (Input.GetMouseButton(0))
		// {
		// 	PanCam(Input.mousePosition);
		// }

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
			touchZero = Input.GetTouch(0);
			touchOne = Input.GetTouch(1);
		if(!touching){
			touchZeroPos = cam.ScreenToWorldPoint(touchZero.position);
			touchOnePos = cam.ScreenToWorldPoint(touchOne.position);
			centerPoint = (touchZeroPos + touchOnePos) * 0.5f;
			initialDeltaDif = (touchZero.position - touchOne.position).magnitude;
			touching = true;
			onTouchCameraSize = cam.orthographicSize;
		}
			Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
			Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

			float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
			float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;
			
			currentDeltaDif = Mathf.Abs(touchDeltaMag - initialDeltaDif);
			
			float myDeltaDif = prevTouchDeltaMag - touchDeltaMag;

			if (cam.orthographic && currentDeltaDif > minDeltaDif)
			{
				if(prevTouchDeltaMag > touchDeltaMag){
					cam.transform.Translate((initialCameraPosition - new Vector2(cam.transform.position.x, cam.transform.position.y)).normalized * (camZoomMoveSpeed * Vector2.Distance(centerPoint,cam.transform.position)), Space.World);
				}
				else{
					cam.transform.Translate((centerPoint - new Vector2(cam.transform.position.x, cam.transform.position.y)).normalized * (camZoomMoveSpeed * Vector2.Distance(centerPoint,cam.transform.position)), Space.World);
				}
				currentCameraSize = currentCameraSize + (((myDeltaDif/maxDeltaDif))*(maxCameraSize - minCameraSize));
				cam.orthographicSize = Mathf.Clamp(currentCameraSize, minCameraSize, maxCameraSize);
				currentCameraSize = cam.orthographicSize;
			}
			myLastDelta = myDeltaDif;
			// else
			// {
			// 	cam.fieldOfView += deltaMagnitudeDiff * perspectiveZoomSpeed;
			// 	cam.fieldOfView = Mathf.Clamp(cam.fieldOfView, 0.1f, 179.9f);
			// }
		
	}
	
}
