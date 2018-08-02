using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FullScreenImage : MonoBehaviour 
{
	public RectTransform rectTrans;



	void Start () 
	{
		rectTrans = this.GetComponent<RectTransform>();
		
	}



	void Update () 
	{
		this.rectTrans.localScale = new Vector3(Screen.width, Screen.height, 1f);
	}



}
