using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scale : MonoBehaviour 
{
	[Header("General")]
	public bool isAnItemOnScale;
	public GameObject itemOnScale;
	public GameObject onScaleLastFrame;

	public int weightOnScale;
	public float weightDif;
	//public float timeToAdd;

	[Header("Arrow")]
	public GameObject arrow;
	public float arwRotation;

	public float arwRotSecPerPound;
	public AnimationCurve arwAnimCurve;
	public Animator arrowAnimator;
	public Transform[] arwRots;

	public Quaternion onDropRot;
	public Quaternion curQuat;
	public bool adjustArrowRot;

	public float rotLerpTimer;

	[Header("Plate")]
	public bool adjustPlatePos;
	public GameObject scalePlate;
	public AnimationCurve plateAnimCurve;
	public Animator plateAnimator;
	public Transform[] plateYs;

	public float plateYPos;
	public float iniPlateYPos;
	public float onDropYPos;

	public float posLerpTimer;
	public float platePosPerPound;


	void Start () 
	{
		isAnItemOnScale = false;
	}


	void Update () 
	{
	curQuat = arrow.transform.rotation;

	if (itemOnScale != null && itemOnScale != onScaleLastFrame)
	{
		plateAnimator.Play("New State", 0);
		onDropRot = arrow.transform.rotation;
		onDropYPos = scalePlate.transform.localPosition.y;
		weightDif = Mathf.Abs(weightOnScale - itemOnScale.GetComponent<Items>().weight);
		weightOnScale = itemOnScale.GetComponent<Items>().weight;
		adjustArrowRot = true;
		adjustPlatePos = true;
		//timeToAdd = Time.deltaTime / (arwRotSecPerPound * weightDif);
	}

	if (itemOnScale == null && itemOnScale != onScaleLastFrame)
	{
		//if (arrowAnimator.GetCurrentAnimatorStateInfo(0).IsName("PlateGiggle")) plateAnimator.SetTrigger("Exit");
		//plateAnimator.ResetTrigger("PlateGiggleTrig");
		plateAnimator.Play("New State", 0);
		onDropRot = arrow.transform.rotation;
		onDropYPos = scalePlate.transform.localPosition.y;
		weightDif = Mathf.Abs(weightOnScale - 0);
		weightOnScale = 0;
		adjustArrowRot = true;
		adjustPlatePos = true;
	}

	if (adjustArrowRot)
	{
		AdjustScaleArrow(weightOnScale, onDropRot, weightDif);
	}

	if (adjustPlatePos)
	{
		AdjustScalePlate(weightOnScale, onDropYPos, weightDif);
	}

	onScaleLastFrame = itemOnScale;
	}


	public void AdjustScaleArrow(int weight, Quaternion startRot, float lerpTime)
	{
		rotLerpTimer += Time.deltaTime / (arwRotSecPerPound * lerpTime);
		arrow.transform.rotation = Quaternion.Lerp(startRot, arwRots[weight].rotation, rotLerpTimer); 
		if (rotLerpTimer >= 1f) 
		{ 
			arrow.transform.rotation = arwRots[weight].rotation;
			arrowAnimator.SetTrigger("Wiggle"); 
			adjustArrowRot = false;
			rotLerpTimer = 0f;
			return; 
		}
	}


	public void AdjustScalePlate(int weight, float startY, float lerpTime)
	{
		posLerpTimer += Time.deltaTime / (platePosPerPound * lerpTime);
		plateYPos = Mathf.Lerp(startY, plateYs[weight].localPosition.y, posLerpTimer);
		scalePlate.transform.localPosition = new Vector3(scalePlate.transform.localPosition.x, plateYPos, scalePlate.transform.localPosition.z);
		if (posLerpTimer >= 1f || Mathf.Abs(scalePlate.transform.localPosition.y - plateYs[weight].localPosition.y) < 0.05f)
		{ 
			scalePlate.transform.localPosition = new Vector3(scalePlate.transform.localPosition.x, plateYs[weight].localPosition.y, scalePlate.transform.localPosition.z);
			plateAnimator.Play("PlateGiggle", 0);
			adjustPlatePos = false;
			posLerpTimer = 0f;
			return;
		}
	}
}