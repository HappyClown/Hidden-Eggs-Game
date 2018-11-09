
using UnityEngine;
using System.Collections;

public class AdjustCamSize : MonoBehaviour 
{
	//public bool maintainWidth=true;
	//[Range(-1,1)]
	//public int adaptPosition;

	//public float defaultWidth;
	public float defaultHeight;
	public float camAspect;

	public float screenWidth;
	public float screenHeight;
	// public float screenAspect;
	public bool adjustCamSizeToWidth;

	public float desiredAspectRatio;

	public Vector3 screenToWorldSideScreen;
	public float adjustedMaxX;
	public float minCamSize;

	public LevelTapMannager lvlTapManScript;

	public Camera cam;
	//public Vector3 CameraPos;


	void Awake () 
	{
		if (!cam) { cam = this.GetComponent<Camera>(); }
		//CameraPos = Camera.main.transform.position;

		defaultHeight = Camera.main.orthographicSize;
		//defaultWidth = Camera.main.orthographicSize * Camera.main.aspect;

		camAspect = Camera.main.aspect;

		screenWidth = Screen.width;
		screenHeight = Screen.height;

		//screenAspect = screenWidth / screenHeight;

		if (adjustCamSizeToWidth)
		{
			cam.orthographicSize = (desiredAspectRatio / camAspect) * defaultHeight;
		}

		screenToWorldSideScreen = cam.ScreenToWorldPoint(new Vector3(screenWidth, 0, 0));

		if (lvlTapManScript != null) { adjustedMaxX = screenToWorldSideScreen.x - (lvlTapManScript.minCameraSize * camAspect); }
	}
	

	void Update () 
	{
	
		// if (maintainWidth) 
		// {
		// 	Camera.main.orthographicSize = defaultWidth / Camera.main.aspect;
		// 	//CameraPos.y was added in case camera in case camera's y is not in 0
		// 	Camera.main.transform.position= new Vector3(CameraPos.x,CameraPos.y + adaptPosition*(defaultHeight-Camera.main.orthographicSize),CameraPos.z);
		// } 
		// else 
		// {
		// 	//CameraPos.x was added in case camera in case camera's x is not in 0
		// 	Camera.main.transform.position= new Vector3(CameraPos.x + adaptPosition*(defaultWidth-Camera.main.orthographicSize*Camera.main.aspect) ,CameraPos.y,CameraPos.z);
		// }
	}
}