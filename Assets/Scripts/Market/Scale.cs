using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scale : MonoBehaviour 
{
	public bool isAnItemOnScale;
	public GameObject itemOnScale;
	public GameObject onScaleLastFrame;

	public GameObject arrow;
	public float arwRotation;

	public float arwRotSecPerPound;
	public AnimationCurve arwAnimCurve;
	public Animator anim;
	public Transform[] arwRots;
	
	public GameObject scalePlate;

	public Quaternion onDropRot;
	public Quaternion curQuat;
	public bool adjustArrowRot;
	public int weightOnScale;

	public float rotLerpTimer;
	public float weightDif;
	public float timeToAdd;


	void Start () 
	{
		isAnItemOnScale = false;
	}


	void Update () 
	{
	// 	anim.SetInteger("Weight", 99);
	// 	if (itemOnScale != null/*  && itemOnScale != onScaleLastFrame */)
	// 	{
	// 		anim.SetInteger("Weight", itemOnScale.GetComponent<Items>().weight);
	// 	}
	// 	else if (itemOnScale == null && itemOnScale != onScaleLastFrame)
	// 	{
	// 		anim.SetInteger("Weight", 0); 
	// 	}

	curQuat = arrow.transform.rotation;

	if (itemOnScale != null && itemOnScale != onScaleLastFrame)
	{
		onDropRot = arrow.transform.rotation;
		weightDif = Mathf.Abs(weightOnScale - itemOnScale.GetComponent<Items>().weight);
		weightOnScale = itemOnScale.GetComponent<Items>().weight;
		adjustArrowRot = true;
		//timeToAdd = Time.deltaTime / (arwRotSecPerPound * weightDif);
	}

	if (itemOnScale == null && itemOnScale != onScaleLastFrame)
	{
		onDropRot = arrow.transform.rotation;
		weightDif = Mathf.Abs(weightOnScale - 0);
		weightOnScale = 0;
		adjustArrowRot = true;
	}

	if (adjustArrowRot)
	{
		AdjustScaleArrow(weightOnScale, onDropRot, weightDif);
	}

	onScaleLastFrame = itemOnScale;

	
	// float angleDif = Quaternion.Angle(arrow.transform.rotation, arwRots[weightOnScale].rotation);
	// if (weightDif <= 0) { weightDif = Mathf.Round(angleDif / 43); Debug.Log(weightDif);} // OR = 1 (optimisation vs accuracy if they spam click)

	// GO BACK TO ZERO WHEN NOTHING IS ON THE SCALE

	// 	if (itemOnScale == null) 
	// 	{ 
	// 		onScaleLastFrame = null;
	// 		anim.SetInteger("Weight", 0); 
	// 	}
	}

	public void AdjustScaleArrow(int weight, Quaternion startRot, float lerpTime)
	{
			// if (itemOnScale.GetComponent<Items>().weight == 0) 
			// { 
			// 	arrow.transform.rotation = Quaternion.Lerp(arrow.transform.rotation, arwRots[0].rotation, Time.deltaTime * arrowRotSpd); 
			// 	return;
			// }
			//{anim.SetInteger("Weight", 0);}
			// else if (weight == 1)
			// { 
				/* if (arrow.transform.rotation != arwRots[1].rotation) */ 
		rotLerpTimer += Time.deltaTime / (arwRotSecPerPound * lerpTime);
		arrow.transform.rotation = Quaternion.Lerp(startRot, arwRots[weight].rotation, rotLerpTimer); 
		if (Quaternion.Angle(arrow.transform.rotation, arwRots[weight].rotation) < 0.1f) { arrow.transform.rotation = arwRots[weight].rotation; anim.SetTrigger("Wiggle"); adjustArrowRot = false; rotLerpTimer = 0f; return; }
			// }
			// {anim.SetInteger("Weight", 1);}
			// else if (itemOnScale.GetComponent<Items>().weight == 2)
			// { arrow.transform.rotation = Quaternion.Lerp(arrow.transform.rotation, arwRots[2].rotation, Time.deltaTime * arrowRotSpd); return;}
			// //{anim.SetInteger("Weight", 2);}
			// else if (itemOnScale.GetComponent<Items>().weight == 3)
			// { arrow.transform.rotation = Quaternion.Lerp(arrow.transform.rotation, arwRots[3].rotation, Time.deltaTime * arrowRotSpd); return;}
			// //{anim.SetInteger("Weight", 3);}
			// else if (itemOnScale.GetComponent<Items>().weight == 4)
			// { arrow.transform.rotation = Quaternion.Lerp(arrow.transform.rotation, arwRots[4].rotation, Time.deltaTime * arrowRotSpd); return;}
			// //{anim.SetInteger("Weight", 4);}
			// else if (itemOnScale.GetComponent<Items>().weight == 5)
			// { arrow.transform.rotation = Quaternion.Lerp(arrow.transform.rotation, arwRots[5].rotation, Time.deltaTime * arrowRotSpd); return;}
			// //{anim.SetInteger("Weight", 5);}
			// else if (itemOnScale.GetComponent<Items>().weight == 6)
			// { arrow.transform.rotation = Quaternion.Lerp(arrow.transform.rotation, arwRots[6].rotation, Time.deltaTime * arrowRotSpd); return;}
			// //{anim.SetInteger("Weight", 6);}
	}
}