using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Items : MonoBehaviour 
{
	public int weight;
	public Vector3 initialPos;
	public bool inCrate;

	void Start () 
	{
		initialPos = this.transform.position;
		inCrate = false;
	}
	
	void Update () 
	{
		
	}

	public void BackToInitialPos ()
	{
		this.transform.position = initialPos;
		this.inCrate = false;
	}
}
