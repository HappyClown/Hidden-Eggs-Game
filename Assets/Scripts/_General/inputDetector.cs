using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class inputDetector : MonoBehaviour {
	public bool isPhoneDevice;
	public bool cancelDoubleTap;
	public bool twoFingerDrag;
	#region Tap Variables
	[Header("Tap Detection")]
	[Tooltip("Check if you want to detect Tap")]
	public bool detectTap, singleTap;
	private Vector2 tapPosition;
	private bool tapped;
	public Vector3 TapPosition{get{return tapPosition;}}
	public bool Tapped{get{return tapped;}}
	#endregion

	#region Double Tap Variables
	[Header("Double Tap Detection")]
	[Tooltip("Check if you want to detect Double Tap")]
	public bool detectDoubleTap;
	[Tooltip("Insert the desired max tap delay time for produce a double tap")]
	public float minDoubleTapTime;
	private float doubleTapTimer, doubleTapCounter = 0f;
	private bool doubleTapped = false, checkDoubleTap = false;
	public bool DoubleTapped{get{return doubleTapped;}}
	#endregion

	#region Drag Variables
	[Header("Drag Detection")]
	[Tooltip("Check if you want to detect Drag input")]
	public bool detectDrag;
	[Tooltip("Dragging deathzone Radius")]
	public float draggingDeathzone;
	public Vector2 startDragTouch, draggingPosition, prevDragPosition, releaseDragPos;
	public bool isDragging = false, dragStarted = false, dragReleased = false;
	public Vector2 DraggingPosition{get{return draggingPosition;}}
	#endregion

	#region DoubleTouch
	[Header("Double Touch")]
	[Tooltip("Check if you want to detect Double touch (mobile only)")]
	public bool detectDoubleTouch;	
	public bool doubleTouched = false;
	public bool DoubleTouched{get{return doubleTouched;}}
	public Vector2 touchOne, touchTwo;
	#endregion

	#region Swipe Variables
	//Swipe Detection
	[Header("Swipe Detection")]
	[Tooltip("Check if you want to detect Swipe")]
	public bool detectSwipe;
	private bool swipeTap, swipeLeft, swipeRight, swipeUp, swipeDown; 
	private bool isSwiping = false;
	[Tooltip("Swipe death zone detection")]
	[Range(50,200)]
	public float swipeDeathzoneRadius = 100f;
	public Vector2 startSwipeTouch, swipeDelta, firstSwipeTouch;
	public Vector2 SwipeDelta{ get {return swipeDelta;}}
	public bool SwipeLeft{get {return swipeLeft;}}
	public bool SwipeRight{get {return swipeRight;}}
	public bool SwipeUp{get {return swipeUp;}}
	public bool SwipeDown{get {return swipeDown;}}
	public bool IsSwiping{get {return isSwiping;}}
	public Vector2 FirstSwipeTouch{get {return firstSwipeTouch;}}
	#endregion
	void Awake(){
		isPhoneDevice = false;
		if (SystemInfo.deviceType == DeviceType.Handheld){
            isPhoneDevice = true;
        }

	}
	void Start(){
		if(detectDoubleTap)
		detectTap=true;
		tapped = false;
		cancelDoubleTap = false;
		
	}
	// Update is called once per frame
	void Update () {
		#region TapCode without double tap
		if(detectTap){
			tapped = false;
			if(Input.GetMouseButtonUp(0) && !isPhoneDevice && !isDragging){
				singleTap = true;
				tapPosition = Input.mousePosition;
				tapped = true;
			}
			if(Input.touchCount == 1 && isPhoneDevice && !isDragging){
				singleTap = true;
				if(Input.touches[0].phase == TouchPhase.Ended || Input.touches[0].phase == TouchPhase.Canceled && !isDragging){					
					tapPosition = Input.touches[0].position;
					tapped = true;
				}
			}
		}
		#endregion

		#region Double Tap Code
		if(detectDoubleTap){
			if(checkDoubleTap)
			{
				if(!cancelDoubleTap){
					doubleTapped = true;
					checkDoubleTap = false;
				}
				else{
					checkDoubleTap = false;
					cancelDoubleTap = false;
				}
			}
			else{
				doubleTapped = false;
			}
			if(Input.GetMouseButtonDown(0) && !isPhoneDevice){
				singleTap = true;
				if(doubleTapCounter == 0){					
					tapPosition = Input.mousePosition;
				}
				doubleTapCounter ++;
				if(doubleTapCounter == 2 ){
					if(doubleTapTimer <= minDoubleTapTime){
						checkDoubleTap = true;
						tapped = false;
						ResetDoubleTap();
					}else{
						ResetDoubleTap();
					}
				}
			}
			
			if(Input.touchCount == 1 && isPhoneDevice){
				singleTap = true;
				if(Input.touches[0].phase == TouchPhase.Began){	
					if(doubleTapCounter == 0){					
						tapPosition = Input.touches[0].position;
					}				
					doubleTapCounter ++;
					if(doubleTapCounter == 2 ){
						if(doubleTapTimer <= minDoubleTapTime){
							checkDoubleTap = true;
							tapped = false;
							ResetDoubleTap();
						}else{
							ResetDoubleTap();
						}
					}
				}
			}
			if(doubleTapCounter == 1)
			{
				if(doubleTapTimer > minDoubleTapTime || cancelDoubleTap){
					ResetDoubleTap();
					cancelDoubleTap = false;
				}else{
					doubleTapTimer += Time.deltaTime;
				}
			}
		}
		#endregion

		#region Dragging Code
		if(detectDrag){
			dragReleased = false;
			if(Input.GetMouseButton(0) && !isPhoneDevice){
				singleTap = true;
				if(Input.GetMouseButtonDown(0)){
					startDragTouch = Input.mousePosition;
					dragStarted = true;
				}
				else{
					dragStarted = false;
				}
				if(isDragging){
					prevDragPosition = draggingPosition;
				}
				draggingPosition = Input.mousePosition;
				if(Vector2.Distance(draggingPosition,startDragTouch) > draggingDeathzone && !isDragging){
					isDragging = true;
					prevDragPosition = draggingPosition;
				}
			}
			if(Input.GetMouseButtonUp(0) && !isPhoneDevice){
				if(isDragging || dragStarted){
					ResetDragging();
				}
			}

			if(Input.touchCount == 1 && isPhoneDevice){
				singleTap = true;
				if(Input.touches[0].phase == TouchPhase.Began){
					startDragTouch = Input.touches[0].position;					
					dragStarted = true;
				}
				else{
					dragStarted = false;
				}
				if(isDragging){
					prevDragPosition = draggingPosition;
				}
				draggingPosition = Input.touches[0].position;
				if(Vector2.Distance(draggingPosition,startDragTouch) > draggingDeathzone && !isDragging){
					isDragging = true;
					prevDragPosition = startDragTouch;
				}	 
				
				if(Input.touches[0].phase == TouchPhase.Ended || Input.touches[0].phase == TouchPhase.Canceled){
					if(isDragging || dragStarted){
						ResetDragging();
					}
				}
			}
			else if (!twoFingerDrag && Input.touchCount > 1) {
				ResetDragging();
			}
		}
		#endregion
		#region DetectDouble Touch
		if(detectDoubleTouch){
			doubleTouched = false;
			if(Input.touchCount == 2){
				doubleTouched = true;
				singleTap = false;
				touchOne = Input.touches[0].position;
				touchTwo = Input.touches[1].position;
			}
		}
		#endregion
		//if(tapped)Debug.Log("tapped");
		//if(doubleTapped)Debug.Log("D0ubleTapped");
		//if(isDragging)Debug.Log("Dragging");

		#region SwipeCode
		if(detectSwipe){
			swipeTap = swipeLeft = swipeRight = swipeUp = swipeDown = false;
			#region StandAlone
			if(Input.GetMouseButtonDown(0)){
				swipeTap = true;
				isSwiping = true;
				startSwipeTouch = Input.mousePosition;
			}
			else if(Input.GetMouseButtonUp(0)){
				isSwiping = false;
				ResetSwipe();
			}
			#endregion
			
			#region Mobile Inputs
			if(Input.touchCount == 1 ){
				if(Input.touches[0].phase == TouchPhase.Began){
					swipeTap = true;
					isSwiping = true;
					startSwipeTouch = Input.touches[0].position;
				}
				else if(Input.touches[0].phase == TouchPhase.Ended || Input.touches[0].phase == TouchPhase.Canceled){
					isSwiping = false;
					ResetSwipe();
				}
			}
			#endregion

			//calculate the distance
			swipeDelta = Vector2.zero;
			if(isSwiping){
				if(Input.touchCount == 1){
					swipeDelta = Input.touches[0].position - startSwipeTouch;
				}else if(Input.GetMouseButton(0)){
					swipeDelta = (Vector2)Input.mousePosition - startSwipeTouch;
				}
				//Did we cross the deathzone?
				if(swipeDelta.magnitude > swipeDeathzoneRadius){
					firstSwipeTouch = startSwipeTouch;
					//which direction?
					float x = swipeDelta.x;
					float y = swipeDelta.y;
					if(Mathf.Abs(x) > Mathf.Abs(y)){
						//left or right
						if(x < 0){swipeLeft = true;}
						else{swipeRight = true;}
					}
					else{
						//up or down
						if(y < 0){swipeDown = true;	}
						else{swipeUp = true;}
					}
					ResetSwipe();
				}
			}
		}
		#endregion
	}

	#region DoubleTap Functions
	public void ResetDoubleTap(){
		doubleTapCounter = doubleTapTimer = 0;
	}
	#endregion

	#region Dragging Functions
	private void ResetDragging(){
		isDragging = false;
		dragStarted = false;
		dragReleased = true;
		releaseDragPos = draggingPosition;
		draggingPosition = startDragTouch = Vector2.zero;
	}
	#endregion

	#region Swipe Functions
	private void ResetSwipe(){
		startSwipeTouch = swipeDelta = Vector2.zero;
		isSwiping = false;
	}
	#endregion

}
