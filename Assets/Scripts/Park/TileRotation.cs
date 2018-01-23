using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileRotation : MonoBehaviour 
{


	public int initialRot;

	public float zRotation;


	void Start () 
	{
		zRotation = this.transform.rotation.z;

		initialRot = Random.Range(0,4);

		if (initialRot == 0) { zRotation = 0; } else if (initialRot == 1) { zRotation = 90; } else if (initialRot == 2) { zRotation = 180; } else if (initialRot == 3) { zRotation = 270; }

		this.transform.eulerAngles = new Vector3(this.transform.rotation.x, this.transform.rotation.y, zRotation);

	}

}
