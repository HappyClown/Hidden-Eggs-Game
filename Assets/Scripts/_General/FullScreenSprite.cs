using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullScreenSprite : MonoBehaviour 
{
	private Transform trans;



	void Start () 
	{
		trans = this.GetComponent<Transform>();
		
	}



	void Update () 
	{
		this.trans.localScale = new Vector3(Screen.width, Screen.height, 1f);
	}

}