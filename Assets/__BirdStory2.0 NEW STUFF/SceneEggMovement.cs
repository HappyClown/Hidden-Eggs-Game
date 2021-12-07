using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneEggMovement : MonoBehaviour {
	public ClickOnEggs clickOnEggsScript;
	public MoveWithCamera moveWithCamScript;
	//public Transform panelParentTransform;
	public SceneEggFXPool fxPool;
	[Header("Click Animation")]
	//public Animator animator;
	public float eggClickAnimLength = 0.5f;
	[Header("Egg Movement")]
	public float moveDuration;
	public AnimationCurve animCurve;
	public Vector3 cornerScale;
	private Vector3 adjustedCornerScale;

	public IEnumerator MoveSceneEggToCorner(GameObject sceneEgg, GameObject panelPosition, int eggNumber) {
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
		Vector3 startPos = new Vector3 (sceneEgg.transform.position.x, sceneEgg.transform.position.y, -5f);
		Vector3 startScale = sceneEgg.transform.localScale;
		float targetZ = startPos.z + 0.5f; 
		Vector3 adjustedPanelPos = new Vector3(panelPosition.transform.position.x, panelPosition.transform.position.y, targetZ);
		fxPool.PlayEggTrailFX(sceneEgg.transform, moveDuration);
		while (timer < 1f) {
			yield return new WaitForEndOfFrame();
			timer += Time.deltaTime/moveDuration;
			adjustedPanelPos = new Vector3(panelPosition.transform.position.x, panelPosition.transform.position.y, targetZ);
			sceneEgg.transform.position = Vector3.Lerp(startPos, adjustedPanelPos, animCurve.Evaluate(timer));
			// Adjust the scale constantly to account for the camera zoom which makes the egg panel change in scale.
			adjustedCornerScale = cornerScale * moveWithCamScript.newScale;
			sceneEgg.transform.localScale = Vector3.Lerp(startScale, adjustedCornerScale, animCurve.Evaluate(timer));
			yield return null;
		}
		// Egg has arrived to its position in the egg panel, signal that one less egg is moving.
		clickOnEggsScript.EggMoving(false);
		sceneEgg.transform.parent = clickOnEggsScript.eggPanel.transform;
		sceneEgg.transform.position = new Vector3(panelPosition.transform.position.x, panelPosition.transform.position.y, panelPosition.transform.position.z-(eggNumber*0.01f));
		sceneEgg.transform.localScale = cornerScale;
	}
}