using System;
using UnityEngine;

[Serializable]
public class RotateVariables
{
	public bool rotateInX, rotateInY, rotateInZ; 
	public float rotationSpeedX, rotationSpeedY, rotationSpeedZ;
}

public class RotateXYZ : MonoBehaviour 
{
	
	private float rotationX, rotationY, rotationZ;
	public RotateVariables rotations;



	void Update () 
	{
		if (rotations.rotateInX)
		{
			rotationX += Time.deltaTime * rotations.rotationSpeedX; 
			transform.eulerAngles = new Vector3(rotationX, rotationY, rotationZ) ;
		}

		if (rotations.rotateInY)
		{
			rotationY += Time.deltaTime * rotations.rotationSpeedY; 
			transform.eulerAngles = new Vector3(rotationX, rotationY, rotationZ) ;
		}

		if (rotations.rotateInZ)
		{
			rotationZ += Time.deltaTime * rotations.rotationSpeedZ; 
			transform.eulerAngles = new Vector3(rotationX, rotationY, rotationZ) ;
		}
	}
}
