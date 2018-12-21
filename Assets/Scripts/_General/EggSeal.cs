using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggSeal : MonoBehaviour {
	public Animator anim;

	void PlayEggSeal() {
		anim.SetTrigger("PlaySeal");
	}
}