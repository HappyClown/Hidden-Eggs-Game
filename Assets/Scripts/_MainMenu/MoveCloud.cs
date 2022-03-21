using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCloud : MonoBehaviour 
{
	private bool moveOut;
	private bool moveIn;
	public bool moveLeft;
	public SpriteRenderer cloudSprite;
	public AnimationCurve animCurve;
	private float lerpValue, alphaValue;
	public float moveDuration, moveDurMin, moveDurMax;
	private Vector3 startPos, endPos;
	private Vector3 lerpStart, lerpEnd;
	public float moveX;

	void Start () {
		moveDuration = Random.Range(moveDurMin, moveDurMax);
		startPos = this.transform.position;
		int posNeg = 1;
		if(moveLeft) { 
			posNeg = -1; 
		}
		moveX = moveX * posNeg;	
		endPos = new Vector3(this.transform.position.x + moveX, this.transform.position.y, this.transform.position.z);
	}

	void Update () {
		if (moveIn) {
			if (lerpValue < 1) {
				lerpValue += Time.deltaTime / moveDuration;
				this.transform.position = Vector3.Lerp(lerpStart, lerpEnd, animCurve.Evaluate(lerpValue));
				alphaValue += Time.deltaTime / moveDuration;
				cloudSprite.color = new Color(1, 1, 1, animCurve.Evaluate(alphaValue));
			}
			else {
				moveIn = false;
				lerpValue = 0;
				alphaValue = 1;
			}
		}
		if (moveOut) {
			if (lerpValue < 1){
				lerpValue += Time.deltaTime / moveDuration;
				this.transform.position = Vector3.Lerp(lerpStart, lerpEnd, animCurve.Evaluate(lerpValue));
				alphaValue -= Time.deltaTime / moveDuration;
				cloudSprite.color = new Color(1, 1, 1, animCurve.Evaluate(alphaValue));
			}
			else {
				moveOut = false;
				lerpValue = 0;
				alphaValue = 0;
				this.gameObject.SetActive(false);
			}
		}
	}

	public void MoveIn() {
		this.gameObject.SetActive(true);
		moveDuration = Random.Range(moveDurMin, moveDurMax);
		moveOut = false;
		moveIn = true;
		lerpStart = endPos;
		lerpEnd = startPos;
		lerpValue = 0;
		alphaValue = 0;
	}

	public void MoveOut() {
		moveDuration = Random.Range(moveDurMin, moveDurMax);
		moveOut = true;
		moveIn = false;
		lerpStart = startPos;
		lerpEnd = endPos;
		lerpValue = 0;
		alphaValue = 1;
	}
}
