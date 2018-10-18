using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentToOnAwake : MonoBehaviour 
{
	public Transform myParent;

	void Awake () 
	{
		this.transform.parent = myParent;
	}
}