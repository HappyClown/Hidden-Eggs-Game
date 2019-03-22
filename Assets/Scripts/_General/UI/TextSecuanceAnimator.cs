using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextSecuanceAnimator : MonoBehaviour {

	// Use this for initialization
	public Animator[] animators;
	private int animatorsQuantity, currentItem;
	public float jumpTime, endDelay;
	private float jumpTimer, delayTimer;
	public bool playAnimation, addDelay, instaStart, resetAnimation;
	void Start () {
		UpdateAnimatorList();
		animatorsQuantity = animators.Length;
		currentItem = 0;
		jumpTimer = jumpTime;
		if(instaStart){
			delayTimer = endDelay;		
		}else{
			delayTimer = 0;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(playAnimation){
			if(currentItem == 0 && addDelay && delayTimer < endDelay){
				delayTimer += Time.deltaTime;
			}
			else{
				if(currentItem == 0 && jumpTimer == 0){
					foreach (Animator myAnim in animators)
					{
						myAnim.SetTrigger("ResetAnim");
					}
				}
				jumpTimer += Time.deltaTime;
				if(jumpTimer >= jumpTime){
					animators[currentItem].SetTrigger("AnimateText");
					jumpTimer = 0;
					currentItem ++;
					if(currentItem == animatorsQuantity){
						currentItem = 0;
						delayTimer = 0;
					}
				}
			}
		}
		if(resetAnimation){
			foreach (Animator myAnim in animators)
			{
				myAnim.SetTrigger("ResetAnim");
			}
			currentItem = 0;
			jumpTimer = jumpTime;
			if(instaStart){
				delayTimer = endDelay;		
			}else{
				delayTimer = 0;
			}
			resetAnimation = false;
		}
	}
	public void UpdateAnimatorList()
    {
        animators = GetComponentsInChildren<Animator>();
	}
}
