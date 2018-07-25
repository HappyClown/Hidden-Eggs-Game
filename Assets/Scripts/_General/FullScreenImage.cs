using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FullScreenImage : MonoBehaviour 
{
	public RectTransform rectTrans;
	public Camera cam;



	void Start () 
	{
		rectTrans = this.GetComponent<RectTransform>();
	}



	void Update () 
	{
		this.rectTrans.localScale = new Vector3(cam.orthographicSize, cam.orthographicSize, 1f);
	}
}
