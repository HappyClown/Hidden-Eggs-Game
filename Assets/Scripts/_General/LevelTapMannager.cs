using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTapMannager : MonoBehaviour {

	[Header("Main Camera in the level")]
	public Camera cam;
	//Tap and panning variables
	//Click on egg script reference for the single tap
	public ClickOnEggs myClickOnEggs;
	//Death zone for panning move and min time for double tap and the speed
	[Header("Deathzone inputDetec drag section")]
	public float panningSpeed;
	public float dTapMoveSpeed, dTapZoomSpeed;
	//Panning Move bool nad doubleTapped
	private bool  doubleTapped, zoomIn;
	//movement vectors
	private Vector2 lastPosition;
	//Zoom Variables
	[Header("Camera movement speed (usually is 2)")]
	public float camMovSpeed;
	[Header("Camera zoom speed (usually is 50)")]
	public float camZoomSpeed;
	public float camZoomDuration;
	private float camZoomSpeedModifier;
	public AnimationCurve camZoomCurve;
	private float lerpValue;
	//touch position holders
	private Vector2 centerPoint, touchZeroPos, touchOnePos;
	[Header("Camera boudaries (usually 5.9 - 3.3)")]
	public float maxX;public float maxY;
	//touching screen detectors
	private bool touching = false, zooming = false, fromZoom = false;
	//touch variables holders
	private Touch touchZero, touchOne;
	[Header("Camera Min Ortho Size (usually 7.5)")]
	public float minCameraSize;
	//max camera orthographicsize is taken from the camera
	public float maxCameraSize;
	//current camera size at every frame
	private float currentCameraSize, cameraSizeOnClick;
	[Header("Touch Death zone Delta (usually 30)")]
	public float minDeltaDif;
	[Header("Touch max Delta(usually 400)")]
	public float maxDeltaDif;
	//touch delta holders
	private float initialDeltaDif, currentDeltaDif = 0;
	//initial camera position holder
	private Vector3 initialCameraPosition;
	private Vector3 varCamPos;
	private float currentX, currentY;
	[HideInInspector]
	public RaycastHit2D hit;

	//Input Detector
	public inputDetector myInput;
	public AdjustCamSize adjustCamSizeScript;

	void Start(){
		//set the max camera orthographic size from the start
		maxCameraSize = cam.orthographicSize;
		//set the current camera size as the currenc ortographic size
		currentCameraSize = cam.orthographicSize;
		//get the initial camera X and Y values
		initialCameraPosition = cam.transform.position;

		//New - Adjust the MaxX based on the screen size/ratio
		maxX = adjustCamSizeScript.adjustedMaxX;
	}
	void Update ()
	{
		if(doubleTapped){
			DoubleTap();
		}
		else if (myInput.singleTap && !zooming && !fromZoom){
			if(myInput.DoubleTapped){
				touchZero.position = myInput.TapPosition;
				DoubleTap();
			}
			if(myInput.isDragging){
				PanningCamera();
			}
		}
		//check if the player is doing two contacts
		else if (Input.touchCount == 2 /*&& myInput.isPhoneDevice*/) { 
			//activate the zoom function
			PinchCamZoom(); 
			fromZoom = true;
		}else{
			if(Input.touchCount == 1){
				fromZoom = false;
				myInput.startDragTouch = Input.touches[0].position;
			}
			//reset values when the player is not doing two contacts
			zooming = false;
			currentDeltaDif = 0;
		}
		
		FixCameraPosition();		
	}

	void PinchCamZoom()
	{
		//set the touch 0 and 1 variables with the two touches from the input accordingly
			touchZero = Input.GetTouch(0);
			touchOne = Input.GetTouch(1);
		//check if the touch is new
		if(!zooming){
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
			zooming = true;
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

			

		//we check if the current delta is bigger than the min delta to make a death zone
			if (cam.orthographic && currentDeltaDif > minDeltaDif)
			{
				//if the previus delta is bigger than the current delta, it means the player is closing the fingers, so we zoom out
				//if(prevTouchDeltaMag > touchDeltaMag){					
					//cam.transform.position = Vector3.Lerp(cam.transform.position,initialCameraPosition, Time.deltaTime * camMovSpeed);
					//cam.transform.Translate((initialCameraPosition - new Vector2(cam.transform.position.x, cam.transform.position.y)).normalized * (camMovMoveSpeed * Vector2.Distance(centerPoint,cam.transform.position)), Space.World);
				//}
				//if the previus delta is less than the current delta, it means the player is oppening the fingers, so we zoom in
				//else{
					cam.transform.position = Vector3.Lerp(cam.transform.position,new Vector3(centerPoint.x,centerPoint.y,initialCameraPosition.z), Time.deltaTime * camMovSpeed);
					//cam.transform.Translate((centerPoint - new Vector2(cam.transform.position.x, cam.transform.position.y)).normalized * (camMovMoveSpeed * Vector2.Distance(centerPoint,cam.transform.position)), Space.World);
				//}
				//the following function sets the camera size accordingly to the max delta and the max and min camera size
				//the convertion returns a value for the camera size
				float newCameraSize = currentCameraSize + (((myDeltaDif/maxDeltaDif))*(maxCameraSize - minCameraSize));
				currentCameraSize = Mathf.Lerp(currentCameraSize,newCameraSize,Time.deltaTime * camZoomSpeed);
				//clamp the value in between the camera limits
				cam.orthographicSize = Mathf.Clamp(currentCameraSize, minCameraSize, maxCameraSize);
				//update the current camera size
				currentCameraSize = cam.orthographicSize;				
			}
	}
	void DoubleTap(){
		//Debug.Log("doube tap!!!");
		if(!doubleTapped){
			doubleTapped = true;
			centerPoint =  cam.ScreenToWorldPoint(touchZero.position);
			cameraSizeOnClick = cam.orthographicSize; 
			if(cam.orthographicSize < maxCameraSize){
				zoomIn = false;
				camZoomSpeedModifier =  (1 - ((currentCameraSize - minCameraSize) / (maxCameraSize - minCameraSize))) * camZoomDuration;
				varCamPos = cam.transform.position;
			}
			else{
				zoomIn = true;
				varCamPos = cam.ScreenToWorldPoint(touchZero.position);
			}
		}
		if(zoomIn){
			lerpValue += Time.deltaTime / camZoomDuration;
			cam.transform.position = Vector3.Lerp(initialCameraPosition,new Vector3(varCamPos.x,varCamPos.y,initialCameraPosition.z), camZoomCurve.Evaluate(lerpValue));
			currentCameraSize = Mathf.Lerp(cameraSizeOnClick,minCameraSize,camZoomCurve.Evaluate(lerpValue));
			if(currentCameraSize <= minCameraSize){
				doubleTapped = false;
				currentCameraSize = cameraSizeOnClick = minCameraSize;
				lerpValue = 0f;
			}
		}
		else{
			lerpValue += Time.deltaTime / camZoomSpeedModifier;
			cam.transform.position = Vector3.Lerp(varCamPos,initialCameraPosition, camZoomCurve.Evaluate(lerpValue));
			currentCameraSize = Mathf.Lerp(cameraSizeOnClick,maxCameraSize,camZoomCurve.Evaluate(lerpValue));
			if(currentCameraSize >= maxCameraSize){
				doubleTapped = false;
				currentCameraSize = cameraSizeOnClick = maxCameraSize;
				lerpValue = 0f;
			}
		}
		cam.orthographicSize = Mathf.Clamp(currentCameraSize, minCameraSize, maxCameraSize);

	}
	public void ZoomOutCameraReset(){
		if(cam.orthographicSize < maxCameraSize){
			 zoomIn = false;
			 DoubleTap();
		}
	}
	void PanningCamera(){
		if(cam.orthographicSize < maxCameraSize){
		float slideMagnitude = (myInput.draggingPosition - myInput.prevDragPosition).magnitude;
		Vector2 dir = (myInput.draggingPosition - myInput.prevDragPosition).normalized * -1;
		Vector3 newPos = cam.transform.position + new Vector3(dir.x,dir.y,0)*(slideMagnitude*0.01f);
		cam.transform.position = Vector3.Lerp(cam.transform.position,newPos,Time.deltaTime*panningSpeed);
		//cam.transform.position += new Vector3(dir.x,dir.y,0)*Time.deltaTime*panningSpeed;
		}
	}
	void FixCameraPosition(){
		currentX = (maxX*(currentCameraSize-maxCameraSize)*-1)/(maxCameraSize - minCameraSize);
		currentY = (maxY*(currentCameraSize-maxCameraSize)*-1)/(maxCameraSize - minCameraSize);	
				
		if(cam.transform.position.x > currentX){cam.transform.position = new Vector3(currentX,cam.transform.position.y,initialCameraPosition.z);}
		if(cam.transform.position.x < (currentX*-1)){cam.transform.position = new Vector3((currentX*-1),cam.transform.position.y,initialCameraPosition.z);}
		if(cam.transform.position.y > currentY){cam.transform.position = new Vector3(cam.transform.position.x,currentY,initialCameraPosition.z);}
		if(cam.transform.position.y < (currentY*-1)){cam.transform.position = new Vector3(cam.transform.position.x,(currentY*-1),initialCameraPosition.z);}
	}
}
