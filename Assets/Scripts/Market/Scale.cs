using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scale : MonoBehaviour 
{
	public bool isAnItemOnScale;
	public GameObject itemOnScale;

	public GameObject arrow;
	public float arrowRotation;
	public float arrowRotSpd;
	public AnimationCurve arwAnimCurve;

	public Animator anim;

	public Transform[] arwRots;



	void Start () 
	{
		isAnItemOnScale = false;
	}
	


	void Update () 
	{
		anim.SetInteger("Weight", 11);

		if (itemOnScale != null)
		{
			if (itemOnScale.GetComponent<Items>().weight == 0) 
			//{ arrow.transform.rotation = Quaternion.Lerp(arrow.transform.rotation, arwRots[0].rotation, Time.deltaTime * arrowRotSpd); }
			{anim.SetInteger("Weight", 0); itemOnScale = null; return;}
			if (itemOnScale.GetComponent<Items>().weight == 1)
			//{ arrow.transform.rotation = Quaternion.Lerp(arrow.transform.rotation, arwRots[1].rotation, Time.deltaTime * arrowRotSpd); }
			{anim.SetInteger("Weight", 1); itemOnScale = null; return;}
			if (itemOnScale.GetComponent<Items>().weight == 2)
			//{ arrow.transform.rotation = Quaternion.Lerp(arrow.transform.rotation, arwRots[2].rotation, Time.deltaTime * arrowRotSpd); }
			{anim.SetInteger("Weight", 2); itemOnScale = null; return;}
			if (itemOnScale.GetComponent<Items>().weight == 3)
			//{ arrow.transform.rotation = Quaternion.Lerp(arrow.transform.rotation, arwRots[3].rotation, Time.deltaTime * arrowRotSpd); }
			{anim.SetInteger("Weight", 3); itemOnScale = null; return;}
			if (itemOnScale.GetComponent<Items>().weight == 4)
			//{ arrow.transform.rotation = Quaternion.Lerp(arrow.transform.rotation, arwRots[4].rotation, Time.deltaTime * arrowRotSpd); }
			{anim.SetInteger("Weight", 4); itemOnScale = null; return;}
			if (itemOnScale.GetComponent<Items>().weight == 5)
			//{ arrow.transform.rotation = Quaternion.Lerp(arrow.transform.rotation, arwRots[5].rotation, Time.deltaTime * arrowRotSpd); }
			{anim.SetInteger("Weight", 5); itemOnScale = null; return;}
			if (itemOnScale.GetComponent<Items>().weight == 6)
			//{ arrow.transform.rotation = Quaternion.Lerp(arrow.transform.rotation, arwRots[6].rotation, Time.deltaTime * arrowRotSpd); }
			{anim.SetInteger("Weight", 6); itemOnScale = null; return;}
			
		}
		//else //{ arrow.transform.rotation = Quaternion.Lerp(arrow.transform.rotation, arwRots[0].rotation, Time.deltaTime * arrowRotSpd); }
		//{anim.SetInteger("Weight", 0); itemOnScale = null;}

	}
}
