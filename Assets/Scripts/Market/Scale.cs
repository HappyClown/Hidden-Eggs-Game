using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scale : MonoBehaviour 
{
	public bool isAnItemOnScale;
	public GameObject itemOnScale;
	public GameObject onScaleLastFrame;

	public GameObject arrow;
	public float arrowRotation;
	public float arrowRotSpd;
	public AnimationCurve arwAnimCurve;

	public Animator anim;

	public Transform[] arwRots;

	public GameObject scalePlate;



	void Start () 
	{
		isAnItemOnScale = false;
	}
	


	void Update () 
	{
		anim.SetInteger("Weight", 99);


		if (itemOnScale != null && itemOnScale != onScaleLastFrame)
		{
			anim.SetInteger("Weight", itemOnScale.GetComponent<Items>().weight);
		}
		else if (itemOnScale == null && itemOnScale != onScaleLastFrame)
		{
			anim.SetInteger("Weight", 0); 
		}


		onScaleLastFrame = itemOnScale;

		if (itemOnScale == null) 
		{ 
			onScaleLastFrame = null;
		}

	}
}

			// if (itemOnScale.GetComponent<Items>().weight == 0) 
			// //{ arrow.transform.rotation = Quaternion.Lerp(arrow.transform.rotation, arwRots[0].rotation, Time.deltaTime * arrowRotSpd); }
			// {anim.SetInteger("Weight", 0);}
			// if (itemOnScale.GetComponent<Items>().weight == 1)
			// //{ arrow.transform.rotation = Quaternion.Lerp(arrow.transform.rotation, arwRots[1].rotation, Time.deltaTime * arrowRotSpd); }
			// {anim.SetInteger("Weight", 1);}
			// if (itemOnScale.GetComponent<Items>().weight == 2)
			// //{ arrow.transform.rotation = Quaternion.Lerp(arrow.transform.rotation, arwRots[2].rotation, Time.deltaTime * arrowRotSpd); }
			// {anim.SetInteger("Weight", 2);}
			// if (itemOnScale.GetComponent<Items>().weight == 3)
			// //{ arrow.transform.rotation = Quaternion.Lerp(arrow.transform.rotation, arwRots[3].rotation, Time.deltaTime * arrowRotSpd); }
			// {anim.SetInteger("Weight", 3);}
			// if (itemOnScale.GetComponent<Items>().weight == 4)
			// //{ arrow.transform.rotation = Quaternion.Lerp(arrow.transform.rotation, arwRots[4].rotation, Time.deltaTime * arrowRotSpd); }
			// {anim.SetInteger("Weight", 4);}
			// if (itemOnScale.GetComponent<Items>().weight == 5)
			// //{ arrow.transform.rotation = Quaternion.Lerp(arrow.transform.rotation, arwRots[5].rotation, Time.deltaTime * arrowRotSpd); }
			// {anim.SetInteger("Weight", 5);}
			// if (itemOnScale.GetComponent<Items>().weight == 6)
			// //{ arrow.transform.rotation = Quaternion.Lerp(arrow.transform.rotation, arwRots[6].rotation, Time.deltaTime * arrowRotSpd); }
			// {anim.SetInteger("Weight", 6);}