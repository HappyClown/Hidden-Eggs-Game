
using UnityEngine;
using System.Collections;

public class AdjustCamSize : MonoBehaviour 
{
	//public bool maintainWidth=true;
	//[Range(-1,1)]
	//public int adaptPosition;

	//public float defaultWidth;
	public bool adjustCamSize = true;
	public bool useSafeArea = false;
	public float defaultHeight;
	public float camAspect;
	public float safeCamAspect;

	public float screenWidth;
	public float screenHeight;
	public float safeScreenWidth;
	public float safeScreenHeight;
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
		if (adjustCamSize) {
			if (!cam) { cam = this.GetComponent<Camera>(); }
			defaultHeight = Camera.main.orthographicSize;


			screenWidth = Screen.width;
			screenHeight = Screen.height;
			camAspect = Camera.main.aspect;

			safeScreenWidth = Screen.safeArea.width;
			safeScreenHeight = Screen.safeArea.height;
			safeCamAspect = safeScreenWidth/safeScreenHeight;
			
			// Only adjust the camera size if the aspect ratio is smaller (shape of the screen is more square, then )
			if (useSafeArea) {
				if (safeCamAspect < desiredAspectRatio) {
					if (adjustCamSizeToWidth)
					{
						cam.orthographicSize = (desiredAspectRatio / safeCamAspect) * defaultHeight;
					}

					screenToWorldSideScreen = cam.ScreenToWorldPoint(new Vector3(safeScreenWidth, 0, 0));

					if (lvlTapManScript != null) { adjustedMaxX = screenToWorldSideScreen.x - (lvlTapManScript.minCameraSize * safeCamAspect); }
				}
			}
			else {
				if (camAspect < desiredAspectRatio) {
					if (adjustCamSizeToWidth)
					{
						cam.orthographicSize = (desiredAspectRatio / camAspect) * defaultHeight;
					}

					screenToWorldSideScreen = cam.ScreenToWorldPoint(new Vector3(screenWidth, 0, 0));

					if (lvlTapManScript != null) { adjustedMaxX = screenToWorldSideScreen.x - (lvlTapManScript.minCameraSize * camAspect); }
				}
			}
		}
	}
	

	// void Update () 
	// {
	
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
	// }
}