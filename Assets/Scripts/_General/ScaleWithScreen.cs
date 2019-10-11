using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleWithScreen : MonoBehaviour {
	public float myIniScaleX, myNewScaleX;
	public float iniCamSize, curCamSize, newCamSize;
	public Camera cam;

	public Vector3 myPos, newPos;
	public Vector3 camPos, curCamPos, newCamPos;
	
	void Start () {
		myIniScaleX = this.transform.localScale.x;
		iniCamSize = cam.orthographicSize;
	}
	
	void Update () {
		if (!cam) {
			cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
			if (iniCamSize <= 0f) {
				iniCamSize = cam.orthographicSize;
			}
		}
		newCamSize = cam.orthographicSize;
		if (curCamSize != newCamSize) {
			myNewScaleX = (newCamSize / iniCamSize) * myIniScaleX;
			this.transform.localScale = new Vector3(myNewScaleX, myNewScaleX, myNewScaleX);
		}
		curCamSize = cam.orthographicSize;
	}
	void LateUpdate () {
		newCamPos = cam.gameObject.transform.position;
		if (curCamPos != newCamPos) {
			this.transform.position = new Vector3(cam.gameObject.transform.position.x, cam.gameObject.transform.position.y, this.transform.position.z);
		}
		curCamPos = cam.gameObject.transform.position;
	}
}
