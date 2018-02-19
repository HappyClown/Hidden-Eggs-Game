using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scale : MonoBehaviour 
{
	public bool isAnItemOnScale;
	public GameObject itemOnScale;

	//public Scale scaleScript;

	public GameObject arrow;
	public float arrowRotation;

	void Start () 
	{
		isAnItemOnScale = false;
	}
	
	void Update () 
	{
		//float itemRotZ = itemOnScale.transform.rotation.z;

		if (itemOnScale != null)
		{
			if (itemOnScale.GetComponent<Items>().weight == 0) { arrowRotation = 0f; }
			if (itemOnScale.GetComponent<Items>().weight == 1) { arrowRotation = -36.75f; }
			if (itemOnScale.GetComponent<Items>().weight == 2) { arrowRotation = -72.75f; }
			if (itemOnScale.GetComponent<Items>().weight == 3) { arrowRotation = -108.75f; }
			if (itemOnScale.GetComponent<Items>().weight == 4) { arrowRotation = -144.75f; }
			if (itemOnScale.GetComponent<Items>().weight == 5) { arrowRotation = -180.75f; }
			if (itemOnScale.GetComponent<Items>().weight == 6) { arrowRotation = -216.75f; }
			
		}
		else { arrowRotation = 0f; }

		arrow.transform.eulerAngles = new Vector3(arrow.transform.rotation.x, arrow.transform.rotation.y, arrowRotation);
	}
}
