using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneEggTrailFXPoolObject : MonoBehaviour {
	public bool inUse;
	public ParticleSystem partSys;
	Transform parentPool;
	public void PlayFX (Transform parentObj, float trailDuration) {
		this.gameObject.SetActive(true);
		parentPool = this.transform.parent;
		this.transform.position = parentObj.position;
		this.transform.parent = parentObj;
		StartCoroutine(TrailFXRoutine(parentObj, trailDuration));
	}
	public IEnumerator TrailFXRoutine(Transform parentObj, float trailDuration) {
		inUse = true;
		ParticleSystem.MainModule mainModule = partSys.main;
		mainModule.duration = trailDuration;
		partSys.Play();
		yield return new WaitForSeconds(partSys.main.duration+partSys.main.startLifetime.constant);
		partSys.Stop();
		this.gameObject.SetActive(false);
		this.transform.parent = parentPool;
		this.transform.localScale = Vector3.one;
		inUse = false;
	}
}
