using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldenEggAnimBehave : StateMachineBehaviour {
	// public SpriteRenderer whiteSprite;

	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	//override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	//override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		// whiteSprite = animator.gameObject.GetComponent<GoldenEggAnimEvents>().overlaySprite;
		// // animator.transform.parent.position = animator.transform.position;
		// // animator.transform.parent.localScale = animator.transform.localScale;
		// // animator.transform.parent.eulerAngles = animator.transform.eulerAngles;
		// whiteSprite.color = new Color(whiteSprite.color.r, whiteSprite.color.g, whiteSprite.color.b, 0f);
		// Debug.Log(whiteSprite.gameObject.name);
		// Debug.Log(animator.gameObject.GetComponent<GoldenEggAnimEvents>().overlaySprite.color);
	}

	// OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
	//override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
	//override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}
}
