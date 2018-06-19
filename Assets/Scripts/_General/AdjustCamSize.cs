
using UnityEngine;
using System.Collections;

public class AdjustCamSize : MonoBehaviour 
{
	public bool maintainWidth=true;
	[Range(-1,1)]
	public int adaptPosition;

	float defaultWidth;
	float defaultHeight;

	Vector3 CameraPos;



	void Start () 
	{
	
		CameraPos = Camera.main.transform.position;

		defaultHeight = Camera.main.orthographicSize;
		defaultWidth = Camera.main.orthographicSize * Camera.main.aspect;
	}
	


	void Update () 
	{
	
		if (maintainWidth) 
		{

			Camera.main.orthographicSize = defaultWidth / Camera.main.aspect;


//CameraPos.y was added in case camera in case camera's y is not in 0
			Camera.main.transform.position= new Vector3(CameraPos.x,CameraPos.y + adaptPosition*(defaultHeight-Camera.main.orthographicSize),CameraPos.z);


		} 
		else 
		{
//CameraPos.x was added in case camera in case camera's x is not in 0
			Camera.main.transform.position= new Vector3(CameraPos.x + adaptPosition*(defaultWidth-Camera.main.orthographicSize*Camera.main.aspect) ,CameraPos.y,CameraPos.z);

		}


	}
}