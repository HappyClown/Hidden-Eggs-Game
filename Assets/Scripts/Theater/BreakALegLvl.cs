using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BreakALegLvl : MonoBehaviour {

	public TeatherBag Bag1,Bag2,Bag3;
	public Image SmallCircle;
	public float initialRotation;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void MoveBag1(bool up){
		if(up){
			if(!Bag1.up){
				RotateCircle(Bag1.Value*-1);				
				Bag1.MoveBagUp();
			}
		}else{
			if(!Bag1.down){
				RotateCircle(Bag1.Value);
				Bag1.MoveBagDown();
			}
		}
	}
	public void MoveBag2(bool up){
		if(up){
			if(!Bag2.up){
				RotateCircle(Bag2.Value*-1);
				Bag2.MoveBagUp();
			}
		}else{
			if(!Bag2.down){
				RotateCircle(Bag2.Value);
				Bag2.MoveBagDown();				
			}
		}
	}
	public void MoveBag3(bool up){
		if(up){
			if(!Bag3.up){
				RotateCircle(Bag3.Value*-1);
				Bag3.MoveBagUp();
			}
		}else{
			if(!Bag3.down){
				RotateCircle(Bag3.Value);
				Bag3.MoveBagDown();				
			}
		}
	}
	public void RotateCircle(float amount){
		Vector3 currentRot = SmallCircle.rectTransform.rotation.eulerAngles;
		currentRot += new Vector3(0,0,amount);
		SmallCircle.rectTransform.rotation = Quaternion.Euler(currentRot);
	}
	public void SetUpLvl(){
		SmallCircle.rectTransform.rotation = Quaternion.Euler(0,0,-initialRotation);
	}
}
