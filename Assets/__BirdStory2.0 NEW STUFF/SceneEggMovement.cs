using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneEggMovement : MonoBehaviour {
	public ClickOnEggs clickOnEggsScript;
	public MoveWithCamera moveWithCamScript;
	public SceneEggFXPool fxPool;
	[Header("Click Animation")]
	//public Animator animator;
	public float eggClickAnimLength = 0.5f;
	[Header("Egg Movement")]
	public float moveDuration;
	public AnimationCurve animCurve;
	public Vector3 cornerScale;

	public IEnumerator MoveSceneEggToCorner(GameObject sceneEgg, GameObject panelPosition) {
		// Ask to do pop animation, grab the animator on the scene egg, enable it, play the animation.
		Animator animator = sceneEgg.GetComponent<Animator>();
		animator.enabled = true;
		animator.SetTrigger("EggPop");
		// Play on Click FX.
		fxPool.PlayEggClickFX(sceneEgg.transform.position);
		// Wait for the egg click animation to finish.
		yield return new WaitForSeconds(eggClickAnimLength);
		animator.enabled = false;
		// Once animation is done, activate the trail FX and move the egg to its given panel position while taking into account panel size changes caused by camera zooming.
		float timer = 0f;
		Vector3 startPos = sceneEgg.transform.position;
		Vector3 startScale = sceneEgg.transform.localScale;
		fxPool.PlayEggTrailFX(sceneEgg.transform, moveDuration);
		while (timer < 1f) {
			timer += Time.deltaTime/moveDuration;
			sceneEgg.transform.position = Vector3.Lerp(startPos, panelPosition.transform.position, animCurve.Evaluate(timer));
			sceneEgg.transform.localScale = Vector3.Lerp(startScale, cornerScale, animCurve.Evaluate(timer));
			yield return null;
		}
		// Egg has arrived to its position in the egg panel, signal that one less egg is moving.
		clickOnEggsScript.EggMoving(false);
	}
}