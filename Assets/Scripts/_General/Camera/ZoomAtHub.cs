using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomAtHub : MonoBehaviour {

	private bool zoomIn;
	public float fromZoom, toZoom, speed, zoomSpeed;
	private float currentZoom, lerpValue, camLerpValue;
	private Vector3 zoomPosition, startPosition;
	public Camera myCam;

	//TEST SOUNDS
	public AudioManagerHubMenu audiomanmenuScript;

	// Use this for initialization
	void Start () {
		zoomIn = false;
		myCam = this.GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void Update () {
		if(zoomIn){
			lerpValue += Time.deltaTime * speed;
			myCam.gameObject.transform.position = Vector3.Lerp(startPosition, zoomPosition, lerpValue);
			camLerpValue += Time.deltaTime * zoomSpeed;
			myCam.orthographicSize = Mathf.Lerp(currentZoom, toZoom, camLerpValue);
		}
	}
	public void StartZoom(Vector3 toPosition){
		zoomIn = true;
		zoomPosition = toPosition;
		startPosition = myCam.transform.position;
		currentZoom = myCam.orthographicSize;

		//sound
		audiomanmenuScript.ZoomSound();
	}
}
