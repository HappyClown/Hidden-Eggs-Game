using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinchZoom : MonoBehaviour 
{	
	[Header("Main Camera in the level")]
	public Camera cam;
	[Header("Camera movement speed")]
	public float camZoomMoveSpeed;
	//touch position holders
	private Vector2 centerPoint, touchZeroPos, touchOnePos;
	[Header("Camera boudaries")]
	public float maxX;public float maxY;
	//touching screen detector
	private bool touching = false;
	//touch variables holders
	private Touch touchZero, touchOne;
	[Header("Camera Min Zoom Size (orthographicSize)")]
	public float minCameraSize;
	//max camera orthographicsize is taken from the camera
	private float maxCameraSize;
	//current camera size at every frame
	private float currentCameraSize;
	[Header("Touch Death zone Delta")]
	public float minDeltaDif;
	[Header("Touch max Delta")]
	public float maxDeltaDif;
	//touch delta holders
	private float initialDeltaDif, currentDeltaDif = 0;
	//initial camera position holder
	private Vector3 initialCameraPosition;
	private float currentX, currentY;
	void Start(){
		//set the max camera orthographic size from the start
		maxCameraSize = cam.orthographicSize;
		//set the current camera size as the currenc ortographic size
		currentCameraSize = cam.orthographicSize;
		//get the initial camera X and Y values
		initialCameraPosition = cam.transform.position;
	}
	void Update ()
	{
		//check if the player is doing two contacts
		if (Input.touchCount == 2) { 
			//activate the zoom function
			PinchCamZoom(); 
		}else{
			//reset values when the player is not doing two contacts
			touching = false;
			currentDeltaDif = 0;
		}		
	}

	void PinchCamZoom()
	{
		//set the touch 0 and 1 variables with the two touches from the input accordingly
			touchZero = Input.GetTouch(0);
			touchOne = Input.GetTouch(1);
		//check if the touch is new
		if(!touching){
			//set the initioal position holders as the touches converted into screen to world positions
			touchZeroPos = cam.ScreenToWorldPoint(touchZero.position);
			touchOnePos = cam.ScreenToWorldPoint(touchOne.position);
			//Set the center initial piont
			centerPoint = (touchZeroPos + touchOnePos) * 0.5f;
			if(centerPoint.x > maxX){centerPoint.x = maxX;}
			if(centerPoint.x < (maxX*-1)){centerPoint.x =(maxX*-1);}
			if(centerPoint.y > maxY){centerPoint.y = maxY;}
			if(centerPoint.y < (maxY*-1)){centerPoint.y = (maxY*-1);}
			//set the initial delta difference
			initialDeltaDif = (touchZero.position - touchOne.position).magnitude;
			//set the touch detector as true so the next frame the toucj wont be new
			touching = true;
		}
		//create previous positions using the delta position property from the touch variables so we can detect the sliding change in the finger
			Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
			Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;
		//with the previous touch position we can set a previous delta amount of movement, then the current denta with the current touches
			float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
			float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;
		//the current delta difference variable gives and absulute number of the difference within the current delta and the initial
		//is used for the touch deathzone
			currentDeltaDif = Mathf.Abs(touchDeltaMag - initialDeltaDif);
		//delta diference is set to know the chance of the delta every frame	
			float myDeltaDif = prevTouchDeltaMag - touchDeltaMag;

			
				currentX = (maxX*(currentCameraSize-maxCameraSize)*-1)/(maxCameraSize - minCameraSize);
				currentY = (maxY*(currentCameraSize-maxCameraSize)*-1)/(maxCameraSize - minCameraSize);	

		//we check if the current delta is bigger than the min delta to make a death zone
			if (cam.orthographic && currentDeltaDif > minDeltaDif)
			{
				//if the previus delta is bigger than the current delta, it means the player is closing the fingers, so we zoom out
				if(prevTouchDeltaMag > touchDeltaMag){					
					cam.transform.position = Vector3.Lerp(cam.transform.position,initialCameraPosition,Time.deltaTime*camZoomMoveSpeed);
					//cam.transform.Translate((initialCameraPosition - new Vector2(cam.transform.position.x, cam.transform.position.y)).normalized * (camZoomMoveSpeed * Vector2.Distance(centerPoint,cam.transform.position)), Space.World);
				}
				//if the previus delta is less than the current delta, it means the player is oppening the fingers, so we zoom in
				else{
					cam.transform.position = Vector3.Lerp(cam.transform.position,new Vector3(centerPoint.x,centerPoint.y,initialCameraPosition.z),Time.deltaTime*camZoomMoveSpeed);
					//cam.transform.Translate((centerPoint - new Vector2(cam.transform.position.x, cam.transform.position.y)).normalized * (camZoomMoveSpeed * Vector2.Distance(centerPoint,cam.transform.position)), Space.World);
				}
				
				if(centerPoint.x > currentX){cam.transform.position = new Vector3(currentX,cam.transform.position.y,initialCameraPosition.z);}
				if(centerPoint.x < (currentX*-1)){cam.transform.position = new Vector3((currentX*-1),cam.transform.position.y,initialCameraPosition.z);}
				if(centerPoint.y > currentY){cam.transform.position = new Vector3(cam.transform.position.x,currentY,initialCameraPosition.z);}
				if(centerPoint.y < (currentY*-1)){cam.transform.position = new Vector3(cam.transform.position.x,(currentY*-1),initialCameraPosition.z);}
				
				//the following function sets the camera size accordingly to the max delta and the max and min camera size
				//the convertion returns a value for the camera size
				currentCameraSize = currentCameraSize + (((myDeltaDif/maxDeltaDif))*(maxCameraSize - minCameraSize));
				//clamp the value in between the camera limits
				cam.orthographicSize = Mathf.Clamp(currentCameraSize, minCameraSize, maxCameraSize);
				//update the current camera size
				currentCameraSize = cam.orthographicSize;
			}
	}
	
}
