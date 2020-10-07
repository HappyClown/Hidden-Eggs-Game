using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneEggFXPool : MonoBehaviour {
	public GameObject eggClickFXPrefab;
	public GameObject eggTrailFXPrefab;
	public List<SceneEggClickFXPoolObject> eggClickFXPoolObjects = new List<SceneEggClickFXPoolObject>();
	public List<SceneEggTrailFXPoolObject> eggTrailFXPoolObjects = new List<SceneEggTrailFXPoolObject>();

	public void PlayEggClickFX(Vector2 position) {
		// This requests an Egg Click FX and makes its particle system play.
		RequestEggClickFX().PlayFX(position);
	}

	public SceneEggClickFXPoolObject RequestEggClickFX() {
		// Look for a pool object that is not currently in use.
		for(int i = 0; i < eggClickFXPoolObjects.Count; i++) {
			if (!eggClickFXPoolObjects[i].inUse) {
				return eggClickFXPoolObjects[i];
			}
		}
		// If all the effects in the list are already in use, create a new one, add it to the list and return it.
		SceneEggClickFXPoolObject newPoolObj = Instantiate(eggClickFXPrefab, Vector3.zero, Quaternion.identity, this.transform).GetComponent<SceneEggClickFXPoolObject>();
		eggClickFXPoolObjects.Add(newPoolObj);
		return newPoolObj;
	}

	public void PlayEggTrailFX(Transform parentObj, float trailDuration) {
		// This requests an Egg Click FX and makes its particle system play.
		RequestEggTrailFX().PlayFX(parentObj, trailDuration);
	}

	public SceneEggTrailFXPoolObject RequestEggTrailFX() {
		// Look for a pool object that is not currently in use.
		for(int i = 0; i < eggTrailFXPoolObjects.Count; i++) {
			if (!eggTrailFXPoolObjects[i].inUse) {
				return eggTrailFXPoolObjects[i];
			}
		}
		// If all the effects in the list are already in use, create a new one, add it to the list and return it.
		SceneEggTrailFXPoolObject newPoolObj = Instantiate(eggTrailFXPrefab, Vector3.zero, Quaternion.identity, this.transform).GetComponent<SceneEggTrailFXPoolObject>();
		eggTrailFXPoolObjects.Add(newPoolObj);
		return newPoolObj;
	}
}
