using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingClam : MonoBehaviour {
	public Animator anim;
	public float minWait, maxWait;
	private float jumpTimer, curWait;
	private int clamNum;
	public bool active;

	void Update () {
		if (active) {
			jumpTimer += Time.deltaTime;
			if (clamNum > 0) {
				clamNum = 0;
				anim.SetInteger("ClamJump", clamNum);
				Debug.Log("Hello? Should set anim int to dzéro!");
			}
			if (jumpTimer >= curWait) {
				clamNum = Mathf.RoundToInt(Random.Range(1, 4));
				anim.SetInteger("ClamJump", clamNum);
				curWait = Random.Range(minWait, maxWait);
				jumpTimer = 0f;
			}
		}
	}

	public void StartClamAnim() {
		curWait = Random.Range(minWait, maxWait);
		active = true;
	}
}
