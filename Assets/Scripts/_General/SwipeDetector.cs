using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeDetector : MonoBehaviour {

	private bool tap, swipeLeft, swipeRight, swipeUp, swipeDown; 
	private bool isDraging = false;
	public float deathzoneRadius;
	private Vector2 startTouch, swipeDelta, firstTouch;
	public Vector2 SwipeDelta{ get {return swipeDelta;}}
	public bool SwipeLeft{get {return swipeLeft;}}
	public bool SwipeRight{get {return swipeRight;}}
	public bool SwipeUp{get {return swipeUp;}}
	public bool SwipeDown{get {return swipeDown;}}
	public bool IsDraging{get {return isDraging;}}
	public Vector2 FirstTouch{get {return firstTouch;}}
	
	// Update is called once per frame
	void Update () {
		tap = swipeLeft = swipeRight = swipeUp = swipeDown = false;
		#region StandAlone
		if(Input.GetMouseButtonDown(0)){
			tap = true;
			isDraging = true;
			startTouch = Input.mousePosition;
		}
		else if(Input.GetMouseButtonUp(0)){
			isDraging = false;
			Reset();
		}
		#endregion
		
		#region Mobile Inputs
		if(Input.touches.Length > 0 ){
			if(Input.touches[0].phase == TouchPhase.Began){
				tap = true;
				isDraging = true;
				startTouch = Input.touches[0].position;
			}
			else if(Input.touches[0].phase == TouchPhase.Ended || Input.touches[0].phase == TouchPhase.Canceled){
				isDraging = false;
				Reset();
			}
		}
		#endregion

		//calculate the distance
		swipeDelta = Vector2.zero;
		if(isDraging){
			if(Input.touches.Length > 0){
				swipeDelta = Input.touches[0].position - startTouch;
			}else if(Input.GetMouseButton(0)){
				swipeDelta = (Vector2)Input.mousePosition - startTouch;
			}
			if(swipeDelta.magnitude > deathzoneRadius){
				firstTouch = startTouch;
				//which direction?
				float x = swipeDelta.x;
				float y = swipeDelta.y;
				if(Mathf.Abs(x) > Mathf.Abs(y)){
					//left or right
					if(x < 0){
						swipeLeft = true;
					}
					else{
						swipeRight = true;
					}
				}
				else{
					//up or down
					if(y < 0){
						swipeDown = true;
					}
					else{
						swipeUp = true;
					}
				}
				Reset();
			}
		}
		//Did we cross the deathzone?
		
	}
	private void Reset(){
		startTouch = swipeDelta = Vector2.zero;
		isDraging = false;
	}
}
