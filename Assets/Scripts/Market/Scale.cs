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

	private bool setTemporalRot = false;
	private float temporalTime = 0f;
	private Quaternion tempCurrentRot = Quaternion.identity;


	void Start () 
	{
		isAnItemOnScale = false;
	}


	void Update () 
	{
	curQuat = arrow.transform.localRotation;

	if (itemOnScale != null && itemOnScale != onScaleLastFrame)
	{
		plateAnimator.Play("New State", 0);
		onDropRot = arrow.transform.localRotation;
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
		onDropRot = arrow.transform.localRotation;
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
		
		if(lerpTime > 4)
		{
			float secTime = Mathf.Abs((3 - weight)/lerpTime);
			float restTime = 1 - secTime;
			temporalTime += (Time.deltaTime /(arwRotSecPerPound *lerpTime * secTime ));
			Debug.Log("temporalTime: "+temporalTime.ToString()+"  secTime: "+secTime.ToString()+" weight: "+weight.ToString());
			if(temporalTime <= 1){
				arrow.transform.localRotation = Quaternion.Lerp(startRot, arwRots[3].localRotation, temporalTime); 
			}else{
				if(!setTemporalRot){				
					tempCurrentRot = arrow.transform.localRotation;
					setTemporalRot = true;
				}
				rotLerpTimer += Time.deltaTime / (arwRotSecPerPound * lerpTime * restTime);
				arrow.transform.localRotation = Quaternion.Lerp(tempCurrentRot, arwRots[weight].localRotation, rotLerpTimer); 
				if (rotLerpTimer >= 1f) { 
					arrow.transform.localRotation = arwRots[weight].localRotation;
					arrowAnimator.SetTrigger("Wiggle"); 
					adjustArrowRot = false;
					rotLerpTimer = 0f;
					setTemporalRot = false;
					tempCurrentRot = Quaternion.identity;
					temporalTime = 0f;
					return; 
				}
			}
		}
		else{
			rotLerpTimer += Time.deltaTime / (arwRotSecPerPound * lerpTime);
			arrow.transform.localRotation = Quaternion.Lerp(startRot, arwRots[weight].localRotation, rotLerpTimer); 
			if (rotLerpTimer >= 1f) 
			{ 
				arrow.transform.localRotation = arwRots[weight].localRotation;
				arrowAnimator.SetTrigger("Wiggle"); 
				adjustArrowRot = false;
				rotLerpTimer = 0f;
				setTemporalRot = false;
				tempCurrentRot = Quaternion.identity;
				temporalTime = 0f;
				return; 
			}
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