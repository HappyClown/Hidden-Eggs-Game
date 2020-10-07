using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneEggClickFXPoolObject : MonoBehaviour {
	public bool inUse;
	public ParticleSystem partSys;
	public void PlayFX(Vector2 position) {
		this.transform.position = position;
		this.gameObject.SetActive(true);
		StartCoroutine(ClickFXRoutine());
	}
	public IEnumerator ClickFXRoutine() {
		inUse = true;
		partSys.Play();
		yield return new WaitForSeconds(partSys.main.duration);
		this.gameObject.SetActive(false);
		inUse = false;
	}
}
