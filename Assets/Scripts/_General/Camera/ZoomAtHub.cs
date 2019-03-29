using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomAtHub : MonoBehaviour {

	private bool zoomIn;
	public float fromZoom, toZoom, speed, zoomSpeed;
	private float currentZoom;
	private Vector3 ZoomPosition;
	public Camera myCam;
	// Use this for initialization
	void Start () {
		zoomIn = false;
		myCam = this.GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void Update () {
		if(zoomIn){
			myCam.gameObject.transform.position = Vector3.Lerp(myCam.gameObject.transform.position,ZoomPosition, Time.deltaTime * speed);
			currentZoom = myCam.orthographicSize;
			myCam.orthographicSize = Mathf.Lerp(currentZoom,toZoom,Time.deltaTime * zoomSpeed);
		}
	}
	public void StartZoom(Vector3 toPosition){
		zoomIn = true;
		ZoomPosition = toPosition;
	}
}
