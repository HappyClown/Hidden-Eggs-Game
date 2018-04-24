using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjInUse : MonoBehaviour 
{
	public Vector3 originPos;
	public GameObject objToFollow;
	public bool inUse;



	void Start () 
	{
		originPos = this.transform.position;
	}
	


	void Update () 
	{
		if (objToFollow != null)
		{
			this.transform.position = new Vector3(objToFollow.transform.position.x, objToFollow.transform.position.y, -3.1f);
		}

		if (this.transform.position != originPos)
		{
			inUse = true;
		}
		else
		{
			inUse = false;
		}
	}
}
